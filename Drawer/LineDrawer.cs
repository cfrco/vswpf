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

            return new BoardLine()
            {
                Point0 = start,
                Point1 = end,
            };
        }

        public override void Render(IRenderEngine engine)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            engine.RenderLine(pen, start, end);
        }
    }
}