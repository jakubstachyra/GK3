using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GK3
{
    public class Utils
    {
        public static void MovePoint(ref PointF[] points, int index, MouseEventArgs e)
        {
            PointF point = points[index];

            if(point != null)
            {
                if(index == 0 || index == points.Count() - 1)
                {
                    points[index] = new PointF(point.X, e.Location.Y);
                }
                points[index] = new PointF(e.Location.X, e.Location.Y);
            }
        }
        public static bool  Compare(PointF p1, PointF p2, int eps)
        {
            if(Math.Abs(p1.X - p2.X) < eps && Math.Abs(p1.Y - p2.Y) < eps)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
