using System.Windows;
using System.Windows.Media;

public interface IRenderEngine
{
    void RenderLine(Pen pen, Point point0, Point point1);

    void RenderEllipse(Brush brush, Pen pen, Point center, double radiusX, double radiusY);
}

static class RenderEngineExtensions
{
    public static void RenderSquare(this IRenderEngine engine, Pen pen, Point center, double size)
    {
        double x = center.X - size / 2;
        double y = center.Y - size / 2;
        engine.RenderLine(pen, new Point(x, y), new Point(x + size, y));
        engine.RenderLine(pen, new Point(x, y + size), new Point(x + size, y + size));
        engine.RenderLine(pen, new Point(x, y), new Point(x, y + size));
        engine.RenderLine(pen, new Point(x + size, y), new Point(x + size, y + size));
    }
}