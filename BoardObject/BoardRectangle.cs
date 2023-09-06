using System;
using System.Windows;
using System.Windows.Media;

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
            LeftTop  = new Point(LeftTop.X + offset.X, LeftTop.Y + offset.Y);
        }

        public override void Render(IRenderEngine engine)
        {
            Pen pen = new Pen() { Brush = new SolidColorBrush(Color), Thickness = Thickness };
            Render(engine, pen, LeftTop, Width, Height);
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
    }
}