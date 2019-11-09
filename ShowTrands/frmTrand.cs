using System;
using BruTile;
using BruTile.Predefined;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoAPI.Geometries;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using GeoAPI.CoordinateSystems.Transformations;
using SharpMap.Forms;
using SharpMap.Layers;
using SharpMap.Styles;
using System.IO;
using System.Drawing.Drawing2D;
using System.Globalization;


using ProjNet.CoordinateSystems.Transformations;
using GeoAPI.CoordinateSystems;
using ProjNet.CoordinateSystems;


using SharpMap.Data;
using SharpMap.Data.Providers;
using GeoPoint = GeoAPI.Geometries.Coordinate;

namespace IntersectionTest
{
    public partial class frmTrand : Form
    {
        public frmTrand()
        {
            InitializeComponent();
        }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        private static ICoordinateTransformation _wgs84ToGoogle;


        /// <summary>
        /// Wgs84 to Google Mercator Coordinate Transformation
        /// </summary>
        public static ICoordinateTransformation Wgs84toGoogleMercator
        {
            get
            {

                if (_wgs84ToGoogle == null)
                {
                    CoordinateSystemFactory csFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
                    CoordinateTransformationFactory ctFac = new CoordinateTransformationFactory();

                    IGeographicCoordinateSystem wgs84 = csFac.CreateGeographicCoordinateSystem(
                      "WGS 84", AngularUnit.Degrees, HorizontalDatum.WGS84, PrimeMeridian.Greenwich,
                      new AxisInfo("north", AxisOrientationEnum.North), new AxisInfo("east", AxisOrientationEnum.East));

                    List<ProjectionParameter> parameters = new List<ProjectionParameter>();
                    parameters.Add(new ProjectionParameter("semi_major", 6378137.0));
                    parameters.Add(new ProjectionParameter("semi_minor", 6378137.0));
                    parameters.Add(new ProjectionParameter("latitude_of_origin", 0.0));
                    parameters.Add(new ProjectionParameter("central_meridian", 0.0));
                    parameters.Add(new ProjectionParameter("scale_factor", 1.0));
                    parameters.Add(new ProjectionParameter("false_easting", 0.0));
                    parameters.Add(new ProjectionParameter("false_northing", 0.0));
                    IProjection projection = csFac.CreateProjection("Google Mercator", "mercator_1sp", parameters);

                    IProjectedCoordinateSystem epsg900913 = csFac.CreateProjectedCoordinateSystem(
                      "Google Mercator", wgs84, projection, LinearUnit.Metre, new AxisInfo("East", AxisOrientationEnum.East),
                      new AxisInfo("North", AxisOrientationEnum.North));

                    ((CoordinateSystem)epsg900913).DefaultEnvelope = new[] { -20037508.342789, -20037508.342789, 20037508.342789, 20037508.342789 };

                    _wgs84ToGoogle = ctFac.CreateFromCoordinateSystems(wgs84, epsg900913);
                }

                return _wgs84ToGoogle;

            }
        }

        private void frmTrand_Load(object sender, EventArgs e)
        {
            this.mapBox1.Map.BackgroundLayer.Clear();

            var gss = new NtsGeometryServices();
            var css = new SharpMap.CoordinateSystems.CoordinateSystemServices(
                new CoordinateSystemFactory(),
                new CoordinateTransformationFactory(),
                SharpMap.Converters.WellKnownText.SpatialReference.GetAllReferenceSystems());

            GeoAPI.GeometryServiceProvider.Instance = gss;
            SharpMap.Session.Instance
                .SetGeometryServices(gss)
                .SetCoordinateSystemServices(css)
                .SetCoordinateSystemRepository(css);

            GeometryFactory gf = new GeometryFactory(new PrecisionModel(), 3857);

            IMathTransform mathTransform = Wgs84toGoogleMercator.MathTransform;


            SharpMap.Layers.TileAsyncLayer osmLayer = new SharpMap.Layers.TileAsyncLayer(BruTile.Predefined.KnownTileSources.Create(BruTile.Predefined.KnownTileSource.OpenStreetMap),
        "TileLayer - OSM");

            this.mapBox1.Map.BackgroundLayer.Add(osmLayer);



            this.mapBox1.ActiveTool = MapBox.Tools.Pan;

            GeoPoint gpt = Wgs84toGoogleMercator.MathTransform.Transform(new GeoPoint(29, 59));
            GeoPoint gpt2 = Wgs84toGoogleMercator.MathTransform.Transform(new GeoPoint(31, 61));
            this.mapBox1.Map.ZoomToBox( new Envelope(gpt,gpt2));
            
            this.mapBox1.Refresh();







        }


