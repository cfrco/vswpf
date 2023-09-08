using System.Windows;
using System.Windows.Media;
using vswpf.BoardObject;
using vswpf.RenderEngine;

namespace vswpf.Drawer
{
    class RectangleDrawer : AbsTwoPointsDrawer
    {
        public override IBoardObject GetBoardObject()
        {
            if (Geometry.Distance(start, end) <= 2)
            {
                return null;
            }

            Point leftTop;
            double width;
            double height;
            BoardRectangle.CalcRecntagle(start, end, out leftTop, out width, out height);
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
            BoardRectangle.CalcRecntagle(start, end, out leftTop, out width, out height);
            BoardRectangle.Render(engine, pen, leftTop, width, height);
        }
    }
}