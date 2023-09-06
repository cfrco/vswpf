using System;
using System.Windows;
using System.Windows.Media;

namespace vswpf.BoardObject
{
    class BoardLine : BoardShape
    {
        public Point Point0 { get; set; }
        public Point Point1 { get; set; }

        public BoardLine() : base()
        {
        }

        public BoardLine(BoardLine line) : base(line)
        {
            Point0 = line.Point0;
            Point1 = line.Point1;
        }

        public override void Offset(Point offset)
        {
            Point0  = new Point(Point0.X + offset.X, Point0.Y + offset.Y);
            Point1  = new Point(Point1.X + offset.X, Point1.Y + offset.Y);
        }

        public override void Render(IRenderEngine engine)
        {
            Pen pen = new Pen(new SolidColorBrush(Color), Thickness);
            engine.RenderLine(pen, Point0, Point1);
        }

        public override double MouseTest(Point position)
        {
            double a = Point1.Y - Point0.Y;
            double b = Point0.X - Point1.X;
            double c = Point1.X * Point0.Y - Point0.X * Point1.Y;

            return Math.Abs(a * position.X + b * position.Y + c) / Math.Sqrt(a * a + b * b);
        }

        public override IBoardObject Clone()
        {
            return new BoardLine(this);
        }
    }
}