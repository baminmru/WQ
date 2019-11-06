using System;
using BruTile;
using BruTile.Predefined;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    public partial class frmOSM : Form
    {
        public frmOSM()
        {
            InitializeComponent();
        }


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


        private static ILayer CreateLayer(string path, VectorStyle style)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
                throw new FileNotFoundException("file not found", path);

            string full = file.FullName;
            string name = Path.GetFileNameWithoutExtension(full);
            ILayer layer = new VectorLayer(name, new ShapeFile(full, true))
            {
                SRID = 4326,
                CoordinateTransformation = Wgs84toGoogleMercator,
                TargetSRID = 3857,
                Style = style,
                SmoothingMode = SmoothingMode.AntiAlias
            };
            return layer;
        }

        private void frmOSM_Load(object sender, EventArgs e)
        {


            
        }

            private void SaveLayer(string name, VectorLayer vl)
            {


                ProjNet.CoordinateSystems.CoordinateSystemFactory csFact = new ProjNet.CoordinateSystems.CoordinateSystemFactory();

                string wkt_gk = @"PROJCS[""KGA"",GEOGCS[""GCS_Pulkovo_1942"",DATUM[""D_Pulkovo_1942"",SPHEROID[""Krasovsky_1940"",6378245.0,298.3]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]],PROJECTION[""Transverse_Mercator""],PARAMETER[""False_Easting"",96065.591],PARAMETER[""False_Northing"",-6552809.659],PARAMETER[""Central_Meridian"",30.0],PARAMETER[""Scale_Factor"",1.0],PARAMETER[""Latitude_Of_Origin"",0.0],UNIT[""Meter"",1.0]]";

                GeoAPI.CoordinateSystems.ICoordinateSystem kga = csFact.CreateFromWkt(wkt_gk);
                GeoAPI.CoordinateSystems.IGeographicCoordinateSystem wgs84 = csFact.CreateGeographicCoordinateSystem(
                        "WGS 84", AngularUnit.Degrees, HorizontalDatum.WGS84, PrimeMeridian.Greenwich,
                        new AxisInfo("north", AxisOrientationEnum.North), new AxisInfo("east", AxisOrientationEnum.East));

                ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();



                var kga2wgs84 = ctFact.CreateFromCoordinateSystems(kga, wgs84);

                SharpMap.Data.Providers.ShapeFile provider = (SharpMap.Data.Providers.ShapeFile)vl.DataSource;

                int fc = provider.GetFeatureCount();
                StringBuilder sb = new StringBuilder();


            CultureInfo ci = new CultureInfo("en-US");
            //Dictionary<int, string> ids = new Dictionary<int, string>();
            for (uint i = 0; i < fc; i++)
             {
                txtOUt.Text = i.ToString() + "(" + fc.ToString() + ")";
                Application.DoEvents();
                
                string s = @"INSERT INTO OSM ([osm_id] , [NAME] ,[oneway], [roadtype] ,[DATA]) VALUES (";

                

                FeatureDataRow fdr = provider.GetFeature(i);
                IGeometry g = fdr.Geometry;
                if(fdr["name"].ToString() != "")
                {
                    //try
                    //{
                    //    ids.Add(Int32.Parse(fdr["osm_id"].ToString()), fdr["name"].ToString());
                    //}
                    //catch { }
                    s = s + fdr["osm_id"].ToString();
                    s = s + ",'" + fdr["name"].ToString() + "'";
                    s = s + "," + fdr["oneway"].ToString() + "";
                    s = s + ",'" + fdr["type"].ToString() + "'";
                }
                else
                {
                    s = s + fdr["osm_id"].ToString();
                    //if (fdr["ref"].ToString() != "")
                    //{
                    //    try
                    //    {
                    //        s = s + ",'" + ids[Int32.Parse(fdr["ref"].ToString())] + "'";
                    //    }
                    //    catch
                    //    {
                    //        s = s + ",''";
                    //    }
                    //}
                    //else
                    //{
                        s = s + ",''";
                    //}
                    
                    s = s + "," + fdr["oneway"].ToString() + "";
                    s = s + ",'" + fdr["type"].ToString() + "'";
                }



                //foreach (DataColumn dc in fdr.Table.Columns)
                //{
                //    System.Diagnostics.Debug.Print(dc.ColumnName);
                //    System.Diagnostics.Debug.Print(fdr[dc.ColumnName].ToString());
                //}



                if (g.Coordinates.Length > 0)
                {
                    string l;
                    //l = "'LINESTRING(";
                    //bool isFirst = true;
                    //foreach (GeoPoint gpt in g.Coordinates)
                    //{
                    //    if (!isFirst)
                    //    {
                    //        l += ",";

                    //    }
                    //    l += gpt.X.ToString("0.0000000000", ci) + " " + gpt.Y.ToString("0.0000000000", ci);

                    //    isFirst = false;
                    //}
                    //l = l + ")'";

                    l = g.ToString();
                    s = s + ",'" + l +"'";
                    s = s + ");";
                }
                else
                {
                    s = s + ",NULL";
                    s = s + ");";
                }
           
                sb.AppendLine(s);
                   
            }

            File.WriteAllText(Application.StartupPath + "/OSM/" + name + ".sql", sb.ToString());


         }

        private void cmdLoad_Click(object sender, EventArgs e)
        {
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


            Envelope geom = GeometryTransform.TransformBox(
                new Envelope(28, 31, 58, 61),
                mathTransform);



            /*


            ProjNet.CoordinateSystems.CoordinateSystemFactory csFact = new ProjNet.CoordinateSystems.CoordinateSystemFactory();

        string wkt_gk = "PROJCS[\"DHDN / 3-degree Gauss zone 3 (deprecated)\",GEOGCS[\"DHDN\",DATUM[\"Deutsches_Hauptdreiecksnetz\",SPHEROID[\"Bessel 1841\",6377397.155,299.1528128,AUTHORITY[\"EPSG\",\"7004\"]],AUTHORITY[\"EPSG\",\"6314\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4314\"]],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",9],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",3500000],PARAMETER[\"false_northing\",0],AUTHORITY[\"EPSG\",\"31463\"],AXIS[\"X\",EAST],AXIS[\"Y\",NORTH]]";

        GeoAPI.CoordinateSystems.ICoordinateSystem gauss_krueger_3 = csFact.CreateFromWkt(wkt_gk);

        GeoAPI.CoordinateSystems.ICoordinateSystem webmercator = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator;

        ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

        layer.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(gauss_krueger_3, webmercator);
        layer.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(webmercator, gauss_krueger_3);

    */


            ProjNet.CoordinateSystems.CoordinateSystemFactory csFact = new ProjNet.CoordinateSystems.CoordinateSystemFactory();

            string wkt_gk = @"PROJCS[""KGA"",GEOGCS[""GCS_Pulkovo_1942"",DATUM[""D_Pulkovo_1942"",SPHEROID[""Krasovsky_1940"",6378245.0,298.3]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]],PROJECTION[""Transverse_Mercator""],PARAMETER[""False_Easting"",96065.591],PARAMETER[""False_Northing"",-6552809.659],PARAMETER[""Central_Meridian"",30.0],PARAMETER[""Scale_Factor"",1.0],PARAMETER[""Latitude_Of_Origin"",0.0],UNIT[""Meter"",1.0]]";

            GeoAPI.CoordinateSystems.ICoordinateSystem kga = csFact.CreateFromWkt(wkt_gk);

            GeoAPI.CoordinateSystems.ICoordinateSystem webmercator = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator;

            GeoAPI.CoordinateSystems.IGeographicCoordinateSystem wgs84 = csFact.CreateGeographicCoordinateSystem(
                     "WGS 84", AngularUnit.Degrees, HorizontalDatum.WGS84, PrimeMeridian.Greenwich,
                     new AxisInfo("north", AxisOrientationEnum.North), new AxisInfo("east", AxisOrientationEnum.East));

            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();


            DirectoryInfo ldi = new DirectoryInfo(Application.StartupPath + "/osm");
            foreach (FileInfo fi in ldi.GetFiles("*.shp"))
            {

                VectorLayer streets = (VectorLayer)CreateLayer(fi.FullName, new VectorStyle { Line = new Pen(Color.Red) });
                streets.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(kga, webmercator);
                streets.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(webmercator, kga);
                SaveLayer(fi.Name, streets);

            }
        }
    }
    
}
