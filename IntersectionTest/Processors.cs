using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IntersectionTest
{
    public static class Processors
    {


        public static  bool IsInPolygon(PointF point, List<PointF> poly)
        {
            var coef = poly.Skip(1).Select((p, i) =>
                                            (point.Y - poly[i].Y) * (p.X - poly[i].X)
                                          - (point.X - poly[i].X) * (p.Y - poly[i].Y));

            var coefNum = coef.GetEnumerator();

            if (coef.Any(p => p == 0))
                return true;

            float lastCoef = coefNum.Current,
                count = coef.Count();

            coefNum.MoveNext();

            do
            {
                if (coefNum.Current - lastCoef < 0)
                    return false;

                lastCoef = coefNum.Current;
            }
            while (coefNum.MoveNext());

            return true;
        }

        public static bool IsPointInPolygon2(PointF point, List<PointF> polygon)
        {
            var intersects = new List<float>();
            var a = polygon.Last();
            foreach (var b in polygon)
            {
                if (b.X == point.X && b.Y == point.Y)
                {
                    return true;
                }

                if (b.X == a.X && point.X == a.X && point.X >= Math.Min(a.Y, b.Y) && point.Y <= Math.Max(a.Y, b.Y))
                {
                    return true;
                }

                if (b.Y == a.Y && point.Y == a.Y && point.X >= Math.Min(a.X, b.X) && point.X <= Math.Max(a.X, b.X))
                {
                    return true;
                }

                if ((b.Y < point.Y && a.Y >= point.Y) || (a.Y < point.Y && b.Y >= point.Y))
                {
                    var px = (float)(b.X + 1.0 * (point.Y - b.Y) / (a.Y - b.Y) * (a.X - b.X));
                    intersects.Add(px);
                }

                a = b;
            }

            intersects.Sort();
            return intersects.IndexOf(point.X) % 2 == 0 || intersects.Count(x => x < point.X) % 2 == 1;
        }

        // Calculate the distance between
        // point pt and the segment p1 --> p2.
        public static double FindDistanceToSegment(
            PointF pt, PointF p1, PointF p2, out PointF closest)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) /
                (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }


        // http://www.cyberforum.ru/windows-forms/thread976818.html
        public static PointF[] Line2Poly( PointF StartPointF,PointF EndPointF, int kink)
        {

            double delta = kink * 360.0 / (2.0 * Math.PI * 6378137.0);

            double angle = //угол между прямой и Ох
                (
                    Math.PI / 2 + //поворачиваем прямую - находим угол перпендикуляра
                    (
                        Math.Atan
                            (
                                (EndPointF.Y - StartPointF.Y) /
                                (EndPointF.X - StartPointF.X)
                            )
                    )
                );

            double sin = Math.Sin(angle );
            double cos = Math.Cos(angle );

            float x = (float)(StartPointF.X + delta * cos);
            float y = (float)(StartPointF.Y + delta * sin);

            //конец перпендекуляра
            //проведенного из начала прямой(StartPointF)
            PointF p1 = new PointF(x, y);

            x = (float)(EndPointF.X + delta * cos);
            y = (float)(EndPointF.Y + delta * sin);

            //конец перпендекуляра
            //проведенного из конца прямой(EndPointF)
            PointF p2 = new PointF(x, y);



            double angle2 = //угол между прямой и Ох
                (
                    - Math.PI / 2 + //поворачиваем прямую - находим угол перпендикуляра
                    (
                        Math.Atan
                            (
                                (EndPointF.Y - StartPointF.Y) /
                                (EndPointF.X - StartPointF.X)
                            )
                    )
                );

            double sin2 = Math.Sin(angle2);
            double cos2 = Math.Cos(angle2);

            float x2 = (float)(StartPointF.X + delta * cos2);
            float y2 = (float)(StartPointF.Y + delta * sin2);

            //конец перпендекуляра
            //проведенного из начала прямой(StartPointF)
            PointF p3 = new PointF(x2, y2);

            x2 = (float)(EndPointF.X + delta * cos2);
            y2 = (float)(EndPointF.Y + delta * sin2);

            //конец перпендекуляра
            //проведенного из конца прямой(EndPointF)
            PointF p4 = new PointF(x2, y2);

            return new PointF[]
                     {
                        p1,
                        p3,
                        p2,
                        p4
                     };
        }


        // see http://www.bdcc.co.uk/Gmaps/Services.htm
        // http://www.cyberforum.ru/csharp-beginners/thread1237322.html        
        public static List<TrackPoint> GDouglasPeucker(List<TrackPoint> source, int kink)
        {
            int n_source, n_stack, n_dest, start, end, i, sig;
            double dev_sqr, max_dev_sqr, band_sqr;
            double x12, y12, d12, x13, y13, d13, x23, y23, d23;
            var F = ((Math.PI / 180.0) * 0.5);
            int[] index;
            int[] sig_start;
            int[] sig_end;

            if (source.Count < 3) return (source);

            n_source = source.Count;

            // допустимое отклонение 
            band_sqr = kink * 360.0 / (2.0 * Math.PI * 6378137.0);  
            band_sqr *= band_sqr;


            n_dest = 0;
            sig_start = new int[source.Count];
            sig_end = new int[source.Count];
            index = new int[source.Count];
            sig_start[0] = 0;
            sig_end[0] = n_source - 1;
            n_stack = 1;

            while (n_stack > 0)
            {

                start = sig_start[n_stack - 1];
                end = sig_end[n_stack - 1];
                n_stack--;

                if ((end - start) > 1)
                {
                    x12 = (source[end].X - source[start].X);
                    y12 = (source[end].Y - source[start].Y);
                    if (Math.Abs(x12) > 180.0)
                        x12 = 360.0 - Math.Abs(x12);
                    x12 *= Math.Cos(F * (source[end].Y + source[start].Y));
                    d12 = (x12 * x12) + (y12 * y12);


                    for (i = start + 1, sig = start, max_dev_sqr = -1.0; i < end; i++)
                    {
                        x13 = (source[i].X - source[start].X);
                        y13 = (source[i].Y - source[start].Y);
                        if (Math.Abs(x13) > 180.0)
                            x13 = 360.0 - Math.Abs(x13);
                        x13 *= Math.Cos(F * (source[i].Y + source[start].Y));
                        d13 = (x13 * x13) + (y13 * y13);

                        x23 = (source[i].X - source[end].X);
                        y23 = (source[i].Y - source[end].Y);
                        if (Math.Abs(x23) > 180.0)
                            x23 = 360.0 - Math.Abs(x23);
                        x23 *= Math.Cos(F * (source[i].Y + source[end].Y));
                        d23 = (x23 * x23) + (y23 * y23);

                        if (d13 >= (d12 + d23))
                            dev_sqr = d23;
                        else if (d23 >= (d12 + d13))
                            dev_sqr = d13;
                        else
                            dev_sqr = (x13 * y12 - y13 * x12) * (x13 * y12 - y13 * x12) / d12;// solve triangle

                        if (dev_sqr > max_dev_sqr)
                        {
                            sig = i;
                            max_dev_sqr = dev_sqr;
                        }
                    }

                    if (max_dev_sqr < band_sqr)
                    {
                        index[n_dest] = start;
                        n_dest++;
                    }
                    else
                    {
                        n_stack++;
                        sig_start[n_stack - 1] = sig;
                        sig_end[n_stack - 1] = end;

                        n_stack++;
                        sig_start[n_stack - 1] = start;
                        sig_end[n_stack - 1] = sig;
                    }
                }
                else
                {
                    index[n_dest] = start;
                    n_dest++;
                }
            }
            index[n_dest] = n_source - 1;
            n_dest++;
            List<TrackPoint> r = new List<TrackPoint>();
            for (i = 0; i < n_dest; i++)
            {
                if (i > 0)
                {
                    if (index[i] > index[i - 1] + 1)
                    {
                        DateTime t1, t2;

                        
                        d12 = 0;
                        for(int idx = index[i - 1]; idx < index[i]; idx++)
                        {
                            t2 = source[idx+1 ].T;
                            t1 = source[idx].T;
                            d12 += source[idx+1].V * Math.Abs((t2 - t1).TotalHours);
                            
                        }

                        //d12=DistanceOnEarth(source[index[i]].X, source[index[i]].Y, source[index[i - 1]].X, source[index[i - 1]].Y);


                        t1 = source[index[i - 1]].T;
                        t2 = source[index[i]].T;
                        double V;
                        V= d12 / Math.Abs((t2 - t1).TotalHours);  
                     
                        {
                            source[index[i]].V = V;
                            //source[index[i]].M = d12.ToString() + "/" + Math.Abs((t2 - t1).TotalSeconds).ToString();
                        }

                        

                    }
                }
                r.Add(source[index[i]]);
            }
            return r;
        }

        public static  double DistanceOnEarth(double Lat1, double Long1, double Lat2, double Long2)
        {

            // определение расстояний между географическими координатами. Координаты должны быть десятичными
            // расстояние выводится в метрах
            Double Lat1r = Math.PI * Lat1 / 180;
            Double Lat2r = Math.PI * Lat2 / 180;
            Double Long1r = Math.PI * Long1 / 180;
            Double Long2r = Math.PI * Long2 / 180;
            Double dLng = Math.Abs(Long2r - Long1r);

            Double r1 = 2.0 * Math.Asin(
                Math.Sqrt(
                    Math.Pow( Math.Sin((Lat1r-Lat2r)/2), 2.0 ) + Math.Cos(Lat1r)* Math.Cos(Lat2r)* Math.Pow( Math.Sin(dLng/2), 2.0 )
                    )
                )     * 6372795.0;

           //Double r= Math.Atan2(
           //     Math.Sin( Lat1r) * Math.Sin(Lat2r) 
           //     + Math.Cos(Lat1r) * Math.Cos(Lat2r) * Math.Cos(dLng)
                
           //     , 
           //     Math.Pow(
           //         Math.Pow(Math.Cos(Lat2r) * Math.Sin(dLng), 2) 
           //             + Math.Pow(Math.Cos(Lat1r) * Math.Sin(Lat2r) 
           //             - Math.Sin(Lat1r) * Math.Cos(Lat2r) *  Math.Cos(dLng
           //             )
           //         , 2)
           //         , 
           //    0.5)
           //     ) * 6372795;

            return r1;
            
        }


    }
}
