using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using GeoJSON.Net;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System.Threading;
//using BAMCIS.GeoJSON;
using NLog;



namespace IntersectionTest
{
    public partial class Form1 : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static Double MinV = 1.0;
        private static Double Distance = 1.0;
        private static int ACount = 0;
        private static int FinishedCount = 0;
        private static int Total = 0;


        private static object optLocker = new object();
        //private static object jsonLocker = new object();
        // private static string header=@"""TrackID"";""RecordTime"";""N"";""E"";""Velocity"";""Marker""";
        public Form1()
        {
            InitializeComponent();
        }
        //private List<Intersections> iList = null;

       

        private void Button2_Click(object sender, EventArgs e)
        {
            if (opf.ShowDialog() == DialogResult.OK)
            {
                cmdCSV.Enabled = false;
                grpParam.Enabled = false;
                FinishedCount = 0;
                ACount = 0;
                int show = 0;
                CultureInfo ci = new CultureInfo("en-US");

                // get parameters
                try
                {
                    MinV = Double.Parse(txtMinV.Text);
                }
                catch
                {
                    MinV = 3.0;
                }

                try
                {
                    Distance = Double.Parse(txtDistance.Text);
                }
                catch
                {
                    Distance = 5.0;
                }

                try
                {
                    StopTime = int.Parse(txtStopTime.Text);
                }
                catch
                {
                    StopTime = 5;
                }

                FileInfo fi = new FileInfo(opf.FileName);

                String path2save = fi.DirectoryName;
                path2save += "\\Tracks";


                // clean directory
                if (Directory.Exists(path2save))
                {
                    try { Directory.Delete(path2save, true); }
                    catch { }
                }
                int cnt = 100;
                while (Directory.Exists(path2save) && cnt > 0)
                {
                    cnt--;
                    label1.Text = " Удаление файлов " + DateTime.Now;
                    Application.DoEvents();

                }


                // create directoies
                if (!Directory.Exists(path2save))
                {
                    try { Directory.CreateDirectory(path2save); }
                    catch { }
                }

                if (!Directory.Exists(path2save + "\\opt"))
                {
                    try { Directory.CreateDirectory(path2save + "\\opt"); }
                    catch { }
                }

                if (!Directory.Exists(path2save + "\\lnk"))
                {
                    try { Directory.CreateDirectory(path2save + "\\lnk"); }
                    catch { }
                }

                //if (!Directory.Exists(path2save + "\\GeoJSON"))
                //{
                //    try { Directory.CreateDirectory(path2save + "\\GeoJSON"); }
                //    catch { }
                //}


                // read raw file and split to individual tracks
                var it = File.ReadLines(fi.FullName);

                string TrackID = "";
                string sNE;
                Double N = 0.0, E = 0.0, V = 0.0;


                Dictionary<string, List<string>> Reorg = new Dictionary<string, List<string>>();

                logger.Info("Read tracks from " + fi.FullName);
                // pass 1
                foreach (string s in it)
                {
                    string[] cols = s.Split(';');
                    if (cols.Length >= 8)
                    {
                        TrackID = cols[0];
                        N = Double.Parse(cols[4], ci);
                        E = Double.Parse(cols[5], ci);
                        V = Double.Parse(cols[6], ci);
                        N = N * 180 / Math.PI;
                        E = E * 180 / Math.PI;

                        sNE = cols[0] + ";" + cols[3] + ";" + N.ToString(ci) + ";" + E.ToString(ci) + ";" + cols[6];

                        if (Reorg.ContainsKey(TrackID))
                        {
                            Reorg[TrackID].Add(sNE);
                        }
                        else
                        {
                            List<string> newList = new List<string>();
                            newList.Add(sNE);
                            Reorg.Add(TrackID, newList);
                        }
                    }
                    Total++;
                    show++;
                    if (show == 500)
                    {
                        label1.Text = "Строк: " + Total.ToString();
                        Application.DoEvents();
                        show = 0;
                    }
                }

                label1.Text = "Строк: " + Total.ToString();
                Application.DoEvents();

                Total = 0;
                show = 0;
                logger.Info("Saveing reorganized tracks to individual files");
                foreach (string s in Reorg.Keys)
                {
                    File.AppendAllLines(path2save + "\\" + s + ".csv", Reorg[s]);
                    Reorg[s].Clear();
                    Total++;
                    show++;
                    if (show == 100)
                    {
                        label1.Text = "Записано треков: " + Total.ToString();
                        Application.DoEvents();
                        show = 0;
                    }
                }
                Reorg.Clear();

               logger.Info("Записано треков: " + Total.ToString());

               label1.Text = "Записано треков: " + Total.ToString();
                Application.DoEvents();


                // pass2
                List<TrackPoint> rawTrack = new List<TrackPoint>();
                ThreadPool.SetMinThreads(50, 0);
                ThreadPool.SetMaxThreads(200, 0);
                DirectoryInfo di = new DirectoryInfo(path2save + "\\");

                logger.Info("Analizing traks");
                Total = 0;
                timer1.Enabled = true;
                foreach (FileInfo tFile in di.GetFiles("*.csv"))
                {
                    AState a = new AState()
                    {
                        FileName = tFile.FullName,
                        SavePath = path2save
                    };
                    ACount++;
                    ThreadPool.QueueUserWorkItem(AnalizeTrackQ, a);
                }


                // wait while pass2 finished
                while (ACount > 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Application.DoEvents();
                }

                // pass 3
                FinishedCount = 0;
                ACount = 0;

                di = new DirectoryInfo(path2save + "\\opt");
                timer2.Enabled = true;
                {

                    startTime = DateTime.Now;

                    ThreadPool.SetMinThreads(20, 0);
                    ThreadPool.SetMaxThreads(40, 0);

                    foreach (FileInfo fi2 in di.GetFiles("*.csv"))
                    {
                        fCnt += 1;
                        AState a = new AState()
                        {
                            FileName = fi2.FullName,
                            SavePath = fi2.DirectoryName
                        };
                        ACount++;
                        ThreadPool.QueueUserWorkItem(LinkFile, a);
                    }


                }

            }
        }

