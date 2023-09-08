using System.Windows.Media;
using vswpf.BoardObject;
using vswpf.RenderEngine;

namespace vswpf.Drawer
{
    class EllipseDrawer : AbsTwoPointsDrawer
    {
        public override IBoardObject GetBoardObject()
        {
            if (Geometry.Distance(start, end) <= 2)
            {
                return null;
            }

            return new BoardEllipse()
            {
                Point0 = start,
                Point1 = end,
            };
        }

        public override void Render(IRenderEngine engine)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            BoardEllipse.Render(engine, null, pen, start, end);
        }
    }
}