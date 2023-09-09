using System.Windows.Media;
using VsWpf.BoardObject;
using VsWpf.RenderEngine;

namespace VsWpf.Drawer
{
    class LineDrawer : AbsTwoPointsDrawer
    {
        public override IBoardObject? GetBoardObject()
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
            engine.RenderLine(new Pen(new SolidColorBrush(color), thickness > 0 ? thickness : 1), start, end);
        }
    }
}