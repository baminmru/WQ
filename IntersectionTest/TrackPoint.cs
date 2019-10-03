using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectionTest
{
    public class TrackPoint
    {
        public Double X { get; set; }
        public Double Y { get; set; }
        public Double V { get; set; }

        public DateTime T { get; set; }
        public string M { get; set; }

        public TrackPoint() { }

        public TrackPoint(TrackPoint t) {
            X = t.X;
            Y = t.Y;
            V = t.V;
            T = t.T;
            M = t.M;
        }
    }
}
