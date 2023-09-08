using System;
using System.Windows;
using System.Windows.Media;
using vswpf.RenderEngine;

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
            Point0 = Geometry.Offset(Point0, offset);
            Point1 = Geometry.Offset(Point1, offset);
        }

        public override void Render(IRenderEngine engine)
        {
            Pen pen = new Pen(new SolidColorBrush(Color), Thickness);
            engine.RenderLine(pen, Point0, Point1);

            if (Selected)
            {
                pen = new Pen(Brushes.Black, 1);
                engine.RenderSquare(pen, Point0, 5);
                engine.RenderSquare(pen, Point1, 5);
            }
        }

        public override double MouseTest(Point position)
        {
            double a = Point1.Y - Point0.Y;
            double b = Point0.X - Point1.X;
            double c = Point1.X * Point0.Y - Point0.X * Point1.Y;

            double dist = Math.Abs(a * position.X + b * position.Y + c) / Math.Sqrt(a * a + b * b);
            double d1 = Geometry.Distance(Point0, position);
            double d2 = Geometry.Distance(Point1, position);
            double rLine = Geometry.Distance(Point0, Point1);
            double aLine = Math.Sqrt(d1 * d1 - dist * dist) + Math.Sqrt(d2 * d2 - dist * dist);
            if (Math.Abs(rLine - aLine) > 1E-3)
            {
                return Math.Min(d1, d2);
            }
            return dist;
        }

        public override IBoardObject Clone()
        {
            return new BoardLine(this);
        }
    }
}