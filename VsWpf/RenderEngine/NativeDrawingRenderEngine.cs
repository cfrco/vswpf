using System.Windows;
using System.Windows.Media;

namespace VsWpf.RenderEngine
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

        public void RenderPath(Brush? brush, Pen? pen, Point[] points)
        {
            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(points[0], true, true); // Start point, isFilled, isClosed
                ctx.PolyLineTo(points, true, true); // isStroked, isSmoothJoin
            }
            geometry.Freeze();

            dc.DrawGeometry(brush, pen, geometry);
        }

        public void RenderEllipse(Brush? brush, Pen? pen, Point center, double radiusX, double radiusY)
        {
            dc.DrawEllipse(brush, pen, center, radiusX, radiusY);
        }
    }
}