using System;
using System.Windows;
using System.Windows.Media;

namespace vswpf.BoardObject
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
            Brush brush = new SolidColorBrush(Color);
            Pen pen = new Pen(brush, Thickness);
            Render(engine, null, pen, Point0, Point1);

            if (Selected)
            {
                pen = new Pen(Brushes.Black, 1);
                double x, y, rx, ry;
                calculateEllipse(Point0, Point1, out x, out y, out rx, out ry);
                engine.RenderSquare(pen, new Point(x + rx, y), 5);
                engine.RenderSquare(pen, new Point(x - rx, y), 5);
                engine.RenderSquare(pen, new Point(x, y + ry), 5);
                engine.RenderSquare(pen, new Point(x, y - ry), 5);
            }
        }

        public override double MouseTest(Point position)
        {
            double x, y, rx, ry;
            calculateEllipse(Point0, Point1, out x, out y, out rx, out ry);
            double ellipseEquation = Math.Pow((position.X - x) / rx, 2) + Math.Pow((position.Y - y) / ry, 2);
            if (ellipseEquation <= 1)
            {
                return 0;
            }
            return 1000; // TODO
        }

        public override IBoardObject Clone()
        {
            return new BoardEllipse(this);
        }

        public static void Render(IRenderEngine engine, Brush brush, Pen pen, Point point0, Point point1)
        {
            double x, y, rx, ry;
            calculateEllipse(point0, point1, out x, out y, out rx, out ry);
            engine.RenderEllipse(brush, pen, new Point(x, y), rx, ry);
        }

        private static void calculateEllipse(Point point0, Point point1, out double x, out double y, out double rx, out double ry)
        {
            x = (point0.X + point1.X) / 2;
            y = (point0.Y + point1.Y) / 2;
            rx = Math.Abs(point1.X - point0.X) / 2;
            ry = Math.Abs(point1.Y - point0.Y) / 2;
        }
    }
}