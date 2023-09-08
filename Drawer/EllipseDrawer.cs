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

            return adjustShape(new BoardEllipse()
            {
                Point0 = start,
                Point1 = end,
            });
        }

        public override void Render(IRenderEngine engine)
        {
            BoardEllipse.Render(engine, null, getPen(), start, end);
        }
    }
}