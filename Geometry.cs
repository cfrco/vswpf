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

        public static double PointToLineDistance(Point point0, Point point1, Point position, out bool isOnLine)
        {
            double a = point1.Y - point0.Y;
            double b = point0.X - point1.X;
            double c = point1.X * point0.Y - point0.X * point1.Y;

            double p2ldist = Math.Abs(a * position.X + b * position.Y + c) / Math.Sqrt(a * a + b * b);
            double d1 = Distance(point0, position);
            double d2 = Distance(point1, position);
            double lineLength = Distance(point0, point1);
            double p2pLength = Math.Sqrt(d1 * d1 - p2ldist * p2ldist) + Math.Sqrt(d2 * d2 - p2ldist * p2ldist);
            if (Math.Abs(lineLength - p2pLength) > 1E-3)
            {
                isOnLine = false;
                return Math.Min(d1, d2);
            }
            isOnLine = true;
            return p2ldist;
        }

        public static bool PointInLine(Point point0, Point point1, Point position, double thickness, double distance)
        {
            bool isOnLine;
            double p2l = PointToLineDistance(point0, point1, position, out isOnLine);
            if (isOnLine)
            {
                return p2l - thickness / 2 <= distance;
            }
            return p2l <= distance;
        }

        public static Point Offset(Point src, Point offset)
        {
            return new Point(src.X + offset.X, src.Y + offset.Y);
        }
    }
}