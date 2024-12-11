using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle_2.Models
{
    public class MonitorInfo
    {
        public IntPtr Handle { get; set; }
        public bool IsPrimary { get; set; }
        public Rectangle Bounds { get; set; }
        public Rectangle WorkArea { get; set; }

        public MonitorInfo(IntPtr handle, bool isPrimary, Rectangle bounds, Rectangle workArea)
        {
            Handle = handle;
            IsPrimary = isPrimary;
            Bounds = bounds;
            WorkArea = workArea;
        }
    }
}
