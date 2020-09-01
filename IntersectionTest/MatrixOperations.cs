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
    class MatrixOperations
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
     




        private static void PorcessStream(Stream data, string srcName)
        {
            Dictionary<string, List<TrackPoint>> Reorg = new Dictionary<string, List<TrackPoint>>();
            using (StreamReader stream = new StreamReader(data))
            {
                string TrackID = "";

                Double N = 0.0, E = 0.0, V = 0.0;
                DateTime d = DateTime.MinValue;
                bool OK;


                Total = 0;
                while (stream.Peek() >= 0)
                {
                    string s = stream.ReadLine();
                    string[] cols = s.Split(';');
                    OK = false;
                    if (cols.Length >= 7)
                    {
                        OK = true;
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
                                OK = false;
                                d = DateTime.MinValue;
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
                                OK = false;
                                d = DateTime.MinValue;
                            }
                        }
                    }

                    // optimized tracks
                    if (cols.Length == 5)
                    {
                        TrackID = cols[0];
                        OK = true;

                        d = DateTime.MinValue;

                        if (cols[1].Length == 23)
                        {
                            try
                            {
                                d = DateTime.ParseExact(cols[1], "yyyy-MM-dd HH:mm:ss.fff", ci);
                            }
                            catch
                            {
                                OK = false;
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
                                OK = false;
                            }
                        }

                        if (cols[2].Contains(","))
                            E = Double.Parse(cols[2], ciRus);
                        else
                            E = Double.Parse(cols[2], ci);
                        if (cols[3].Contains(","))
                            N = Double.Parse(cols[3], ciRus);
                        else
                            N = Double.Parse(cols[3], ci);


                        if (cols[4].Contains(","))
                            V = Double.Parse(cols[4], ciRus);
                        else
                            V = Double.Parse(cols[4], ci);



                        //N = N * 180 / Math.PI;
                        //E = E * 180 / Math.PI;


                    }
                    if (OK)
                    {
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
                    optimized = AnalizeTrack(Reorg[s], s, srcName);
                    if (optimized != null)
                        dCnt += optimized.Keys.Count;


                
                    {

                        logger.Info("Linking " + s);

                        foreach (string sOpt in optimized.Keys)
                        {

                            List<MatrixItem> lnk;
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
            FileInfo fi = new FileInfo(zName);
            string xName = fi.Name.Replace(fi.Extension, "");

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
                            PorcessStream(entry.Open(), xName);
                        }
                    }
                }

                // process gzip file
                if (zName.ToLower().EndsWith(".gzip"))
                {
                    logger.Info("Processing as GZIP file");
                    using (var zipStream = new GZipStream(file, CompressionMode.Decompress))
                    {
                        PorcessStream(zipStream, xName);
                    }
                }

                // process unpacked csv file
                if (zName.ToLower().EndsWith(".csv"))
                {
                    logger.Info("Processing as CSV file");
                    PorcessStream(file, xName);
                }
            }
        }

      

        public static bool ProcessFolder(string Wildcard)
        {
      
            DirectoryInfo di = new DirectoryInfo(Folder);
            List<FileInfo> files = new List<FileInfo>();
            //if (WithSubfolder)
            //{
            //    foreach (DirectoryInfo sdi in di.GetDirectories())
            //    {
            //        files.AddRange(sdi.GetFiles(Wildcard));
            //    }
            //}
            //else
            {
                files.AddRange(di.GetFiles(Wildcard));
            }



            fCnt = files.Count;

            if (fCnt > 0)
            {
                StartTime = DateTime.MinValue;
                fCur = 0;
              
                {
                    ThreadPool.SetMinThreads(15, 0);
                    ThreadPool.SetMaxThreads(16, 0);
                    foreach (FileInfo fi in files)
                    {
                        ThreadPool.QueueUserWorkItem(ProcessFile, (object)fi.FullName);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }

        }

        private static DateTime DFrom = new DateTime(2019, 1, 1);
        private static DateTime DTo = new DateTime(2021, 1, 1);

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


                if (tt.T < DTo && tt.T >= DFrom && !double.IsNaN(tt.V))
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
                                {
                                    l.Add(cur);
                                }
                                    

                                // дальше будет уже следующая
                                cur = new List<TrackPoint>();
                            }
                        }



                    }
                }

            }
            if (cur.Count > 0)
                l.Add(cur);
            return l;
        }


        private static Dictionary<string, List<TrackPoint>> AnalizeTrack(List<TrackPoint> Track, string TrackID, string srcName)
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
                    if (t.Count > 1)
                    {
                        opt.Add(TrackID + "." + driveIndex.ToString(), t);
                    }
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex, "AnalizeTrack for " + TrackID + "." + driveIndex.ToString());
                }
            }
            return opt;
        }

        private static List<MatrixItem> LinkFile(string sCN, string LinkTtrackName, List<TrackPoint> rawTrack)
        {

            DataTable dt;
            System.Data.SqlClient.SqlConnection cn;
            List<MatrixItem> matrix = new List<MatrixItem>();

            try
            {
                string geom;
                string startObj = "";
                string endObj = "";
                DateTime tStart;
                DateTime tEnd;
                TrackPoint startPoint;
                TrackPoint endPoint;

                if (rawTrack.Count > 0)
                {

                    cn = new SqlConnection(sCN);
                    cn.Open();
                    if (cn.State == ConnectionState.Open)
                    {


                        startPoint = rawTrack[0];
                        endPoint = rawTrack[rawTrack.Count - 1];
                        tStart = startPoint.T;
                        tEnd = endPoint.T;

                        // check Start point of track
                        {
                            geom = "POINT(";
                            geom += startPoint.X.ToString("0.0000000000", ci) + " " + startPoint.Y.ToString("0.0000000000", ci);
                            geom += ")";

                            dt = new DataTable();

                            // проверяем на попадание стартовой точки в район
                            {
                                dt = new DataTable();
                                //iQryCnt++;
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = cn;

                                cmd.CommandText = @"SELECT  CODE FROM REGIONG where DATA.STContains(N'" + geom + "') = 1 ";
                                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                                try
                                {
                                    sda.Fill(dt);
                                }
                                catch (System.Exception ex)
                                {
                                    logger.Debug(cmd.CommandText + " " + ex.Message);
                                }


                                sda.Dispose();
                                cmd.Dispose();
                            }


                            if (dt.Rows.Count > 0)
                            {
                                startObj = ""+ dt.Rows[0]["CODE"].ToString();
                                dt.Dispose();
                            }



                            // check end point of track
                            {
                                geom = "POINT(";
                                geom += endPoint.X.ToString("0.0000000000", ci) + " " + endPoint.Y.ToString("0.0000000000", ci);
                                geom += ")";



                                dt = new DataTable();

                                // проверяем на попадание стартовой точки в район
                                {
                                    dt = new DataTable();
                                    //iQryCnt++;
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = cn;

                                    cmd.CommandText = @"SELECT  CODE FROM REGIONG where DATA.STContains(N'" + geom + "') = 1 ";
                                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                                    try
                                    {
                                        sda.Fill(dt);
                                    }
                                    catch (System.Exception ex)
                                    {
                                        logger.Debug(cmd.CommandText + " " + ex.Message);
                                    }


                                    sda.Dispose();
                                    cmd.Dispose();
                                }


                                if (dt.Rows.Count > 0)
                                {
                                    endObj = "" + dt.Rows[0]["CODE"].ToString();
                                    dt.Dispose();
                                }


                            }
                            cn.Close();

                        }

                        if(startObj != "" && endObj != "" && startObj != endObj && (tEnd-tStart).TotalMinutes > 5)
                        {

                            String sData = "LINESTRING(";
                            Boolean isFirst = true;
                            foreach (TrackPoint tp in rawTrack)
                            {
                                if (!isFirst)
                                    sData += ",";
                                sData += tp.X.ToString("0.0000000000", ci) + " " + tp.Y.ToString("0.0000000000", ci) +"  NULL " + tp.V.ToString("0.000", ci);
                                isFirst = false;
                            }

                            sData += ")";

                            matrix.Add(new MatrixItem 
                            {
                                TrackID= LinkTtrackName,
                                StartTime = tStart,
                                EndTime = tEnd,
                                FromRegion =startObj,
                                ToRegion = endObj,
                                Data = sData

                            });
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {

                logger.Error(ex, "Matrix for " + LinkTtrackName);

            }
            finally
            {
            }
            return matrix;

        }


        private static void Save2DB(string sCN, List<MatrixItem> records)
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

                    foreach (MatrixItem l in records)
                    {
                        string qry = @"INSERT INTO Matrix2([TRACK],[FROMTIME],[TOTIME],[FROMREGION],[TOREGION],DATA)VALUES(" +
                        "'" + l.TrackID + "',convert(datetime,'" + l.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "',121),convert(datetime,'" + l.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "',121),'" + 
                        l.FromRegion + "','" + l.ToRegion + "','" +l.Data +"')";
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
        }


    }
}
