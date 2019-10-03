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



namespace IntersectionTest
{
    public partial class Form1 : Form
    {

        private static Double MinV = 1.0;
        private static Double Distance = 1.0;
        private static int ACount = 0;
        private static int FinishedCount = 0;
        private static int Total = 0;


        private static object optLocker = new object();
        private static object jsonLocker = new object();
        private static string header=@"""TrackID"";""RecordTime"";""N"";""E"";""Velocity"";""Marker""";
        public Form1()
        {
            InitializeComponent();
        }
        private List<Intersections> iList = null;

        private void Button1_Click(object sender, EventArgs e1)
        {
            iList = new List<Intersections>();
            Random rnd = new Random();
            int ICount = 0;
            int Total = 0;
            DateTime s = DateTime.Now;
            DateTime e;
            TimeSpan sp;
            for (int i = 0; i < 25000; i++)
            {
                double x;
                double y;
                double w;
                double h;

                x = rnd.NextDouble() * 1000;
                y = rnd.NextDouble() * 1000;
                w = 1 + rnd.NextDouble() * 100;
                h = 1 + rnd.NextDouble() * 100;

                RectangleF r1 = new RectangleF((float)x, (float)y, (float)w, (float)h);

                for (int j = 0; j < 10000; j++)
                {

                  
                    x = rnd.NextDouble() * 1000;
                    y = rnd.NextDouble() * 1000;
                    w = 1 + rnd.NextDouble() * 100;
                    h = 1 + rnd.NextDouble() * 100;
                    RectangleF r2 = new RectangleF((float)x, (float)y, (float)w, (float)h);
                    if (r1.IntersectsWith(r2))
                    {
                        ICount++;
                        iList.Add(new Intersections() { Track = i,Street=j });
                    }
                    Total++;
                }
               
                label1.Text = Total.ToString("0,0") + " : " + ICount.ToString("0,0") ;
                Application.DoEvents();
            }
            e = DateTime.Now;
            sp = new TimeSpan(e.Ticks - s.Ticks);
            label1.Text = Total.ToString("0,0") + " : " + ICount.ToString("0,0") +"  -> "+ sp.ToString();

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (opf.ShowDialog() == DialogResult.OK)
            {
                cmdCSV.Enabled = false;
                grpParam.Enabled = false;
                FinishedCount = 0;
                ACount = 0;
                FileInfo fi = new FileInfo(opf.FileName);

                String path2save = fi.DirectoryName;
                path2save += "\\Tracks";
                if (!Directory.Exists(path2save))
                {
                    try { Directory.CreateDirectory(path2save); }
                    catch { }
                }

                if (!Directory.Exists(path2save +"\\opt"))
                {
                    try { Directory.CreateDirectory(path2save+"\\opt"); }
                    catch { }
                }

                if (!Directory.Exists(path2save + "\\GeoJSON"))
                {
                    try { Directory.CreateDirectory(path2save + "\\GeoJSON"); }
                    catch { }
                }

                var it = File.ReadLines(fi.FullName);

         
                CultureInfo ci = new CultureInfo("en-US");
               
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



                string TrackID = "";
                
                List<string> oneTrack = new List<string>();
               
             
                string sNE;
                Double N = 0.0, E = 0.0, V = 0.0;
                List<TrackPoint> rawTrack = new List<TrackPoint>();
                ThreadPool.SetMinThreads(10, 0);
                ThreadPool.SetMaxThreads(50, 0);
                timer1.Enabled = true;

                foreach (string s in it)
                {
                    

                    string[] cols = s.Split(';');
                    
                  
                    if (cols.Length >= 8)
                    {

                        N = Double.Parse(cols[4], ci);
                        E = Double.Parse(cols[5], ci);
                        V = Double.Parse(cols[6], ci);
                        N = N * 180 / Math.PI;
                        E = E * 180 / Math.PI;

                        sNE = cols[0] + ";" + cols[3] + ";" + N.ToString(ci) + ";" + E.ToString(ci) + ";" + cols[6] + ";" + cols[7];

                        
                        if ( TrackID != cols[0])
                        {

                            if (oneTrack.Count > 1)
                            {
                                File.WriteAllLines(path2save + "\\" + TrackID + ".csv", oneTrack);
                                {
                                    List<TrackPoint> Track = new List<TrackPoint>();
                                    rawTrack.ForEach((item) =>
                                    {
                                        Track.Add(new TrackPoint(item));
                                    });

                                    AState a = new AState() {
                                    Track=Track,
                                    TrackID=TrackID,
                                    SavePath=path2save};
                                    ACount++;
                                    ThreadPool.QueueUserWorkItem(AnalizeTrackQ,a);

                                    
                                }
                            }

                            TrackID = cols[0];

                            oneTrack.Clear();
                            oneTrack.Add(header);
                            rawTrack.Clear();
                        }

                        if (TrackID == cols[0])
                        {
                            //if (V > MinV)
                            {

                                TrackPoint tp = new TrackPoint() { X = N, Y = E, V = V, T = DateTime.ParseExact(cols[3], "yyyy-MM-dd HH:mm:ss", ci), M = cols[7] };
                                oneTrack.Add(sNE);
                                rawTrack.Add(tp);
                               
                            }

                        }
                        Application.DoEvents();
                        Total++;
                      
                    }
                  

                }

                // write last Track
                if (oneTrack.Count > 1) {
                    File.WriteAllLines(path2save + "\\" + TrackID + ".csv", oneTrack);
                    {
                        List<TrackPoint> Track = new List<TrackPoint>();
                        rawTrack.ForEach((item) =>
                        {
                            Track.Add(new TrackPoint(item));
                        });
                        AState a = new AState()
                        {
                            Track = Track,
                            TrackID = TrackID,
                            SavePath = path2save
                        };
                        ACount++;
                        ThreadPool.QueueUserWorkItem(AnalizeTrackQ, a);
                    }

                }

            }
        }

