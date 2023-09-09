using System.Windows;
using System.Windows.Media;
using VsWpf.RenderEngine;

namespace VsWpf.BoardObject
{
    class BoardTriangle : BoardShape
    {
        public Point Point0 { get; set; }
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }

        public BoardTriangle() : base()
        {
        }

        public BoardTriangle(BoardTriangle triangle) : base(triangle)
        {
            Point0 = triangle.Point0;
            Point1 = triangle.Point1;
            Point2 = triangle.Point2;
        }

        public override void Offset(Point offset)
        {
            Point0 = Geometry.Offset(Point0, offset);
            Point1 = Geometry.Offset(Point1, offset);
            Point2 = Geometry.Offset(Point2, offset);
        }

        public override void Render(IRenderEngine engine)
        {
            Render(engine, getBrush(), getPen(), Point0, Point1, Point2);

            if (Selected)
            {
                Pen pen = new Pen(Brushes.Black, 1);
                engine.RenderSquare(pen, Point0, 5);
                engine.RenderSquare(pen, Point1, 5);
                engine.RenderSquare(pen, Point2, 5);
            }
        }

        public override bool MouseTest(Point position, double distance)
        {
            if (Thickness <= 0)
            {
                // From ChatGPT
                // Compute barycentric coordinates
                double denom = (Point1.Y - Point2.Y) * (Point0.X - Point2.X) + (Point2.X - Point1.X) * (Point0.Y - Point2.Y);
                double a = ((Point1.Y - Point2.Y) * (position.X - Point2.X) + (Point2.X - Point1.X) * (position.Y - Point2.Y)) / denom;
                double b = ((Point2.Y - Point0.Y) * (position.X - Point2.X) + (Point0.X - Point2.X) * (position.Y - Point2.Y)) / denom;
                double c = 1.0 - a - b;

                // Check if point is inside the triangle
                return (0 <= a && a <= 1 && 0 <= b && b <= 1 && 0 <= c && c <= 1);
            }

            return Geometry.PointInLine(Point0, Point1, position, Thickness, distance) ||
                Geometry.PointInLine(Point1, Point2, position, Thickness, distance) ||
                Geometry.PointInLine(Point2, Point0, position, Thickness, distance);
        }

        public override IBoardObject Clone()
        {
            return new BoardTriangle(this);
        }

        public static void Render(IRenderEngine engine, Brush? brush, Pen? pen, Point point0, Point point1, Point point2)
        {
            engine.RenderPath(brush, pen, new Point[] { point0, point1, point2 });
        }
    }
}