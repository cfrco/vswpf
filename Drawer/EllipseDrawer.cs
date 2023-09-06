
using System.Windows;
using System.Windows.Media;
using vswpf.BoardObject;

namespace vswpf.Drawer
{
    class EllipseDrawer : AbsTwoPointsDrawer
    {
        public override IBoardObject GetBoardObject()
        {
            return new BoardEllipse()
            {
                Point0 = start,
                Point1 = end,
            };
        }

        public override void Render(IRenderEngine engine)
        {
            Brush brush = null;
            Pen pen = new Pen(Brushes.Red, 1);
            BoardEllipse.Render(engine, brush, pen, start, end);
        }
    }
}