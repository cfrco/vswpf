using System.Windows;
using System.Windows.Media;
using vswpf.RenderEngine;

namespace vswpf.BoardObject
{
    class Crosshairs
    {
        public static void Render(IRenderEngine engine, Point point, int size, Brush brush)
        {
            Pen pen = new Pen(brush, 1);
            double x = point.X + 0.5;
            double y = point.Y + 0.5;
            double hs = size / 2;
            engine.RenderLine(pen, new Point(x - hs, y), new Point(x + hs, y));
            engine.RenderLine(pen, new Point(x, y - hs), new Point(x, y + hs));
        }

        public static void RenderMove(IRenderEngine engine, Point point, int size, Brush brush)
        {
            Render(engine, point, size, brush);

            Pen pen = new Pen(brush, 1);
            double x = point.X + 0.5;
            double y = point.Y + 0.5;
            double hs = size / 2;
            float arrow = 2;

            engine.RenderLine(pen, new Point(x - hs, y), new Point(x - hs + arrow, y - arrow));
            engine.RenderLine(pen, new Point(x - hs, y), new Point(x - hs + arrow, y + arrow));
            engine.RenderLine(pen, new Point(x + hs, y), new Point(x + hs - arrow, y - arrow));
            engine.RenderLine(pen, new Point(x + hs, y), new Point(x + hs - arrow, y + arrow));
            engine.RenderLine(pen, new Point(x, y - hs), new Point(x - arrow, y - hs + arrow));
            engine.RenderLine(pen, new Point(x, y - hs), new Point(x + arrow, y - hs + arrow));
            engine.RenderLine(pen, new Point(x, y + hs), new Point(x - arrow, y + hs - arrow));
            engine.RenderLine(pen, new Point(x, y + hs), new Point(x + arrow, y + hs - arrow));
        }
    }
}
