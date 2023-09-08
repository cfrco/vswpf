using System.Windows.Media;
using vswpf.BoardObject;
using vswpf.RenderEngine;

namespace vswpf.Drawer
{
    class LineDrawer : AbsTwoPointsDrawer
    {
        public override IBoardObject GetBoardObject()
        {
            if (Geometry.Distance(start, end) <= 2)
            {
                return null;
            }

            return adjustShape(new BoardLine()
            {
                Point0 = start,
                Point1 = end,
            });
        }

        public override void Render(IRenderEngine engine)
        {
            engine.RenderLine(getPen(), start, end);
        }
    }
}