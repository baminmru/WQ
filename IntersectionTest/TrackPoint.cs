using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectionTest
{
    public class TrackPoint: IEquatable<TrackPoint> , IComparable<TrackPoint>
    {
        public Double X { get; set; }
        public Double Y { get; set; }
        public Double V { get; set; }

        public DateTime T { get; set; }
        

        public string LINKTO { get; set; }

        public double WAY { get; set; }

        public double SPD { get; set; }

        public TrackPoint() {
            WAY = 0.0;
            SPD = 0.0;
            LINKTO = "";
        }

        public TrackPoint(TrackPoint t) {
            X = t.X;
            Y = t.Y;
            V = t.V;
            T = t.T;
            LINKTO = t.LINKTO;
            WAY = t.WAY;
            SPD = t.SPD;
        }

        bool IEquatable<TrackPoint>.Equals(TrackPoint other)
        {
            return this.T.Equals(other.T);
        }

        int IComparable<TrackPoint>.CompareTo(TrackPoint other)
        {
            return this.T.CompareTo(other.T);
        }
    }
}