        private static int StopTime = 1;


        // режем на поездки
        private static List<List<TrackPoint>> SptitTrack(List<TrackPoint> Track)
        {
            List<List<TrackPoint>> l = new List<List<TrackPoint>>();


            if (Track.Count < 2)
            {
                l.Add(Track);
                return l;
            }

            List<TrackPoint> cur = new List<TrackPoint>();
            TrackPoint LastMovePoint = Track[0];
            foreach (TrackPoint tt in Track)
            {
                // если скорость зафиксирована, то используем её
                if (tt.V > MinV)
                {
                    LastMovePoint = tt;
                    cur.Add(tt);
                }
                else
                {
                    // возможно  скорость просто не фиксируется прибором
                    double d = Processors.DistanceOnEarth(tt.X, tt.Y, LastMovePoint.X, LastMovePoint.Y);
                    if (d > Distance)
                    {
                        // если есть перемещение, то продолжаем записывать его в текущую поездку
                        LastMovePoint = tt;
                        cur.Add(tt);
                    }
                    else
                    {
                        // перемещение не зафиксировано
                        // проверяем как долго стоит машина
                        if (Math.Abs((tt.T - LastMovePoint.T).TotalMinutes) > StopTime)
                        {
                            // определили остановку, фиксируем поездку
                            if (cur.Count > 0)
                                l.Add(cur);

                            // дальше будет уже следующая
                            cur = new List<TrackPoint>();
                        }
                    }



                }

            }
            if (cur.Count > 0)
                l.Add(cur);
            return l;
        }


