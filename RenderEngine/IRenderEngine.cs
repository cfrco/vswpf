using System.Windows;
using System.Windows.Media;

namespace vswpf.RenderEngine
{
    public interface IRenderEngine
    {
        void RenderLine(Pen pen, Point point0, Point point1);

        void RenderEllipse(Brush brush, Pen pen, Point center, double radiusX, double radiusY);

        void RenderPath(Brush brush, Pen pen, Point[] points);
    }
}