        private SharpMap.Styles.VectorStyle GetStreetStyle(SharpMap.Data.FeatureDataRow row)
        {
            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();
            int sz;
            if (optBuf.Checked)
                sz = 1;
            else
                sz = 4;

            switch (row["Q"].ToString().ToUpper())
            {


                case "A":
                    style.Fill =Brushes.Green;
                    style.Line = new Pen(style.Fill, sz);
                    return style;
                case "B": 
                    style.Fill = Brushes.YellowGreen;
                    style.Line = new Pen(style.Fill, sz);
                    return style;
                case "C": 
                    style.Fill = Brushes.Yellow;
                    style.Line = new Pen(style.Fill, sz);
                    return style;
                case "D": 
                    style.Fill = Brushes.Orange;
                    style.Line = new Pen(style.Fill, sz);
                    return style;
                case "E": 
                    style.Fill = Brushes.OrangeRed;
                    style.Line = new Pen(style.Fill, sz);
                    return style;
                case "F": 
                    style.Fill = Brushes.Red;
                    style.Line = new Pen(style.Fill, sz);
                    return style;

                default:
                    style.Fill = Brushes.Cyan;
                    style.Line = new Pen(style.Fill, sz);
                    return style;
            }

           
            
        }


        FeatureDataSet fds;

        DataTable dtIntervals;
        private System.Data.SqlClient.SqlConnection cn;
        private SharpMap.Layers.VectorLayer layRoads = null;

