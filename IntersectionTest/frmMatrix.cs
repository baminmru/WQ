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
    public partial class frmMatrix : Form
    {
        public frmMatrix()
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

        private void frmMatrix_Load(object sender, EventArgs e)
        {


            this.mapBox1.Map.BackgroundLayer.Clear();

            var gss = new NtsGeometryServices();
            var css = new SharpMap.CoordinateSystems.CoordinateSystemServices(
                new CoordinateSystemFactory(),
                new CoordinateTransformationFactory(),
                SharpMap.Converters.WellKnownText.SpatialReference.GetAllReferenceSystems());

            GeoAPI.GeometryServiceProvider.Instance = (GeoAPI.IGeometryServices)gss;
            SharpMap.Session.Instance
                .SetGeometryServices((GeoAPI.IGeometryServices)gss)
                .SetCoordinateSystemServices(css)
                .SetCoordinateSystemRepository(css);

            GeometryFactory gf = new GeometryFactory(new PrecisionModel(), 3857);

            IMathTransform mathTransform = Wgs84toGoogleMercator.MathTransform;


            SharpMap.Layers.TileAsyncLayer osmLayer = new SharpMap.Layers.TileAsyncLayer(BruTile.Predefined.KnownTileSources.Create(BruTile.Predefined.KnownTileSource.OpenStreetMap),
        "TileLayer - OSM");
            
            this.mapBox1.Map.BackgroundLayer.Add(osmLayer);


            //Envelope geom = GeometryTransform.TransformBox(
            //    new Envelope(28, 31, 58, 61),
            //    mathTransform);



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


            DirectoryInfo ldi = new DirectoryInfo(Application.StartupPath + "/REG");
            foreach (FileInfo fi in ldi.GetFiles("*.shp"))
            {

                
                VectorLayer streets = (VectorLayer)CreateLayer(fi.FullName, new VectorStyle { Line = new Pen(Color.Red), Fill = new SolidBrush(Color.Green) });
                streets.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(kga, webmercator);
                streets.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(webmercator, kga);
                this.mapBox1.Map.Layers.Add(streets);
                SaveLayer(fi.Name, streets);
                
            }

            GeoPoint gpt = Wgs84toGoogleMercator.MathTransform.Transform(new GeoPoint(29, 59));
            GeoPoint gpt2 = Wgs84toGoogleMercator.MathTransform.Transform(new GeoPoint(31, 61));
            this.mapBox1.Map.ZoomToBox(new GeoAPI.Geometries.Envelope(gpt, gpt2));


            this.mapBox1.Refresh();
            this.mapBox1.ActiveTool= MapBox.Tools.Pan;
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



            // var kga2wgs84 = ctFact.CreateFromCoordinateSystems(kga, wgs84);

            SharpMap.Data.Providers.ShapeFile provider = (SharpMap.Data.Providers.ShapeFile)vl.DataSource;

            int fc = provider.GetFeatureCount();
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();



            for (uint i = 0; i < fc; i++)
            {
                string s = @"INSERT INTO REGION ([OBJECT_ID] ,[NAME] ,[NO] ,[CODE] ,[LAYER] ,[BBOX] ,[DATA]) VALUES (";

                string gs = @"INSERT INTO REGIONG ([OBJECT_ID], [NAME]  ,[CODE], [TNAS]  ,[DATA]) VALUES (";

                string u = @"update UDS set ";

                FeatureDataRow fdr = provider.GetFeature(i);


                //foreach (DataColumn dc in fdr.Table.Columns)
                //{
                //    System.Diagnostics.Debug.Print(dc.ColumnName);

                //    System.Diagnostics.Debug.Print(fdr[dc.ColumnName].ToString());
                //}
                s = s +  fdr["NO"].ToString();
                s = s + ",'" + fdr["NAME"].ToString() + "'";
                s = s + ",'"  + fdr["CODE"].ToString() + "'";
                s = s + ",'" + fdr["TNAS_2010"].ToString() + "'";
                s = s + ",'" + name + "'";

                //gs = gs + fdr["TNAS_2010"].ToString();

                gs = gs +  fdr["NO"].ToString();
                gs = gs + ",'" + fdr["NAME"].ToString() + "'";
                gs = gs + ",'" + fdr["CODE"].ToString() + "'";
                gs = gs + ",'" + fdr["TNAS_2010"].ToString() + "'";


                u = u + "NAME='" + fdr["NAME"].ToString() + "'";
                u = u + ",CODE='" + fdr["CODE"].ToString() + "'";
                u = u + ",TNAS='" + fdr["TNAS_2010"].ToString() + "'";
                u = u + " where OBJECT_ID=" + fdr["NO"].ToString() + ";";




                IGeometry g = fdr.Geometry;
                System.Diagnostics.Debug.Print(g.GeometryType + ":");

                CultureInfo ci = new CultureInfo("en-US");
                string b;
                b = "'LINESTRING(";
                string gb;
                gb = "geography::STGeomFromText('LINESTRING(";

                bool isFirst = true;
                GeoPoint pFirst = null;
                GeoPoint gpt = null;
                foreach (GeoPoint gp in g.Boundary.Coordinates)
                {
                    gpt = gp; //  kga2wgs84.MathTransform.Transform(gp);
                    if (isFirst)
                        pFirst = gpt;
                    if (!isFirst)
                    {
                        b += ",";
                        gb += ",";
                    }
                    gb += "(" + gpt.Y.ToString("0.00000000000000000", ci) + " " + gpt.X.ToString("0.00000000000000000", ci) + ")";
                    b += "(" + gpt.X.ToString("0.000000000000000", ci) + " " + gpt.Y.ToString("0.000000000000000", ci) + ")";
                    isFirst = false;

                }
                //if(pFirst != null && gpt != null && 
                //    ( pFirst.Y.ToString("0.00000000000000000", ci) != gpt.Y.ToString("0.00000000000000000", ci) ||
                //     pFirst.X.ToString("0.00000000000000000", ci) != gpt.X.ToString("0.00000000000000000", ci)
                //     )
                //    )
                //{
                //    gb += "(" + pFirst.Y.ToString("0.00000000000000000", ci) + " " + pFirst.X.ToString("0.00000000000000000", ci) + ")";
                //    b += "(" + pFirst.X.ToString("0.000000000000000", ci) + " " + pFirst.Y.ToString("0.000000000000000", ci) + ")";
                //}
                b += ")'";
                gb += "),4326)'";
                //gs = gs + "," + gb;
                s = s + "," + b;

                if (g.NumGeometries > 1)
                {
                    if (g.OgcGeometryType == GeoAPI.Geometries.OgcGeometryType.MultiPolygon)
                    {
                        string l;
                        string gl;
                        l = "'MULTIPOLYGON(";
                        gl = "geography::STGeomFromText('MULTIPOLYGON(";
                        bool isFirstGeometry = true;
                        MultiPolygon mg = (MultiPolygon)g;
                        foreach (IGeometry g2 in mg.Geometries)
                        {

                            if (!isFirstGeometry)
                            {
                                l += " \r\n,";
                                gl += " \r\n,";
                            }
                            isFirstGeometry = false;

                            isFirst = true;
                            pFirst = null;
                            gpt = null;
                            l += " ((";
                            gl += " ((";
                            foreach (GeoPoint gp in g2.Coordinates)
                            {

                                gpt = gp; // kga2wgs84.MathTransform.Transform(gp);
                                if (isFirst)
                                    pFirst = gpt;
                                if (!isFirst)
                                {
                                    l += ",";
                                    gl += ",";
                                }
                                l += gpt.X.ToString("0.000000000000000", ci) + " " + gpt.Y.ToString("0.000000000000000", ci);
                                gl += gpt.X.ToString("0.000000000000000", ci) + " " + gpt.Y.ToString("0.000000000000000", ci);
                                isFirst = false;

                            }

                            if (pFirst != null && gpt != null &&
                               (pFirst.Y.ToString("0.00000000000000000", ci) != gpt.Y.ToString("0.00000000000000000", ci) ||
                                pFirst.X.ToString("0.00000000000000000", ci) != gpt.X.ToString("0.00000000000000000", ci)
                                )
                               )
                            {
                                System.Diagnostics.Debug.Print(g.ToString());
                                l += "," + pFirst.Y.ToString("0.00000000000000000", ci) + " " + pFirst.X.ToString("0.00000000000000000", ci) ;
                                gl += "," + pFirst.X.ToString("0.000000000000000", ci) + " " + pFirst.Y.ToString("0.000000000000000", ci) ;
                            }
                            l += ")) ";
                            gl += ")) ";


                        }


                        l += ")'";
                        gl += ")',4326)";
                        s = s + "," + l;
                        gs = gs + "," + gl;

                        s = s + ");";
                        gs = gs + ");";
                        sb.AppendLine(s);
                        sb2.AppendLine(u);
                        sb3.AppendLine(gs);
                    }
                }
                else
                {
                    if (g.OgcGeometryType == GeoAPI.Geometries.OgcGeometryType.Polygon)
                    {
                        string l;
                        string gl;
                        l = "'POLYGON((";
                        gl = "geography::STGeomFromText('POLYGON((";
                        isFirst = true;
                        pFirst = null;
                        gpt = null;

                        foreach (GeoPoint gp in g.Coordinates)
                        {

                            gpt = gp; // kga2wgs84.MathTransform.Transform(gp);
                            if (isFirst)
                                pFirst = gpt;
                            if (!isFirst)
                            {
                                l += ",";
                                gl += ",";
                            }
                            l += gpt.X.ToString("0.000000000000000", ci) + " " + gpt.Y.ToString("0.000000000000000", ci);
                            gl += gpt.X.ToString("0.000000000000000", ci) + " " + gpt.Y.ToString("0.000000000000000", ci);
                            isFirst = false;

                        }

                        if (pFirst != null && gpt != null &&
                           (pFirst.Y.ToString("0.00000000000000000", ci) != gpt.Y.ToString("0.00000000000000000", ci) ||
                            pFirst.X.ToString("0.00000000000000000", ci) != gpt.X.ToString("0.00000000000000000", ci)
                            )
                           )
                        {
                            System.Diagnostics.Debug.Print(g.ToString());
                            l += " ," + pFirst.Y.ToString("0.00000000000000000", ci) + " " + pFirst.X.ToString("0.00000000000000000", ci) ;
                            gl += ", " + pFirst.X.ToString("0.000000000000000", ci) + " " + pFirst.Y.ToString("0.000000000000000", ci) ;
                        }
                        l += "))'";
                        gl += "))',4326)";
                        s = s + "," + l;
                        gs = gs + "," + gl;

                        s = s + ");";
                        gs = gs + ");";
                        sb.AppendLine(s);
                        sb2.AppendLine(u);
                        sb3.AppendLine(gs);
                    }
                    else
                    {
                        System.Diagnostics.Debug.Print(g.OgcGeometryType.ToString());
                        System.Diagnostics.Debug.Print(g.ToString());
                    }
                }
            }

            File.WriteAllText(Application.StartupPath + "/REG/"+name+".sql", sb.ToString());
            File.WriteAllText(Application.StartupPath + "/REG/" + name + "_g.sql", sb3.ToString());
            File.WriteAllText(Application.StartupPath + "/REG/" + name + "_update.sql", sb2.ToString());
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

        private void frmMatrix_Resize(object sender, EventArgs e)
        {
            this.mapBox1.Refresh();
        }

        
        private void Button1_Click(object sender, EventArgs e)
        {
            //System.Collections.Generic.IEnumerable<string> jsons;
            //var gjr = new NetTopologySuite.IO.GeoJsonReader();

            //var geom = jsons.Select(json => gjr.Read<GeoAPI.Geometries.IGeometry>(json)).ToList();

            //var fp = new SharpMap.Data.Providers.GeometryFeatureProvider(geom);
            //var l = new SharpMap.Layers.VectorLayer("geojson", fp);
            //l.CoordinateTransformation = new
            //    ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory().CreateFromCoordinateSystems(
            //        ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84,
            //        ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);


            //Random rndGen = new Random();
            //Collection<IGeometry> geometry = new Collection<IGeometry>();

            //VectorLayer layer = new VectorLayer(String.Empty);
            //var gf = new GeometryFactory();
            //switch (rndGen.Next(3))
            //{
            //    case 0:
            //        {
            //            GeneratePoints(gf, geometry, rndGen);
            //            KeyValuePair<string, Bitmap> symbolEntry = getSymbolEntry(rndGen.Next(_symbolTable.Count));
            //            layer.Style.Symbol = symbolEntry.Value;
            //            layer.LayerName = symbolEntry.Key;
            //        }
            //        break;
            //    case 1:
            //        {
            //            GenerateLines(gf, geometry, rndGen);
            //            KeyValuePair<string, Color> colorEntry = getColorEntry(rndGen.Next(_colorTable.Count));
            //            layer.Style.Line = new Pen(colorEntry.Value);
            //            layer.LayerName = String.Format("{0} lines", colorEntry.Key);
            //        }
            //        break;
            //    //case 2:
            //    //    {
            //    //        GeneratePolygons(gf, geometry, rndGen);
            //    //        KeyValuePair<string, Color> colorEntry = getColorEntry(rndGen.Next(_colorTable.Count));
            //    //        layer.Style.Fill = new SolidBrush(colorEntry.Value);
            //    //        layer.LayerName = String.Format("{0} squares", colorEntry.Key);
            //    //    }
            //    //    break;
            //    default:
            //        throw new NotSupportedException();
            //}

            //var provider = new GeometryProvider(geometry);
            //layer.DataSource = provider;
            //this.mapBox1.Map.Layers.Add(layer);
        }
    }
}
