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
            Point0 = new Point(Point0.X + offset.X, Point0.Y + offset.Y);
            Point1 = new Point(Point1.X + offset.X, Point1.Y + offset.Y);
        }

        public override void Render(IRenderEngine engine)
        {
            Brush brush = new SolidColorBrush(Color);
            Pen pen = new Pen(brush, Thickness);
            Render(engine, null, pen, Point0, Point1);
        }

        public override double MouseTest(Point position)
        {
            // TODO
            return 1000000;
        }

        public override IBoardObject Clone()
        {
            return new BoardEllipse(this);
        }

        public static void Render(IRenderEngine engine, Brush brush, Pen pen, Point point0, Point point1)
        {
            double x = (point0.X + point1.X) / 2;
            double y = (point0.Y + point1.Y) / 2;
            double rx = Math.Abs(point1.X - point0.X) / 2;
            double ry = Math.Abs(point1.Y - point0.Y) / 2;
            engine.RenderEllipse(brush, pen, new Point(x, y), rx, ry);
        }
    }
}