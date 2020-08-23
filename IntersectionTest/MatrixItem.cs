using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectionTest
{
    public class MatrixItem
    {
        public string TrackID { get; set; }
        public string FromRegion { get; set; }
        public string ToRegion { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
