using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using vswpf.BoardObject;

namespace vswpf.Drawer
{
    class RectangleDrawer : AbsTwoPointsDrawer
    {
        private void calcRecntagle(out Point leftTop, out double width, out double height)
        {
            double x1 = start.X;
            double x2 = end.X;
            double y1 = start.Y;
            double y2 = end.Y;

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

        public override IBoardObject GetBoardObject()
        {
            Point leftTop;
            double width;
            double height;
            calcRecntagle(out leftTop, out width, out height);
            return new BoardRectangle()
            {
                LeftTop = leftTop,
                Width = width,
                Height = height,
            };
        }

        public override void Render(IRenderEngine engine)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            Point leftTop;
            double width;
            double height;
            calcRecntagle(out leftTop, out width, out height);
            BoardRectangle.Render(engine, pen, leftTop, width, height);
        }
    }
}