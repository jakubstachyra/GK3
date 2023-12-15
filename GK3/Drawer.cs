using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace GK3
{
    internal class Drawer
    {
        PictureBox pictureBox;
        Bitmap bitmap { get; set; }
        Pen pen = new Pen(Color.Black, 1);
        Brush brush = new SolidBrush(Color.Black);
        int pointWidth = 7;
        int pointHeight = 7;
        int eps = 5;

        public Drawer(PictureBox _pictureBox)
        {
            pictureBox = _pictureBox;
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
        }

        public void DrawPoints(object sender, PaintEventArgs e, List<PointF> ControlPoints)
        {
            Graphics g = e.Graphics;
            foreach(var point in ControlPoints)
            {
                g.FillEllipse(brush, point.X - eps, point.Y - eps, pointWidth, pointHeight);
            }
        }
        public void DrawBezier(object sender, PaintEventArgs e, List<PointF> curvePoints)
        {
            Graphics g = e.Graphics;
            g.DrawCurve(pen, curvePoints.ToArray());
        }
        public void DrawDottedLines(object sender, PaintEventArgs e, List<PointF> ControlPoints)
        {   Graphics g = e.Graphics;
            pen.DashStyle = DashStyle.Dash;
            pen.Color = Color.LightGreen;

            g.DrawLines(pen, ControlPoints.ToArray());
            pen.DashStyle = DashStyle.Solid;
            pen.Color = Color.Black;
        }
        public void DrawChart(object sender, PaintEventArgs e)
        {   
            Graphics g = e.Graphics;

            // Rysowanie osi

            for (int x = 380; x <= 780; x += 50)
            {
                int xPos = 30 + (x - 330); // Skalowanie X
                g.DrawLine(Pens.Black, xPos, 390, xPos, 410); // Małe linie na osi X
                g.DrawString(x.ToString(), new Font("Arial", 8), Brushes.Black, xPos - 10, 410);
            }

            // Rysowanie osi Y
            for (float y = 0; y <= 2; y += 0.2f)
            {
                int yPos = 400 - (int)(y * 200); // Skalowanie Y
                g.DrawLine(Pens.Black, 40, yPos, 60, yPos); // Małe linie na osi Y
                g.DrawString(y.ToString("0.0"), new Font("Arial", 8), Brushes.Black,15, yPos - 10);
            }

            // Rysowanie osi
            pen.EndCap = LineCap.ArrowAnchor;
            g.DrawLine(pen, 50, 400, 500, 400);
            g.DrawLine(pen, 50, 400, 50, 25); 
            pen.EndCap = LineCap.NoAnchor;
        }
        public void Clear()
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);

            }
        }
    }
}