        private void cmdStart_Click(object sender, EventArgs e)
        {

            try
            {

                cmdStart.Enabled = false;
                txtCN.Enabled = false;



                ProjNet.CoordinateSystems.CoordinateSystemFactory csFact = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
                GeoAPI.CoordinateSystems.ICoordinateSystem webmercator = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator;

                GeoAPI.CoordinateSystems.IGeographicCoordinateSystem wgs84 = csFact.CreateGeographicCoordinateSystem(
                         "WGS 84", AngularUnit.Degrees, HorizontalDatum.WGS84, PrimeMeridian.Greenwich,
                         new AxisInfo("north", AxisOrientationEnum.North), new AxisInfo("east", AxisOrientationEnum.East));

                ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();


                SharpMap.Rendering.Thematics.CustomTheme myTheme = new SharpMap.Rendering.Thematics.CustomTheme(GetStreetStyle);

                //Create layer

                string ConnectionString = txtCN.Text;

                SqlServer2008 ds;

                if (optBuf.Checked)
                    ds = new SqlServer2008(ConnectionString, "v_USDG", "DATA", "OBJECT_ID", SqlServerSpatialObjectType.Geography, 4326, SqlServer2008ExtentsMode.QueryIndividualFeatures);
                else
                    ds = new SqlServer2008(ConnectionString, "v_USDG2", "DATA", "OBJECT_ID", SqlServerSpatialObjectType.Geography, 4326, SqlServer2008ExtentsMode.QueryIndividualFeatures);

                ds.Open();

                if (layRoads != null) this.mapBox1.Map.Layers.Remove(layRoads);

                fds = new FeatureDataSet();
                Envelope env = new Envelope(new GeoPoint(29, 59), new GeoPoint(31, 61));
                ds.ExecuteIntersectionQuery(env, fds);
                var geometryFeatureProvider = new SharpMap.Data.Providers.GeometryFeatureProvider(fds.Tables[0]);
                foreach (FeatureDataRow fdr in fds.Tables[0].Rows)
                {
                    fdr["Q"] = '?';

                }
                layRoads = new SharpMap.Layers.VectorLayer("Roads", geometryFeatureProvider);
                layRoads.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(wgs84, webmercator);
                layRoads.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(webmercator, wgs84);

                ds.Close();
                ds.Dispose();




                layRoads.Theme = myTheme;

                this.mapBox1.Map.Layers.Add(layRoads);


                SharpMap.Layers.LabelLayer layLabel = new SharpMap.Layers.LabelLayer("Road labels");

                layLabel.DataSource = layRoads.DataSource;
                layLabel.Enabled = true;





                layLabel.Enabled = true;
                layLabel.LabelColumn = "object_id";
                layLabel.MaxVisible = 2;
                layLabel.MaxVisible = 190;
                layLabel.MinVisible = 130;
                layLabel.MultipartGeometryBehaviour = SharpMap.Layers.LabelLayer.MultipartGeometryBehaviourEnum.Largest;
                layLabel.LabelFilter = SharpMap.Rendering.LabelCollisionDetection.ThoroughCollisionDetection;
                layLabel.PriorityColumn = "object_id";
                layLabel.Style.ForeColor = Color.Beige;
                layLabel.Style.Font = new Font(FontFamily.GenericSerif, 12);
                layLabel.Style.BackColor = new System.Drawing.SolidBrush(Color.FromArgb(128, 255, 0, 0));
                layLabel.Style.HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Center;
                layLabel.Style.CollisionDetection = true;


                //Add label layer to map
                this.mapBox1.Map.Layers.Add(layLabel);



                cn = new SqlConnection(ConnectionString);
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {
                    {
                        dtIntervals = new DataTable();

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cn;
                        string qry = @" select distinct YD + ' ' + convert(varchar(1),dayinterval) INTERVAL from trands order by YD + ' ' + convert(varchar(1),dayinterval) ";
                        cmd.CommandText = qry;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);

                        try
                        {
                            sda.Fill(dtIntervals);
                            cmbInterval.DataSource = dtIntervals;
                            cmbInterval.DisplayMember = "INTERVAL";
                            cmbInterval.ValueMember = "INTERVAL";
                            cmbInterval.Enabled = true;
                        }
                        catch (System.Exception ex)
                        {
                            logger.Debug(cmd.CommandText + " " + ex.Message);
                        }


                        sda.Dispose();
                        cmd.Dispose();
                    }

                    if (dtIntervals.Rows.Count > 0)
                    {
                        CurInterval = 0;
                        //timer1.Enabled = true;
                        //do
                        //{
                        //    DataTable dtTrand = new DataTable();
                        //    SqlCommand cmd = new SqlCommand();
                        //    cmd.Connection = cn;
                        //    string qry = @"select * from trands where YD ='" + dtIntervals.Rows[CurInterval]["YD"].ToString() + "' and DAYINTERVAL=" + dtIntervals.Rows[CurInterval]["DAYINTERVAL"].ToString() + " order by object_id";
                        //    cmd.CommandText = qry;
                        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                        //    try
                        //    {
                        //        sda.Fill(dtTrand);
                        //    }
                        //    catch (System.Exception ex)
                        //    {
                        //        logger.Debug(cmd.CommandText + " " + ex.Message);
                        //    }

                        //    sda.Dispose();
                        //    cmd.Dispose();
                        //    Application.DoEvents();
                        //    foreach (FeatureDataRow fdr in fds.Tables[0].Rows)
                        //    {
                        //        fdr["Q"] = '?';
                        //        foreach (DataRow dr in dtTrand.Rows)
                        //        {
                        //            if (fdr["object_id"].ToString() == dr["object_id"].ToString())
                        //            {
                        //                fdr["Q"] = dr["PPM"].ToString();
                        //                break;
                        //            }
                        //        }
                        //    }

                        //    lblInfo.Text = dtIntervals.Rows[CurInterval]["YD"].ToString() + " Интервал:" + dtIntervals.Rows[CurInterval]["DAYINTERVAL"].ToString();
                        //    CurInterval++;
                        //    this.mapBox1.Refresh();
                        //    Application.DoEvents();

                        //} while (CurInterval != dtIntervals.Rows.Count);

                        //cmdStart.Enabled = true;
                        //txtCN.Enabled = true;
                        //this.mapBox1.Map.Layers.Remove(layRoads);

                    }
                }

            }catch(System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private int CurInterval = 0;
        private bool InTimer=false;

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (InTimer)
            {
                Application.DoEvents();
                return;
            }
            InTimer = true;

            //DataTable dtTrand = new DataTable();
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = cn;
            //string qry = @"select * from trands where YD ='" + dtIntervals.Rows[CurInterval]["YD"].ToString() + "' and DAYINTERVAL=" + dtIntervals.Rows[CurInterval]["DAYINTERVAL"].ToString() + " order by object_id";
            //cmd.CommandText = qry;
            //SqlDataAdapter sda = new SqlDataAdapter(cmd);

            //try
            //{
            //    sda.Fill(dtTrand);
            //}
            //catch (System.Exception ex)
            //{
            //    logger.Debug(cmd.CommandText + " " + ex.Message);
            //}

            //sda.Dispose();
            //cmd.Dispose();

            //foreach (FeatureDataRow fdr in fds.Tables[0].Rows)
            //{
            //    fdr["Q"] = '?';
            //    foreach (DataRow dr in dtTrand.Rows)
            //    {
            //        if (fdr["object_id"].ToString() == dr["object_id"].ToString())
            //        {
            //            fdr["Q"] = dr["PPM"].ToString();
            //            break;
            //        }
            //    }
            //}

            //this.Text = dtIntervals.Rows[CurInterval]["YD"].ToString() + " Интервал:" + dtIntervals.Rows[CurInterval]["DAYINTERVAL"].ToString();
            ////lblInfo.Text = dtIntervals.Rows[CurInterval]["YD"].ToString() + " Интервал:" + dtIntervals.Rows[CurInterval]["DAYINTERVAL"].ToString();
            //CurInterval++;
            //if (CurInterval == dtIntervals.Rows.Count)
            //{
            //    CurInterval = 0;
            //    timer1.Enabled = false;
            //    cmdStart.Enabled = true;
            //    txtCN.Enabled = true;
                
            //}


            //this.mapBox1.Refresh();
            //Application.DoEvents();
            InTimer = false;
        }

