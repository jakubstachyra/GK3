using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK3
{
    public partial class Form1 : Form
    {   
        BezierCurve curve;
        Drawer drawer;
        int movingPointIndex;
        int eps = 5;
        bool isMoving = false;

        public Form1()
        {
            InitializeComponent();
            InitializeBezierCurve();
            drawer = new Drawer(pictureBox1);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void InitializeBezierCurve()
        {
            PointF[] points = new PointF[4];
            points[0] = new PointF(100, 150);
            points[1] = new Point(120, 240);
            points[2] = new Point(280, 240);
            points[3] = new PointF(320, 150);
            curve = new BezierCurve(points);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            for(int i = 0; i < curve.ControlPoints.Count(); i++)
            {
                if (Utils.Compare(curve.ControlPoints[i], e.Location, eps)) 
                {
                    movingPointIndex = i;
                    isMoving = true;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMoving = false;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            drawer.Clear();
            drawer.DrawPoints(curve.ControlPoints);
            drawer.DrawBezier(curve.ControlPoints);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving)
            {
                Utils.MovePoint(ref curve.ControlPoints, movingPointIndex, e);
            }
                pictureBox1.Invalidate();
        }
    }
}
