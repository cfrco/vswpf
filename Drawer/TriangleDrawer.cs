using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Media;
using vswpf.BoardObject;

namespace vswpf.Drawer
{
    class TriangleDrawer : AbsTwoPointsDrawer
    {
        public override IBoardObject GetBoardObject()
        {
            if (Geometry.Distance(start, end) <= 2)
            {
                return null;
            }

            Point point0;
            Point point1;
            Point point2;
            calculateTrinagle(out point0, out point1, out point2);
            return new BoardTriangle()
            {
                Point0 = point0,
                Point1 = point1,
                Point2 = point2,
            };
        }

        public override void Render(IRenderEngine engine)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            Point point0;
            Point point1;
            Point point2;
            calculateTrinagle(out point0, out point1, out point2);
            engine.RenderLine(pen, point0, point1);
            engine.RenderLine(pen, point1, point2);
            engine.RenderLine(pen, point0, point2);
        }

        private void calculateTrinagle(out Point point0, out Point point1, out Point point2)
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

            point0 = new Point((x1 + x2) / 2, y1);
            point1 = new Point(x2, y2);
            point2 = new Point(x1, y2);
        }
    }
}