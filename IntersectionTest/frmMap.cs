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
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using GeoAPI.CoordinateSystems.Transformations;
using SharpMap.Forms;
using SharpMap.Layers;
using SharpMap.Styles;


using ProjNet.CoordinateSystems.Transformations;
using GeoAPI.CoordinateSystems;
using ProjNet.CoordinateSystems;


using SharpMap.Data;
using SharpMap.Data.Providers;
using GeoPoint = GeoAPI.Geometries.Coordinate;

namespace IntersectionTest
{
    public partial class frmMap : Form
    {
        public frmMap()
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

        private void FrmMap_Load(object sender, EventArgs e)
        {
            SharpMap.Layers.TileAsyncLayer osmLayer = new SharpMap.Layers.TileAsyncLayer(BruTile.Predefined.KnownTileSources.Create(BruTile.Predefined.KnownTileSource.OpenStreetMap),
               "TileLayer - OSM");
            this.mapBox1.Map.BackgroundLayer.Clear();
            this.mapBox1.Map.BackgroundLayer.Add(osmLayer);

            GeometryFactory gf = new GeometryFactory(new PrecisionModel(), 3857);

            IMathTransform mathTransform = Wgs84toGoogleMercator.MathTransform;
            Envelope geom = GeometryTransform.TransformBox(
                new Envelope(28, 31, 58, 61),
                mathTransform);

            ////Adds a pushpin layer
            //VectorLayer pushPinLayer = new VectorLayer("PushPins");
            //List<IGeometry> geos = new List<IGeometry>();
            //geos.Add(gf.CreatePoint(geom.Centre));
            //GeometryProvider geoProvider = new GeometryProvider(geos);
            //pushPinLayer.DataSource = geoProvider;
            ////this.mapBox1.Map.Layers.Add(pushPinLayer);

           

            this.mapBox1.Map.ZoomToBox(geom);
            
            this.mapBox1.Refresh();
            this.mapBox1.ActiveTool= MapBox.Tools.Pan;








        }

        private void FrmMap_Resize(object sender, EventArgs e)
        {
            this.mapBox1.Refresh();
        }

        private static void GeneratePoints(IGeometryFactory factory, ICollection<IGeometry> geometry, Random rndGen)
        {
            var numPoints = rndGen.Next(10, 100);
            for (var pointIndex = 0; pointIndex < numPoints; pointIndex++)
            {
                var point = new GeoPoint(rndGen.NextDouble() * 1000, rndGen.NextDouble() * 1000);
                geometry.Add(factory.CreatePoint(point));
            }
        }

        private static void GenerateLines(IGeometryFactory factory, ICollection<IGeometry> geometry, Random rndGen)
        {
            var numLines = rndGen.Next(10, 100);
            for (var lineIndex = 0; lineIndex < numLines; lineIndex++)
            {
                var numVerticies = rndGen.Next(4, 15);
                var vertices = new GeoPoint[numVerticies];

                var lastPoint = new GeoPoint(rndGen.NextDouble() * 1000, rndGen.NextDouble() * 1000);
                vertices[0] = lastPoint;

                for (var vertexIndex = 1; vertexIndex < numVerticies; vertexIndex++)
                {
                    var nextPoint = new GeoPoint(lastPoint.X + rndGen.Next(-50, 50),
                                                 lastPoint.Y + rndGen.Next(-50, 50));
                    vertices[vertexIndex] = nextPoint;

                    lastPoint = nextPoint;
                }
                geometry.Add(factory.CreateLineString(vertices));
            }
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


            Random rndGen = new Random();
            Collection<IGeometry> geometry = new Collection<IGeometry>();

            VectorLayer layer = new VectorLayer(String.Empty);
            var gf = new GeometryFactory();
            switch (rndGen.Next(3))
            {
                case 0:
                    {
                        GeneratePoints(gf, geometry, rndGen);
                        KeyValuePair<string, Bitmap> symbolEntry = getSymbolEntry(rndGen.Next(_symbolTable.Count));
                        layer.Style.Symbol = symbolEntry.Value;
                        layer.LayerName = symbolEntry.Key;
                    }
                    break;
                case 1:
                    {
                        GenerateLines(gf, geometry, rndGen);
                        KeyValuePair<string, Color> colorEntry = getColorEntry(rndGen.Next(_colorTable.Count));
                        layer.Style.Line = new Pen(colorEntry.Value);
                        layer.LayerName = String.Format("{0} lines", colorEntry.Key);
                    }
                    break;
                //case 2:
                //    {
                //        GeneratePolygons(gf, geometry, rndGen);
                //        KeyValuePair<string, Color> colorEntry = getColorEntry(rndGen.Next(_colorTable.Count));
                //        layer.Style.Fill = new SolidBrush(colorEntry.Value);
                //        layer.LayerName = String.Format("{0} squares", colorEntry.Key);
                //    }
                //    break;
                default:
                    throw new NotSupportedException();
            }

            var provider = new GeometryProvider(geometry);
            layer.DataSource = provider;
            this.mapBox1.Map.Layers.Add(layer);
        }
    }
}
