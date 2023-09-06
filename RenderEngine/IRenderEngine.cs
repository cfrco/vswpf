using System.Windows;
using System.Windows.Media;

public interface IRenderEngine
{
    void RenderLine(Pen pen, Point point0, Point point1);

    void RenderEllipse(Brush brush, Pen pen, Point center, double radiusX, double radiusY);
}