using System;
using System.Collections.Generic;
using System.Drawing;
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

        public void DrawPoints(PointF[] ControlPoints)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                foreach(var point in ControlPoints)
                {
                    g.FillEllipse(brush, point.X - eps, point.Y - eps, pointWidth, pointHeight);
                }
            }
            pictureBox.Image = bitmap;
        }
        public void DrawBezier(PointF[] ControlPoints)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {   
                g.DrawBeziers(pen, ControlPoints);

            }
            pictureBox.Image = bitmap;
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
