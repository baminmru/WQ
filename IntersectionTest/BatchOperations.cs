using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Data.SqlClient;
using NLog;
using System.Threading;
using System.Globalization;

namespace IntersectionTest
{
    public static class BatchOperations
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static Double MinV = 2.0;
        public static Double Distance = 10.0;
        public static Int64 ACount = 0;
        public static Int64 FinishedCount = 0;
        public static Int64 Total = 0;
        public static double StopTime = 10.0;
        public static DateTime StartTime;


        public static Int64 lCnt = 0;
        public static Int64 lCur = 0;
        public static Int64 tCnt = 0;
        public static Int64 tCur = 0;
        public static Int64 fCnt = 0;
        public static Int64 fCur = 0;
        public static Int64 fDone = 0;
        public static Int64 dCnt = 0;
        public static Int64 dCur = 0;




        private static object optLocker = new object();
        private static CultureInfo ci = new CultureInfo("en-US");
        private static CultureInfo ciRus = new CultureInfo("ru-RU");

        public static string CN;
        public static string Folder;



        private static void PorcessStream(Stream data)
        {
            Dictionary<string, List<TrackPoint>> Reorg = new Dictionary<string, List<TrackPoint>>();
            using (StreamReader stream = new StreamReader(data))
            {
                string TrackID = "";

                Double N = 0.0, E = 0.0, V = 0.0;
                DateTime d;

               

                Total = 0;
                while (stream.Peek() >= 0)
                {
                    string s = stream.ReadLine();
                    string[] cols = s.Split(';');
                    if (cols.Length >= 8)
                    {
                        TrackID = cols[0];
                        if (cols[4].Contains(","))
                            N = Double.Parse(cols[4], ciRus);
                        else
                            N = Double.Parse(cols[4], ci);
                        if (cols[5].Contains(","))
                            E = Double.Parse(cols[5], ciRus);
                        else
                            E = Double.Parse(cols[5], ci);

                        if (cols[6].Contains(","))
                            V = Double.Parse(cols[6], ciRus);
                        else
                            V = Double.Parse(cols[6], ci);
                        N = N * 180 / Math.PI;
                        E = E * 180 / Math.PI;

                        d = DateTime.MinValue;

                        if (cols[3].Length == 23)
                        {
                            try
                            {
                                d = DateTime.ParseExact(cols[3], "yyyy-MM-dd HH:mm:ss.fff", ci);
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            try
                            {
                                d = DateTime.ParseExact(cols[3], "yyyy-MM-dd HH:mm:ss", ci);
                            }
                            catch
                            {
                            }
                        }

                        TrackPoint tp = new TrackPoint() { X = E, Y = N, V = V, T = d };

                        if (Reorg.ContainsKey(TrackID))
                        {
                            Reorg[TrackID].Add(tp);
                        }
                        else
                        {
                            List<TrackPoint> newList = new List<TrackPoint>();
                            newList.Add(tp);
                            Reorg.Add(TrackID, newList);
                        }
                    }
                    Total++;
                    lCnt++;

                }


                Total = 0;
                Dictionary<string, List<TrackPoint>> optimized;
                if (StartTime == DateTime.MinValue)
                {
                    StartTime = DateTime.Now;
                }

                tCnt += Reorg.Keys.Count;

                foreach (string s in Reorg.Keys)
                {
                    tCur++;
                    logger.Info("Optimizing " + s);
                    optimized = AnalizeTrack(Reorg[s], s);
                    dCnt += optimized.Keys.Count;

                    logger.Info("Linking " + s);

                    foreach (string sOpt in optimized.Keys)
                    {

                        List<LinkObject> lnk;
                        lnk = LinkFile(CN, sOpt, optimized[sOpt]);
                        if (lnk.Count > 0)
                        {
                            logger.Info("Saving: " + sOpt);

                            Save2DB(CN, lnk);
                            lnk.Clear();
                        }
                        dCur++;
                        optimized[sOpt].Clear();
                    }

                    optimized.Clear();
                    Reorg[s].Clear();

                }
                Reorg.Clear();

            }
            fDone++;
        }

        private static void ProcessFile(object zipName)
        {

            string zName = (string)zipName;

            using (var file = File.OpenRead(zName))
            {
                fCur++;

                logger.Info("Reading tracks from " + zName);

                // process zip file
                if (zName.ToLower().EndsWith(".zip"))
                {
                    logger.Info("Processing as ZIP file");
                    using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
                    {
                        foreach (var entry in zip.Entries)
                        {
                            PorcessStream(entry.Open());
                        }
                    }
                }

                // process gzip file
                if (zName.ToLower().EndsWith(".gzip"))
                {
                    logger.Info("Processing as GZIP file");
                    using (var zipStream = new GZipStream(file, CompressionMode.Decompress))
                    {
                        PorcessStream(zipStream);
                    }
                }

                // process unpacked csv file
                if (zName.ToLower().EndsWith(".csv"))
                {
                    logger.Info("Processing as CSV file");
                    PorcessStream(file);
                }
            }
        }


        public static bool ProcessFolder(string Wildcard)
        {
            DirectoryInfo di = new DirectoryInfo(Folder);
            FileInfo[] files = di.GetFiles(Wildcard);
            fCnt = files.Length;

            if (fCnt > 0)
            {

                ThreadPool.SetMinThreads(15, 0);
                ThreadPool.SetMaxThreads(16, 0);
                StartTime = DateTime.MinValue;

                fCur = 0;
                foreach (FileInfo fi in files)
                {
                    ThreadPool.QueueUserWorkItem(ProcessFile, (object)fi.FullName);
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }


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


        private static Dictionary<string, List<TrackPoint>> AnalizeTrack(List<TrackPoint> Track, string TrackID)
        {

            Dictionary<string, List<TrackPoint>> opt = new Dictionary<string, List<TrackPoint>>();

            // сортируем список по времени
            Track.Sort();

          
            // разбиваем на поездки
            List<List<TrackPoint>> l = SptitTrack(Track);
            Int64 driveIndex = 0;
            foreach (List<TrackPoint> t in l)
            {
                Total++;
                driveIndex++;
                try
                {
                    List<TrackPoint> optTrack;
                    List<string> optimizedTrack = new List<string>();
                  
                    optTrack = Processors.GDouglasPeucker(t, 8);
                    if (optTrack.Count > 1)
                    {
                            opt.Add(TrackID + "." + driveIndex.ToString(),optTrack);
                    }

                }
                catch (System.Exception ex)
                {
                    logger.Error(ex, "AnalizeTrack for " + TrackID + "." + driveIndex.ToString());
                }


            }
            return opt;


        }


        private static string GetDirection(TrackPoint s, TrackPoint e)
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

       
        private  static List<LinkObject> LinkFile(string sCN, string LinkTtrackName, List<TrackPoint> rawTrack)
        {

            DataTable dt;
            System.Data.SqlClient.SqlConnection cn;
            Double N = 0.0, E = 0.0, V = 0.0;
            List<LinkObject> links = new List<LinkObject>();

            try
            {
                string geom;
                {
                  

                    string prevObj = "";
                    string curObj;
                    DateTime tStart;
                    TrackPoint startPoint;
                    Double wLen = 0.0;
                    double spd = 0.0;
                    string Direction = "";
                    if (rawTrack.Count > 0)
                    {

                        cn = new SqlConnection(sCN);
                        cn.Open();
                        if (cn.State == ConnectionState.Open)
                        {

                            tStart = rawTrack[0].T;
                            startPoint = rawTrack[0];

                            for (int i = 1; i < rawTrack.Count; i++)
                            {
                                geom = "LINESTRING(";

                                geom += rawTrack[i - 1].X.ToString("0.0000000000", ci) + " " + rawTrack[i - 1].Y.ToString("0.0000000000", ci);
                                geom += "," + rawTrack[i].X.ToString("0.0000000000", ci) + " " + rawTrack[i].Y.ToString("0.0000000000", ci);

                                geom += ")";
                                dt = new DataTable();
                                double gLen = 0.0;

                                //// считаем длину сегмента
                                //{
                                //    SqlCommand cmd = new SqlCommand();
                                //    cmd.Connection = cn;
                                //    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STIntersects('" + geom + "') = 1";
                                //    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STContains('" + geom + "') = 1";
                                //    //cmd.CommandText = @"SELECT OBJECT_ID FROM UDS where BUFFER.STIntersects('" + geom + "') = 1 and geography::STGeomFromText( BUFFER.STIntersection('" + geom + "').ToString(),4326).STLength() > geography::STGeomFromText('" + geom + "',4326).STLength()/2";
                                //    cmd.CommandText = "select geometry::STGeomFromText('" + geom + ",0).STLength() L";
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
                                    cmd.CommandText = @"SELECT  OBJECT_ID FROM UDS where BUFFER.STIntersects('" + geom + "') = 1 and BUFFER.STIntersection('" + geom + "').STLength() >=   SEGLENGTH  order by seglength desc"; 
                                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                                    try
                                    {
                                        sda.Fill(dt);
                                    }
                                    catch(System.Exception ex)
                                    {
                                        logger.Debug(cmd.CommandText  + " " +ex.Message);
                                    }


                                    sda.Dispose();
                                    cmd.Dispose();
                                }
                                

                                if (dt.Rows.Count > 0)
                                {
                                    //double intLen = (double)dt.Rows[0]["L"];

                                    //if ( intLen > 8.98315e-5 * 3)
                                    {
                                       
                                        curObj = dt.Rows[0]["OBJECT_ID"].ToString();
                                        if (prevObj != curObj)
                                        {
                                            if (prevObj != "")
                                            {
                                                spd = (wLen / Math.Abs((rawTrack[i - 1].T - tStart).TotalHours));
                                                if (spd > MinV)
                                                {
                                                 
                                                    Direction = GetDirection(startPoint, rawTrack[i - 1]);
                                                    links.Add(new LinkObject()
                                                    {
                                                        ObjectID = prevObj,
                                                        TrackID = LinkTtrackName,
                                                        Direction = Direction,
                                                        T = rawTrack[i - 1].T,
                                                        V = spd,
                                                        SECONDS = (rawTrack[i - 1].T-startPoint.T).TotalSeconds
                                                    });
                                                    //sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[i - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
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
                                    }
                                    //else
                                    //{
                                    //    if (prevObj != "")
                                    //    {

                                    //        // уехали на улицу вне сети - записываем предыдущий сегмент
                                    //        spd = (wLen / Math.Abs((rawTrack[i - 1].T - tStart).TotalHours));
                                    //        if (spd > MinV)
                                    //        {
                                               
                                    //            Direction = GetDirection(startPoint, rawTrack[i - 1]);
                                    //            links.Add(new LinkObject()
                                    //            {
                                    //                ObjectID = prevObj,
                                    //                TrackID = LinkTtrackName,
                                    //                Direction = Direction,
                                    //                T = rawTrack[i - 1].T,
                                    //                V = spd,
                                    //                SECONDS = (rawTrack[i - 1].T - startPoint.T).TotalSeconds
                                    //            });
                                    //            //sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[i - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
                                    //        }
                                           
                                    //    }
                                    //    //tStart = rawTrack[i].T;
                                    //    //wLen = 0;
                                    //    prevObj = "";
                                      
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
                                           
                                            Direction = GetDirection(startPoint, rawTrack[i - 1]);
                                            links.Add(new LinkObject()
                                            {
                                                ObjectID = prevObj,
                                                TrackID = LinkTtrackName,
                                                Direction = Direction,
                                                T = rawTrack[i - 1].T,
                                                V = spd,
                                                SECONDS = (rawTrack[i - 1].T - startPoint.T).TotalSeconds
                                            });
                                           // sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[i - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
                                        }
                                       
                                    }
                                    //tStart = rawTrack[i].T;
                                    //wLen = 0;
                                    prevObj = "";
                                  
                                }
                                dt.Dispose();
                            }

                            if (prevObj != "")
                            {
                                spd = (wLen / Math.Abs((rawTrack[rawTrack.Count - 1].T - tStart).TotalHours));
                                if (spd > MinV)
                                {
                                   
                                    Direction = GetDirection(startPoint, rawTrack[rawTrack.Count - 1]);
                                    links.Add(new LinkObject()
                                    {
                                        ObjectID = prevObj,
                                        TrackID = LinkTtrackName,
                                        Direction = Direction,
                                        T = rawTrack[rawTrack.Count - 1].T,
                                        V = spd,
                                        SECONDS = (rawTrack[rawTrack.Count - 1].T - startPoint.T).TotalSeconds
                                    });
                                    //sb.AppendLine(prevObj + ";" + TrackID + ";" + Direction + ";" + rawTrack[rawTrack.Count - 1].T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + spd.ToString("#0.00", ci));
                                }
                              

                            }
                        }
                        cn.Close();
                      
                    }



                }
            }
            catch (System.Exception ex)
            {

                logger.Error(ex, "LinkFile for " + LinkTtrackName);
                
            }
            finally
            {
                //ACount--;
               // FinishedCount++;
            }
            return links;

        }


        private static void Save2DB(string sCN,List<LinkObject> records)
        {

            
            System.Data.SqlClient.SqlConnection cn;

            

            try
            {

                cn = new SqlConnection(sCN);
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;

                    foreach (LinkObject l in records)
                    {
                            string qry = @"INSERT INTO Track2OBJ([OBJECT_ID] ,[TRACK],[GPSTIME],[DIRECTION] ,[V],SECONDS)VALUES(" +
                            l.ObjectID + ",'" + l.TrackID + "',convert(datetime,'" + l.T.ToString("yyyy-MM-dd HH:mm:ss") + "',121),'" + l.Direction + "'," + l.V.ToString("0.00", ci) +"," + l.SECONDS.ToString("0.0000", ci) + ")";
                            cmd.CommandText = qry;
                            try
                            {
                            ACount++;
                            cmd.ExecuteNonQuery();
                           
                            }
                            catch (System.Exception ex)
                            {
                                logger.Error(ex, "Save2DB for " + l.TrackID);
                            }
                    }
                }
                cn.Close();
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "Save2DB");
            }
            //ACount--;
            //FinishedCount++;
        }

    }
 
}
