using System;
using System.Windows;

namespace vswpf
{
    class Geometry
    {
        public static double Distance(Point p0, Point p1)
        {
            return Math.Sqrt(Math.Pow(p0.X - p1.X, 2) + Math.Pow(p0.Y - p1.Y, 2));
        }

        public static Point Offset(Point src, Point offset)
        {
            return new Point(src.X + offset.X, src.Y + offset.Y);
        }
    }
}