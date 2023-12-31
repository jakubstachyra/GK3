﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK3
{
    public partial class Form1 : Form
    {
        private static readonly double[,] XyzToRgbMatrix = new double[3, 3]{
        { 3.241, -1.5374, -0.4986},
        { -0.9692,  1.876,  0.0416},
        { 0.0556, -0.204,  1.057} };
        BezierCurve curve;
        Drawer drawer;
        int movingPointIndex;
        int eps = 5;
        int margin = 50;
        double k = 0;
        int curveControlPointsCount = 4;
        bool isMoving = false;
        static CIEXYZ[] waveLengths = new CIEXYZ[781];
        CIEXYZ bezierCurvePoint;
        public Form1()
        {
            InitializeComponent();        
            InitializeBezierCurve();
            drawer = new Drawer(pictureBox1);
            CIEXYZ.InitializeWaveLenghts(ref waveLengths, ref k);
            bezierCurvePoint = new CIEXYZ(0, 0, 0);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void InitializeBezierCurve()
        {
            List<PointF>points = new List<PointF>();
            int x = ((pictureBox1.Width - margin) / (curveControlPointsCount + 1)) + margin;
            for (int i = 0; i < curveControlPointsCount; ++i)
            {
                points.Add(new PointF(x, (int)((pictureBox1.Height - margin) * 0.7)));
                x += (pictureBox1.Width - margin) / (curveControlPointsCount + 1);
            }
            curve = new BezierCurve(points);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            for(int i = 0; i < curve.controlPoints.Count(); i++)
            {
                if (Utils.Compare(curve.controlPoints[i], e.Location, eps)) 
                {
                    movingPointIndex = i;
                    isMoving = true;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMoving = false;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            drawer.Clear();
            drawer.DrawChart(sender,e);
            drawer.DrawPoints(sender, e, curve.controlPoints);
            drawer.DrawBezier(sender, e, curve.bezierCurve);
            drawer.DrawDottedLines(sender, e, curve.controlPoints);
            LightPoint();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving)
            {
                Utils.MovePoint(ref curve.controlPoints, movingPointIndex,margin,pictureBox1, e);
                curve.UpdateCurve();
            }
                pictureBox1.Invalidate();
        }

        private void degreeUpDown_ClientSizeChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            curveControlPointsCount = (int)numericUpDown1.Value;
            InitializeBezierCurve();
            pictureBox1.Invalidate();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Pen arrowPen = new Pen(Color.Black, 1)
            {
                CustomEndCap = new AdjustableArrowCap(5, 5)
            };
            e.Graphics.DrawLine(arrowPen, new Point(0, pictureBox2.Height - margin), new Point(pictureBox2.Width - margin, pictureBox2.Height - margin));
            e.Graphics.DrawLine(arrowPen, new Point(margin, pictureBox2.Height), new Point(margin, 0));

            if (backgroundCheckBox.Checked)
            {
                Bitmap backgroundBitmap = new Bitmap(new Bitmap(@"D:\Kuba Studia\Grafika\GK3\GK3\Resources\background.png"),
                    (int)(0.74 * (Math.Min(pictureBox2.Width, pictureBox2.Height) - margin)),
                    (int)(0.84 * (Math.Min(pictureBox2.Width, pictureBox2.Height) - margin)));
                e.Graphics.DrawImage(backgroundBitmap, new Point(margin, pictureBox2.Height - backgroundBitmap.Height - margin));
            }

            if (dotscheckBox.Checked)
            {
                for (int i = 380; i <= 700; ++i)
                {
                    double sumC = waveLengths[i].X + waveLengths[i].Y + waveLengths[i].Z;
                    double xC = sumC == 0 ? 0 : waveLengths[i].X / sumC;
                    double yC = sumC == 0 ? 0 : waveLengths[i].Y / sumC;
                    int rColor = Math.Min((int)(255 * Math.Pow(Math.Max(XyzToRgbMatrix[0, 0] * waveLengths[i].X + XyzToRgbMatrix[0, 1] * waveLengths[i].Y +
                        XyzToRgbMatrix[0, 2] * waveLengths[i].Z, 0), 1 / 2.2)), 255);
                    int gColor = Math.Min((int)(255 * Math.Pow(Math.Max(XyzToRgbMatrix[1, 0] * waveLengths[i].X + XyzToRgbMatrix[1, 1] * waveLengths[i].Y +
                        XyzToRgbMatrix[1, 2] * waveLengths[i].Z, 0), 1 / 2.2)), 255);
                    int bColor = Math.Min((int)(255 * Math.Pow(Math.Max(XyzToRgbMatrix[2, 0] * waveLengths[i].X + XyzToRgbMatrix[2, 1] * waveLengths[i].Y +
                        XyzToRgbMatrix[2, 2] * waveLengths[i].Z, 0), 1 / 2.2)), 255);

                    e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(rColor, gColor, bColor)), new Rectangle(((int)(xC * (Math.Min(pictureBox2.Width,
                        pictureBox2.Height) - margin)) + margin) - 5,
                        (pictureBox2.Height - (int)(yC * (Math.Min(pictureBox2.Width, pictureBox2.Height) - margin)) - margin) - 5, 10, 10));
                }

                if (bezierCurvePoint == null) return;
                double sum = bezierCurvePoint.X + bezierCurvePoint.Y + bezierCurvePoint.Z;
                double x = sum == 0 ? 0 : bezierCurvePoint.X / sum;
                double y = sum == 0 ? 0 : bezierCurvePoint.Y / sum;
                Point p = new Point((int)(x * (Math.Min(pictureBox2.Width, pictureBox2.Height) - margin)) + margin,
                    pictureBox2.Height - (int)(y * (Math.Min(pictureBox2.Width, pictureBox2.Height) - margin)) - margin);

                e.Graphics.FillEllipse(Brushes.DarkMagenta, new Rectangle(p.X - 5, p.Y - 5, 10, 10));
                e.Graphics.DrawString(x.ToString("f") + ", " + y.ToString("f"), new Font("Arial", 10), Brushes.Black, p);
            }
        }
        public  void LightPoint()
        {
            bezierCurvePoint.X = bezierCurvePoint.Y = bezierCurvePoint.Z = 0;
            PointF pointA = curve.controlPoints[0];
            PointF pointB;
            for (double i = 0; i <= 1; i += 0.01)
            {
                pointB = new Point(0, 0);
                for (int j = 0; j <= curveControlPointsCount - 1; j++)
                {
                    int r = 1;
                    int n = curveControlPointsCount - 1;
                    if (j > n) r = 0;
                    else for (int l = 1; l <= j; ++l) r = (r * n--) / l;

                    pointB.X += (int)(curve.controlPoints[j].X * r * Math.Pow(1 - i, curveControlPointsCount - 1 - j) * Math.Pow(i, j));
                    pointB.Y += (int)(curve.controlPoints[j].Y * r * Math.Pow(1 - i, curveControlPointsCount - 1 - j) * Math.Pow(i, j));
                }

                double x1 = (double)(pointA.X - margin) / (pictureBox2.Width - margin) * 499 + 330;
                if(x1 >780)
                {
                    x1 = 780;
                }
                double y1 = (double)(pointA.Y - margin) / (pictureBox2.Height - margin) * 1.8;
                double x2 = (double)(pointB.X - margin) / (pictureBox2.Width - margin) * 499 + 330;
                if (x2 > 780)
                {
                    x2 = 780;
                }
                double y2 = (double)(pointB.Y - margin) / (pictureBox2.Height - margin) * 1.8;
                CIEXYZ v1 = waveLengths[(int)x1];
                CIEXYZ v2 = waveLengths[(int)x2];
                int h = (int)( Math.Abs(pointB.X - pointA.X));
                if (v1 != null && v2 != null)
                {
                    bezierCurvePoint.X += (v1.X * y1 + v2.X * y2) * h / 2;
                    bezierCurvePoint.Y += (v1.Y * y1 + v2.Y * y2) * h / 2;
                    bezierCurvePoint.Z += (v1.Z * y1 + v2.Z * y2) * h / 2;
                }
                pointA = pointB;
            }
            bezierCurvePoint.X *= k;
            bezierCurvePoint.Y *= k;
            bezierCurvePoint.Z *= k;
            pictureBox2.Invalidate();
        }
        private void backgroundCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