        private static void AnalizeTrackQ(object state)
        {
            string TrackID = "";
            Double N = 0.0, E = 0.0, V = 0.0;
            CultureInfo ci = new CultureInfo("en-US");

            AState a = (AState)state;
            List<TrackPoint> rawTrack = new List<TrackPoint>();
            try
            {


                var trackData = File.ReadLines(a.FileName);
                DateTime d = DateTime.MinValue;

                foreach (string s in trackData)
                {
                    string[] cols = s.Split(';');


                    if (cols.Length >= 5)
                    {
                        TrackID = cols[0];
                        N = Double.Parse(cols[2], ci);
                        E = Double.Parse(cols[3], ci);
                        V = Double.Parse(cols[4], ci);

                        if (cols[1].Length == 23)
                        {
                            try
                            {
                                d = DateTime.ParseExact(cols[1], "yyyy-MM-dd HH:mm:ss.fff", ci);
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            try
                            {
                                d = DateTime.ParseExact(cols[1], "yyyy-MM-dd HH:mm:ss", ci);
                            }
                            catch
                            {
                            }
                        }

                        TrackPoint tp = new TrackPoint() { X = N, Y = E, V = V, T = d };
                        rawTrack.Add(tp);
                    }
                }



                AnalizeTrack(rawTrack, TrackID, a.SavePath);

            }catch(System.Exception ex)
            {
                logger.Error(ex, "AnalizeQ for " + a.FileName);
            }
            ACount--;
            FinishedCount++;
        }

        private static void AnalizeTrack(List<TrackPoint> Track, string TrackID, string path2save)
        {
            CultureInfo ci = new CultureInfo("en-US");

            // сортируем список по времени
            Track.Sort();

            // разбиваем на поездки
            List<List<TrackPoint>> l = SptitTrack(Track);
            int driveIndex = 0;
            foreach (List<TrackPoint> t in l)
            {
                Total++;
                driveIndex++;
                try
                {

                    List<string> optimizedTrack; // = new List<string>();

                    //foreach (TrackPoint tp in t)
                    //{
                    //    optimizedTrack.Add(TrackID + "." + driveIndex.ToString() + ";" + tp.T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + tp.X.ToString(ci) + ";" + tp.Y.ToString(ci) + ";" + tp.V.ToString("0.##", ci));
                    //}
                    //lock (optLocker)
                    //{
                    //    File.WriteAllLines(path2save + "\\opt\\" + TrackID + "." + driveIndex.ToString() + ".csv", optimizedTrack);
                    //}


                    
                    optimizedTrack = new List<string>();
                    List<TrackPoint> optTrack;
                    optTrack = Processors.GDouglasPeucker(t, 10);
                    if (optTrack.Count > 1)
                    {

                        foreach (TrackPoint tp in optTrack)
                        {
                            optimizedTrack.Add(TrackID + "." + driveIndex.ToString() + ";" + tp.T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + tp.X.ToString(ci) + ";" + tp.Y.ToString(ci) + ";" + tp.V.ToString("0.##", ci));
                        }
                        lock (optLocker)
                        {
                            File.WriteAllLines(path2save + "\\opt\\" + TrackID + "." + driveIndex.ToString() + ".csv", optimizedTrack);
                        }
                    }

                }catch(System.Exception ex)
                {
                    logger.Error(ex, "AnalizeTrack for " + TrackID + "." + driveIndex.ToString());
                }
               

            }



        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblCnt.Text = "В очереди: " + ACount.ToString() + "\r\n Сделано: " + FinishedCount.ToString();
            label1.Text = "Обработано поездок: " + Total.ToString();
            if (ACount == 0)
            {
                cmdCSV.Enabled = true;
                grpParam.Enabled = true;
                timer1.Enabled = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = (ACount != 0);
        }

        private void CmsShowMap_Click(object sender, EventArgs e)
        {
            frmMap f = new frmMap();
            f.Show();
        }


        private string GetDirection(TrackPoint s, TrackPoint e)
        {
            string d;
            if (s.X < e.X)
            {
                d = "G";
            }
            else
            {
                if (s.X > e.X)
                {
                    d = "R";
                }
                else
                {
                    if (s.Y < e.Y)
                    {
                        d = "G";
                    }
                    else
                    {
                        d = "R";
                    }
                }
            }
            return d;

        }


        private void Save2DB(Object a)
        {

            string FileName;
            FileName = ((AState)a).FileName;
            DateTime prevOut = DateTime.Now;
            System.Data.SqlClient.SqlConnection cn;

            List<TrackPoint> rawTrack = new List<TrackPoint>();
            string TrackID = "";
            string ObjectId = "";
            string Direction = "";
            Double  V = 0.0;
            CultureInfo ci = new CultureInfo("en-US");

            try
            {

                var trackData = File.ReadLines(FileName);
                DateTime d = DateTime.MinValue;
                cn = new SqlConnection(txtCN.Text);
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    
                    foreach (string s in trackData)
                    {

                        string[] cols = s.Split(';');
                        if (cols.Length >= 5)
                        {
                            ObjectId = cols[0];
                            TrackID = cols[1];
                            Direction = cols[2];
                            V = Double.Parse(cols[4], ci);

                            //if (cols[3].Length == 23)
                            //{
                            //    try
                            //    {
                            //        d = DateTime.ParseExact(cols[1], "yyyy-MM-dd HH:mm:ss.fff", ci);
                            //    }
                            //    catch
                            //    {
                            //    }
                            //}
                            //else
                            //{
                            //    try
                            //    {
                            //        d = DateTime.ParseExact(cols[1], "yyyy-MM-dd HH:mm:ss", ci);
                            //    }
                            //    catch
                            //    {
                            //    }
                            //}

                            string qry = @"INSERT INTO Track2OBJ([OBJECT_ID] ,[TRACK],[GPSTIME],[DIRECTION] ,[V])VALUES(" +
                            ObjectId +",'" + TrackID +"',convert(datetime,'" + cols[3] +"',121),'" + Direction +"'," + V.ToString("0.00",ci) +")";
                            cmd.CommandText = qry;
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }catch(System.Exception ex)
                            {
                                logger.Error(ex, "Save2DB for "  +FileName);
                            }
                            

                        }

                    }
                }
                cn.Close();
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "Save2DB for " + FileName);
            }
            ACount--;
            FinishedCount++;
        }
    
        private void LinkFile(Object a)
        {

            string FileName;
            FileName = ((AState)a).FileName;
            DateTime prevOut = DateTime.Now;
            DataTable dt;
            System.Data.SqlClient.SqlConnection cn;

            List<TrackPoint> rawTrack = new List<TrackPoint>();
            string TrackID = "";
            Double N = 0.0, E = 0.0, V = 0.0;
            CultureInfo ci = new CultureInfo("en-US");

            try
            {

                var trackData = File.ReadLines(FileName);
                DateTime d = DateTime.MinValue;
                //bool isHead;
                //isHead = true;
                foreach (string s in trackData)
                {
                    //if (!isHead)
                    {
                        string[] cols = s.Split(';');


                        if (cols.Length >= 5)
                        {
                            TrackID = cols[0];
                            N = Double.Parse(cols[2], ci);
                            E = Double.Parse(cols[3], ci);
                            V = Double.Parse(cols[4], ci);

                            if (cols[1].Length == 23)
                            {
                                try
                                {
                                    d = DateTime.ParseExact(cols[1], "yyyy-MM-dd HH:mm:ss.fff", ci);
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                try
                                {
                                    d = DateTime.ParseExact(cols[1], "yyyy-MM-dd HH:mm:ss", ci);
                                }
                                catch
                                {
                                }
                            }

                            TrackPoint tp = new TrackPoint() { X = N, Y = E, V = V, T = d };
                            rawTrack.Add(tp);
                            //Total++;
                        }
                    }
                    //else
                    //{
                    //    isHead = false;
                    //}
                }

                string geom;

                {
                    StringBuilder sb = new StringBuilder();

                    string prevObj = "";
                    string curObj;
                    DateTime tStart;
                    TrackPoint startPoint;
                    Double wLen = 0.0;
                    double spd = 0.0;
                    string Direction = "";
                    if (rawTrack.Count > 0)
                    {

                        cn = new SqlConnection(txtCN.Text);
                        cn.Open();
                        if (cn.State == ConnectionState.Open)
                        {

                            tStart = rawTrack[0].T;
                            startPoint = rawTrack[0];

                            for (int i = 1; i < rawTrack.Count; i++)
                            {
                                geom = "LINESTRING(";

                                geom += rawTrack[i - 1].Y.ToString("0.0000000000", ci) + " " + rawTrack[i - 1].X.ToString("0.0000000000", ci);
                                geom += "," + rawTrack[i].Y.ToString("0.0000000000", ci) + " " + rawTrack[i].X.ToString("0.0000000000", ci);

                                geom += ")";
                                dt = new DataTable();
                                double gLen=0.0;

                                //// считаем длину сегмента
                                //{
                                //    SqlCommand cmd = new SqlCommand();
                                //    cmd.Connection = cn;
                                //    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STIntersects('" + geom + "') = 1";
                                //    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STContains('" + geom + "') = 1";
                                //    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STIntersects('" + geom + "') = 1 and geography::STGeomFromText( BUFFER.STIntersection('" + geom + "').ToString(),4326).STLength() > geography::STGeomFromText('" + geom + "',4326).STLength()/2";
                                //    cmd.CommandText = "select geography::STGeomFromText('" + geom + "', 4326).STLength() L";
                                //    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                                //    try
                                //    {
                                //        sda.Fill(dt);
                                //    }
                                //    catch
                                //    {
                                //        logger.Debug(cmd.CommandText);
                                //    }

                                //    gLen = (double)dt.Rows[0]["L"];
                                //    sda.Dispose();
                                //    cmd.Dispose();
                                //    dt.Dispose();
                                //}

                                // проверяем на попадание линии в буфер
                                {
                                    dt = new DataTable();
                                    //iQryCnt++;
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = cn;
                                    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STIntersects('" + geom + "') = 1";
                                    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STContains('" + geom + "') = 1";
                                    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STIntersects('" + geom + "') = 1 and geography::STGeomFromText( BUFFER.STIntersection('" + geom + "').ToString(),4326).STLength() > geography::STGeomFromText('" + geom + "',4326).STLength()/2";
                                    //cmd.CommandText = @"SELECT top(1) OBJECT_ID, geography::STGeomFromText( BUFFER.STIntersection('" + geom + "').ToString(),4326).STLength() L FROM UDS where BUFFER.STIntersects('" + geom + "') = 1 order by  geography::STGeomFromText( BUFFER.STIntersection('" + geom + "').ToString(),4326).STLength() desc"; // + (gLen/2).ToString(ci); // geography::STGeomFromText('" + geom + "',4326).STLength()/2";
                                    cmd.CommandText = @"SELECT  OBJECT_ID FROM UDS where FID_GRAPH=-1 and BUFFER.STIntersects('" + geom + "') = 1 and BUFFER.STIntersection('" + geom + "').STLength() >=   SEGLENGTH  order by SEGLENGTH desc";
                                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                                    try
                                    {
                                        sda.Fill(dt);
                                    }
                                    catch
                                    {
                                        logger.Debug(cmd.CommandText);
                                    }


                                    sda.Dispose();
                                    cmd.Dispose();
                                }
                                //else
                                //{
                                //    cQryCnt++;
                                //}


                                if (dt.Rows.Count > 0)
                                {
                                    //double intLen = (double)dt.Rows[0]["L"];

                                    //if (intLen > gLen * 3 / 4 && intLen > 20)
                                    // {
                                    okCnt++;
                                    curObj = dt.Rows[0]["OBJECT_ID"].ToString();
                                    if (prevObj != curObj)
                                    {
                                        if (prevObj != "")
                                        {
                                            spd = (wLen / Math.Abs((rawTrack[i - 1].T - tStart).TotalHours));
                                            if (spd > MinV)
                                            {
                                                rCnt++;
                                                Direction = GetDirection(startPoint, rawTrack[i - 1]);
                                                sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[i - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
                                            }
                                            else
                                            {
                                                zCnt++;
                                            }
                                        }

                                        // начало нового сегмента
                                        tStart = rawTrack[i - 1].T;
                                        startPoint = rawTrack[i - 1];
                                        wLen = Math.Abs((rawTrack[i - 1].T - rawTrack[i].T).TotalHours) * rawTrack[i].V;

                                        prevObj = curObj;
                                    }
                                    else
                                    {
                                        // засчитываем сегмент
                                        wLen += Math.Abs((rawTrack[i - 1].T - rawTrack[i].T).TotalHours) * rawTrack[i].V;
                                    }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (prevObj != "")
                                    //        {

                                    //            // уехали на улицу вне сети - записываем предыдущий сегмент
                                    //            spd = (wLen / Math.Abs((rawTrack[i - 1].T - tStart).TotalHours));
                                    //            if (spd > MinV)
                                    //            {
                                    //                rCnt++;
                                    //                Direction = GetDirection(startPoint, rawTrack[i - 1]);
                                    //                sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[i - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
                                    //            }
                                    //            else
                                    //            {
                                    //                zCnt++;
                                    //            }
                                    //        }
                                    //        //tStart = rawTrack[i].T;
                                    //        //wLen = 0;
                                    //        prevObj = "";
                                    //        skipCnt++;
                                    //    }

                                    //}
                                    //else if (dt.Rows.Count > 1)
                                    //{
                                    //    string objList = "";

                                    //    if (prevObj != "")
                                    //    {
                                    //        bool prevFound = false;
                                    //        for (int j = 0; j < dt.Rows.Count; j++)
                                    //        {
                                    //            if (j > 0)
                                    //                objList += ",";
                                    //            objList += dt.Rows[j]["OBJECT_ID"].ToString();

                                    //            if (dt.Rows[j]["OBJECT_ID"].ToString() == prevObj)
                                    //            {
                                    //                // есть вероятность, что все еще едем по той же улице
                                    //                // засчитываем сегмент
                                    //                wLen += Math.Abs((rawTrack[i - 1].T - rawTrack[i].T).TotalHours) * rawTrack[i].V;
                                    //                okCnt++;
                                    //                prevFound = true;
                                    //                break;

                                    //            }
                                    //        }

                                    //        if (!prevFound)
                                    //        {
                                    //            // уехали на другую улицу - записываем

                                    //            spd = (wLen / Math.Abs((rawTrack[i - 1].T - tStart).TotalHours));
                                    //            if (spd > MinV)
                                    //            {
                                    //                rCnt++;
                                    //                Direction = GetDirection(startPoint, rawTrack[i - 1]);
                                    //                sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[i - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
                                    //            }
                                    //            else
                                    //            {
                                    //                zCnt++;
                                    //            }



                                    //            // нет однозначности куда поехали
                                    //            // пробуем найти ближайшее

                                    //            DataTable dt2 = new DataTable();
                                    //            SqlCommand cmd = new SqlCommand();
                                    //            cmd.Connection = cn;
                                    //            cmd.CommandText = @"SELECT top ( 1 ) OBJECT_ID, DATA.STDistance('" + geom +"') FROM UDS where object_id in(" + objList + ")  order by DATA.STDistance('" + geom + "')";
                                    //            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                                    //            sda.Fill(dt2);
                                    //            sda.Dispose();
                                    //            cmd.Dispose();

                                    //            prevObj = dt2.Rows[0]["OBJECT_ID"].ToString();
                                    //            tStart = rawTrack[i].T;
                                    //            wLen = 0;
                                    //            startPoint = rawTrack[i];
                                    //            okCnt++;
                                    //            dt2.Dispose();
                                    //        }
                                    //    }
                                    //    else
                                    //    {

                                    //        skipCnt++;
                                    //    }


                                    //}
                                }
                                else // row.count=0 !!!
                                {





                                    if (prevObj != "")
                                    {

                                        // уехали на улицу вне сети - записываем предыдущий сегмент
                                        spd = (wLen / Math.Abs((rawTrack[i - 1].T - tStart).TotalHours));
                                        if (spd > MinV)
                                        {
                                            rCnt++;
                                            Direction = GetDirection(startPoint, rawTrack[i - 1]);
                                            sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[i - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
                                        }
                                        else
                                        {
                                            zCnt++;
                                        }
                                    }
                                    //tStart = rawTrack[i].T;
                                    //wLen = 0;
                                    prevObj = "";
                                    skipCnt++;
                                }
                                dt.Dispose();
                            }

                            if (prevObj != "")
                            {
                                spd = (wLen / Math.Abs((rawTrack[rawTrack.Count - 1].T - tStart).TotalHours));
                                if (spd > MinV)
                                {
                                    rCnt++;
                                    Direction = GetDirection(startPoint, rawTrack[rawTrack.Count - 1]);
                                    sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[rawTrack.Count - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
                                }
                                else
                                {
                                    zCnt++;
                                }

                            }
                        }
                        cn.Close();
                        if (sb.ToString() != "")
                        {
                            lock (optLocker)
                            {
                                File.WriteAllText(FileName.Replace(@"\opt\", @"\lnk\"), sb.ToString());
                            }
                        }
                    }



                }
            }
            catch(System.Exception ex)
            {
                
                logger.Error(ex,"LinkFile for " + FileName);
            }
            ACount--;
            FinishedCount++;
        }

        private int fCnt = 0;
        private int okCnt = 0;
        private int iQryCnt = 0;
        private int cQryCnt = 0;
        private int skipCnt = 0;
        private int rCnt = 0;
        private int zCnt = 0;
        
        private DateTime startTime;

        private void Button2_Click_1(object sender, EventArgs e)
        {
            if (txtCN.Text == "") return;
            button2.Enabled = false;
            // get parameters
            try
            {
                MinV = Double.Parse(txtMinV.Text);
            }
            catch
            {
                MinV = 3.0;
            }
            FinishedCount = 0;

            DirectoryInfo di = new DirectoryInfo(txtTrackPath.Text);
            if (!di.Exists) return;
            //string outDir = txtTrackPath.Text.Replace(@"\opt", @"\lnk");
            //DirectoryInfo di2 = new DirectoryInfo(outDir);
            //if (!di2.Exists)
            //    Directory.CreateDirectory(outDir);


            timer2.Enabled = true;
            {
  
                startTime = DateTime.Now;

                ThreadPool.SetMinThreads(20, 0);
                ThreadPool.SetMaxThreads(40, 0);

                foreach (FileInfo fi in di.GetFiles("*.csv"))
                {
                    fCnt += 1;
                    AState a = new AState()
                    {
                        FileName = fi.FullName,
                        SavePath = fi.DirectoryName
                    };
                    ACount++;
                    ThreadPool.QueueUserWorkItem(LinkFile, a);
                }


            }
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblCnt.Text = "Поездок:" + fCnt.ToString() +"\r\nВ очереди: " + ACount.ToString() + "\r\nОбработано: " + FinishedCount.ToString();

            lblCnt.Text +="\r\n* Сегменты *\r\nДопущено=" + okCnt.ToString() +
                 "\r\nПропущено=" + skipCnt.ToString() + "\r\nV<Min=" + zCnt.ToString() + "\r\nЗафиксировано=" + rCnt.ToString();

            //  "\r\nContains = " + cQryCnt.ToString() + "\r\nIntersects = " + iQryCnt.ToString() +

            Application.DoEvents();
            
            if (ACount == 0)
            {
            
                timer2.Enabled = false;
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtCN.Text == "") return;
            button1.Enabled = false;
            
            // get parameters
            FinishedCount = 0;

            DirectoryInfo di = new DirectoryInfo(txtLinkPath.Text);
            if (!di.Exists) return;

            timer3.Enabled = true;

            {

                startTime = DateTime.Now;

                ThreadPool.SetMinThreads(12, 0);
                ThreadPool.SetMaxThreads(20, 0);

                foreach (FileInfo fi in di.GetFiles("*.csv"))
                {
                    fCnt += 1;
                    AState a = new AState()
                    {
                        FileName = fi.FullName,
                        SavePath = fi.DirectoryName
                    };
                    ACount++;
                    ThreadPool.QueueUserWorkItem(Save2DB, a);
                }


            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            lblCnt.Text = "Поездок:" + fCnt.ToString() + "\r\nВ очереди: " + ACount.ToString() + "\r\nОбработано: " + FinishedCount.ToString();

            //lblCnt.Text += "\r\n* Сегменты *\r\nДопущено=" + okCnt.ToString() + "\r\nПропущено=" + skipCnt.ToString() + "\r\nВне скоростного режима=" + zCnt.ToString() + "\r\nУчтено=" + rCnt.ToString();
            Application.DoEvents();

            if (ACount == 0)
            {

                timer3.Enabled = false;
                button1.Enabled = true;
            }
        }
    }
}

