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
        public List<PointF> controlPoints;
        public List<PointF> bezierCurve;
        public BezierCurve(List<PointF> points)
        {
            controlPoints = points;
            bezierCurve = new List<PointF>();
            UpdateCurve();
        }
        public  void UpdateCurve()
        {
            bezierCurve.Clear();
            int numberOfPoints = controlPoints.Count;

            for (int i = 0; i <= numberOfPoints; i++)
            {
                float t = i / (float)numberOfPoints;
                PointF bezierPoint = Utils.CalculateBezierPoint(t, controlPoints);
                bezierCurve.Add(bezierPoint);
            } 
        }

    }
}
