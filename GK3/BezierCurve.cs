using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace GK3
{
    internal class BezierCurve
    {
        public PointF[] ControlPoints;
        
        public BezierCurve(PointF[] points)
        {
            ControlPoints = points;
        }
    }
}
