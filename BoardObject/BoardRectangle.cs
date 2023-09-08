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
            Pen pen = new Pen(new SolidColorBrush(Color), Thickness);
            Render(engine, pen, LeftTop, Width, Height);

            if (Selected)
            {
                pen = new Pen(Brushes.Black, 1);
                engine.RenderSquare(pen, LeftTop, 5);
                engine.RenderSquare(pen, Geometry.Offset(LeftTop, new Point(Width, 0)), 5);
                engine.RenderSquare(pen, Geometry.Offset(LeftTop, new Point(0, Height)), 5);
                engine.RenderSquare(pen, Geometry.Offset(LeftTop, new Point(Width, Height)), 5);
            }
        }

        public override double MouseTest(Point position)
        {
            double dx = 0;
            if (position.X < LeftTop.X) 
            {
                dx = LeftTop.X - position.X;
            }
            else if (position.X >= LeftTop.X && position.X <= LeftTop.X + Width)
            {
                dx = 0;
            }
            else 
            {
                dx = position.X - LeftTop.X;
            }

            double dy = 0;
            if (position.Y < LeftTop.Y)
            {
                dy = LeftTop.Y - position.Y;
            }
            else if (position.Y >= LeftTop.Y && position.Y <= LeftTop.Y + Height)
            {
                dy = 0;
            }
            else 
            {
                dy = position.Y - LeftTop.Y;
            }

            return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }

        public override IBoardObject Clone()
        {
            return new BoardRectangle(this);
        }

        public static void Render(IRenderEngine engine, Pen pen, Point point, double width, double height)
        {
            engine.RenderLine(pen, new Point(point.X, point.Y), new Point(point.X + width, point.Y));
            engine.RenderLine(pen, new Point(point.X, point.Y + height), new Point(point.X + width, point.Y + height));
            engine.RenderLine(pen, new Point(point.X, point.Y), new Point(point.X, point.Y + height));
            engine.RenderLine(pen, new Point(point.X + width, point.Y), new Point(point.X + width, point.Y + height));
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