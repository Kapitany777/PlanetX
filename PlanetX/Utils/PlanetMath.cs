using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PlanetX.Utils
{
    public class PlanetMath
    {
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double d = Math.Sqrt(dx * dx + dy * dy);

            return d;
        }

        public static double Length(double x, double y)
        {
            double d = Math.Sqrt(x * x + y * y);

            return d;
        }

        public static double RotationAngle(double x1, double y1, double x2, double y2)
        {
            double pl = 0.0;
            
            if (y1 > y2)
            {
                double tmp = x1;
                x1 = x2;
                x2 = tmp;

                pl = 180.0;
            }

            double a = x2 - x1;
            double b = y2 - y1;
            double r = Length(a, b);

            double angle = Math.Acos(a / r) * 180.0 / Math.PI + pl;

            return angle;
        }
    }
}