        private void frmTrand_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void cmbInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable dtTrand = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            string qry = @"select * from trands where YD + ' ' + convert(varchar(1),dayinterval) ='" + cmbInterval.Text + "' order by object_id";
            cmd.CommandText = qry;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                sda.Fill(dtTrand);
            }
            catch (System.Exception ex)
            {
                logger.Debug(cmd.CommandText + " " + ex.Message);
            }

            sda.Dispose();
            cmd.Dispose();

            foreach (FeatureDataRow fdr in fds.Tables[0].Rows)
            {
                fdr["Q"] = '?';
                foreach (DataRow dr in dtTrand.Rows)
                {
                    if (fdr["object_id"].ToString() == dr["object_id"].ToString())
                    {
                        fdr["Q"] = dr["PPM"].ToString();
                        break;
                    }
                }
            }

            this.Text = cmbInterval.Text;
            //lblInfo.Text = dtIntervals.Rows[CurInterval]["YD"].ToString() + " Интервал:" + dtIntervals.Rows[CurInterval]["DAYINTERVAL"].ToString();
            //CurInterval++;
            //if (CurInterval == dtIntervals.Rows.Count)
            //{
            //    CurInterval = 0;
            //    timer1.Enabled = false;
            //    cmdStart.Enabled = true;
            //    txtCN.Enabled = true;

            //}


            this.mapBox1.Refresh();
            Application.DoEvents();
            this.Cursor = Cursors.Default;
        }
    }
}
