using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace GK3
{
    public class Utils
    {
        public static void MovePoint(ref List<PointF> points, int index,int margin,PictureBox bezierCurvePictureBox, MouseEventArgs e)
        { 
                double x = (double)(e.Location.X - margin) / (bezierCurvePictureBox.Width - margin) * 500 + 330;
                if (x >= 380 && x <= 780) points[index] = e.Location;
        }
        public static bool  Compare(PointF p1, PointF p2, int eps)
        {
            bool result = (Math.Abs(p1.X - p2.X) < eps && Math.Abs(p1.Y - p2.Y) < eps) ? true: false;
            return result;
        }

        public static PointF CalculateBezierPoint(float t, List<PointF> controlPoints)
        {
            int n = controlPoints.Count - 1;
            PointF point = new PointF(0, 0);

            for (int i = 0; i <= n; i++)
            {
                float binomialCoefficient = BinomialCoefficient(n, i);
                float term = binomialCoefficient * (float)Math.Pow(1 - t, n - i) * (float)Math.Pow(t, i);

                point.X += term * controlPoints[i].X;
                point.Y += term * controlPoints[i].Y;
            }

            return point;
        }

        private static float BinomialCoefficient(int n, int k)
        {
            float result = 1;

            for (int i = 1; i <= k; i++)
            {
                result *= n - (k - i);
                result /= i;
            }

            return result;
        }
      
    }

}
