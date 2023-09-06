using System.Windows.Media;
using vswpf.BoardObject;

namespace vswpf.Drawer
{
    class LineDrawer : AbsTwoPointsDrawer
    {
        public override IBoardObject GetBoardObject()
        {
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