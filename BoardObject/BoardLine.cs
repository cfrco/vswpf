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
            engine.RenderLine(new Pen(new SolidColorBrush(Color), Thickness > 0 ? Thickness : 1), Point0, Point1);

            if (Selected)
            {
                Pen pen = new Pen(Brushes.Black, 1);
                engine.RenderSquare(pen, Point0, 5);
                engine.RenderSquare(pen, Point1, 5);
            }
        }

        public override bool MouseTest(Point position, double distance)
        {
            double thickness = Thickness > 0 ? Thickness : 1;
            return Geometry.PointInLine(Point0, Point1, position, thickness, distance);
        }

        public override IBoardObject Clone()
        {
            return new BoardLine(this);
        }
    }
}