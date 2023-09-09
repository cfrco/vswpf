using System;
using System.Windows;
using System.Windows.Media;
using vswpf.RenderEngine;

namespace vswpf.BoardObject
{
    class BoardRectangle : BoardShape
    {
        public Point LeftTop { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public BoardRectangle() : base()
        {
        }

        public BoardRectangle(BoardRectangle rectangle) : base(rectangle)
        {
            LeftTop = rectangle.LeftTop;
            Width = rectangle.Width;
            Height = rectangle.Height;
        }

        public override void Offset(Point offset)
        {
            LeftTop = Geometry.Offset(LeftTop, offset);
        }

        public override void Render(IRenderEngine engine)
        {
            Render(engine, getBrush(), getPen(), LeftTop, Width, Height);

            if (Selected)
            {
                Pen pen = new Pen(Brushes.Black, 1);
                engine.RenderSquare(pen, LeftTop, 5);
                engine.RenderSquare(pen, Geometry.Offset(LeftTop, new Point(Width, 0)), 5);
                engine.RenderSquare(pen, Geometry.Offset(LeftTop, new Point(0, Height)), 5);
                engine.RenderSquare(pen, Geometry.Offset(LeftTop, new Point(Width, Height)), 5);
            }
        }

        public override bool MouseTest(Point position, double distance)
        {
            if (Thickness <= 0)
            {
                return position.X >= LeftTop.X && position.X <= LeftTop.X + Width &&
                    position.Y >= LeftTop.Y && position.Y <= LeftTop.Y + Height;
            }

            Point point0 = LeftTop;
            Point point1 = Geometry.Offset(LeftTop, new Point(0, Height));
            Point point2 = Geometry.Offset(LeftTop, new Point(Width, Height));
            Point point3 = Geometry.Offset(LeftTop, new Point(Width, 0));

            return Geometry.PointInLine(point0, point1, position, Thickness, distance) ||
                Geometry.PointInLine(point1, point2, position, Thickness, distance) ||
                Geometry.PointInLine(point2, point3, position, Thickness, distance) ||
                Geometry.PointInLine(point3, point0, position, Thickness, distance);
        }

        public override IBoardObject Clone()
        {
            return new BoardRectangle(this);
        }

        public static void Render(IRenderEngine engine, Brush? brush, Pen? pen, Point point, double width, double height)
        {
            Point point0 = point;
            Point point1 = Geometry.Offset(point, new Point(0, height));
            Point point2 = Geometry.Offset(point, new Point(width, height));
            Point point3 = Geometry.Offset(point, new Point(width, 0));
            engine.RenderPath(brush, pen, new Point[] { point0, point1, point2, point3 });
        }

        public static void CalcRecntagle(Point point0, Point point1, out Point leftTop, out double width, out double height)
        {
            double x1 = point0.X;
            double x2 = point1.X;
            double y1 = point0.Y;
            double y2 = point1.Y;

            if (x1 > x2)
            {
                double tmp = x2;
                x2 = x1;
                x1 = tmp;
            }
            if (y1 > y2)
            {
                double tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            leftTop = new Point(x1, y1);
            width = x2 - x1;
            height = y2 - y1;
        }
    }
}