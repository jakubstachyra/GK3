using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GK3
{
    public class CIEXYZ
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public CIEXYZ(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public static void InitializeWaveLenghts(ref CIEXYZ[] waveLengths, ref double k)
        {

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            string[] lines = System.IO.File.ReadAllLines(@"D:\Kuba Studia\Grafika\GK3\GK3\Resources\data.txt");
            foreach (var line in lines)
            {
                string[] subs = line.Split(' ', '\t');
                int wl = int.Parse(subs[0]);
                double x = double.Parse(subs[1]);
                double y = double.Parse(subs[2]);
                double z = double.Parse(subs[3]);
                waveLengths[wl] = new CIEXYZ(x, y, z);
                k += waveLengths[wl].Y;
            }
            k = 1 / k;
        } 
    }
}