        private static int StopTime = 1;


        // режем на поездки
        private static List<List<TrackPoint>> SptitTrack(List<TrackPoint> Track)
        {
            List<List<TrackPoint>> l = new List<List<TrackPoint>>();
           

            if(Track.Count<2)
            {
                l.Add(Track);
                return l;
            }

            List<TrackPoint> cur = new List<TrackPoint>();
            TrackPoint LastMovePoint = Track[0];
            foreach(TrackPoint tt in Track)
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
                    if(d > Distance)
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
            if(cur.Count>0)
                l.Add(cur);
            return l;
        }


        private static void AnalizeTrackQ(object state)
        {
            AState a = (AState)state;
            AnalizeTrack(a.Track, a.TrackID, a.SavePath);
            a.Track.Clear();
            ACount--;
            FinishedCount++;
        }

        private static void AnalizeTrack(List<TrackPoint> Track, string TrackID, string path2save)
        {
            CultureInfo ci = new CultureInfo("en-US");

            Track.Reverse();
            // разбиваем на поездки


            List<List<TrackPoint>> l = SptitTrack(Track);
            int driveIndex = 0;
            foreach (List<TrackPoint> t in l)
            {
                driveIndex++;
                List<TrackPoint> optTrack;
                List<string> optimizedTrack = new List<string>();
                optimizedTrack.Add(header);

                optTrack = Processors.GDouglasPeucker(t, 10);
                if (optTrack.Count > 1)
                {

                    foreach (TrackPoint tp in optTrack)
                    {
                        optimizedTrack.Add(TrackID + ";" + tp.T.ToString("yyyy-MM-dd HH:mm:ss") + ";" + tp.X.ToString(ci) + ";" + tp.Y.ToString(ci) + ";" + tp.V.ToString("0.##", ci) + ";" + tp.M);
                    }
                    lock (optLocker)
                    {
                        File.WriteAllLines(path2save + "\\opt\\" + TrackID + "." + driveIndex.ToString() + ".csv", optimizedTrack);
                    }
                }


                // write GeoJSON using GeoJson.Net
                FeatureCollection fc = new FeatureCollection();
                fc.CRS = GeoJSON.Net.CoordinateReferenceSystem.DefaultCRS.Instance;
                foreach (TrackPoint tp in optTrack)
                {

                    var geom = new GeoJSON.Net.Geometry.Point(new Position(tp.X, tp.Y));
                    var props = new Dictionary<string, object>{
                                    { "ele", tp.V },
                                    { "time", tp.T }
                                };
                    var feature = new Feature(geom, props);
                    fc.Features.Add(feature);
                }

                string j = JsonConvert.SerializeObject(fc);
                File.WriteAllText(path2save + "\\GeoJSON\\" + TrackID + "." + driveIndex.ToString() + ".json", j);



                // write GeoJSON using BAMCIS.GeoJson
                //List<Feature> lf = new List<Feature>();
                //foreach (TrackPoint tp in optTrack)
                //{

                //    var geom = new BAMCIS.GeoJSON.Point(new Position(tp.X, tp.Y));
                //    var props = new Dictionary<string, object>{
                //                    { "ele", tp.V },
                //                    { "time", tp.T }
                //                };
                //    var feature = new Feature(geom, props);
                //    lf.Add(feature);
                //}

                //FeatureCollection fc = new FeatureCollection(lf);
                //lock (jsonLocker)
                //{
                //    File.WriteAllText(path2save + "\\GeoJSON\\" + TrackID + "." + driveIndex.ToString() + ".json", fc.ToJson());
                //}

            }
            

            
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblCnt.Text = "В очереди: " +ACount.ToString() + "\r\n Сделано: " + FinishedCount.ToString();
            label1.Text = "ОБработано строк: " + Total.ToString();
            if (ACount == 0)
            {
                cmdCSV.Enabled = true;
                grpParam.Enabled = true;
                timer1.Enabled = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (ACount != 0);
        }

        private void CmsShowMap_Click(object sender, EventArgs e)
        {
            frmMap f = new frmMap();
            f.Show();
        }
    }
}

