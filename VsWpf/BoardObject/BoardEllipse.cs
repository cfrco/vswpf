using System;
using System.Windows;
using System.Windows.Media;
using VsWpf.RenderEngine;

namespace VsWpf.BoardObject
{
    class BoardEllipse : BoardShape
    {
        public Point Point0 { get; set; }
        public Point Point1 { get; set; }

        public BoardEllipse() : base()
        {
        }

        public BoardEllipse(BoardEllipse ellipse) : base(ellipse)
        {
            Point0 = ellipse.Point0;
            Point1 = ellipse.Point1;
        }

        public override void Offset(Point offset)
        {
            Point0 = Geometry.Offset(Point0, offset);
            Point1 = Geometry.Offset(Point1, offset);
        }

        public override void Render(IRenderEngine engine)
        {
            Render(engine, getBrush(), getPen(), Point0, Point1);

            if (Selected)
            {
                Pen pen = new Pen(Brushes.Black, 1);
                double x, y, rx, ry;
                CalculateEllipse(Point0, Point1, out x, out y, out rx, out ry);
                engine.RenderSquare(pen, new Point(x + rx, y), 5);
                engine.RenderSquare(pen, new Point(x - rx, y), 5);
                engine.RenderSquare(pen, new Point(x, y + ry), 5);
                engine.RenderSquare(pen, new Point(x, y - ry), 5);
            }
        }

        public override bool MouseTest(Point position, double distance)
        {
            double x, y, rx, ry;
            CalculateEllipse(Point0, Point1, out x, out y, out rx, out ry);

            double thickness = Thickness > 0 ? Thickness : 0;

            double rx1 = rx - thickness / 2 - distance;
            double ry1 = ry - thickness / 2 - distance;
            double eq1 = Math.Pow((position.X - x) / rx1, 2) + Math.Pow((position.Y - y) / ry1, 2);
            double rx2 = rx + thickness / 2 + distance;
            double ry2 = ry + thickness / 2 + distance;
            double eq2 = Math.Pow((position.X - x) / rx2, 2) + Math.Pow((position.Y - y) / ry2, 2);
            if (thickness <= 0)
            {
                return eq2 <= 1;
            }
            return eq1 > 1 && eq2 <= 1;
        }

        public override IBoardObject Clone()
        {
            return new BoardEllipse(this);
        }

        public static void Render(IRenderEngine engine, Brush? brush, Pen? pen, Point point0, Point point1)
        {
            double x, y, rx, ry;
            CalculateEllipse(point0, point1, out x, out y, out rx, out ry);
            engine.RenderEllipse(brush, pen, new Point(x, y), rx, ry);
        }

        public static void CalculateEllipse(Point point0, Point point1, out double x, out double y, out double rx, out double ry)
        {
            x = (point0.X + point1.X) / 2;
            y = (point0.Y + point1.Y) / 2;
            rx = Math.Abs(point1.X - point0.X) / 2;
            ry = Math.Abs(point1.Y - point0.Y) / 2;
        }
    }
}