using VsWpf.BoardObject;
using VsWpf.RenderEngine;

namespace VsWpf.Drawer
{
    class EllipseDrawer : AbsTwoPointsDrawer
    {
        public override IBoardObject? GetBoardObject()
        {
            if (Geometry.Distance(start, end) <= 2)
            {
                return null;
            }

            return adjustShape(new BoardEllipse()
            {
                Point0 = start,
                Point1 = end,
            });
        }

        public override void Render(IRenderEngine engine)
        {
            BoardEllipse.Render(engine, getBrush(), getPen(), start, end);
        }
    }
}