using System.Windows;
using System.Windows.Media;

namespace vswpf.RenderEngine
{
    public class NativeDrawingRenderEngine : IRenderEngine
    {
        private DrawingContext dc;

        public NativeDrawingRenderEngine(DrawingContext dc)
        {
            this.dc = dc;
        }

        public void RenderLine(Pen pen, Point point0, Point point1)
        {
            dc.DrawLine(pen, point0, point1);
        }

        public void RenderEllipse(Brush brush, Pen pen, Point center, double radiusX, double radiusY)
        {
            dc.DrawEllipse(brush, pen, center, radiusX, radiusY);
        }
    }
}