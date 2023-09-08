using System.Windows;
using System.Windows.Media;
using vswpf.RenderEngine;

namespace vswpf.BoardObject
{
    abstract class BoardShape : IBoardObject
    {
        public double Thickness { get; set; } = 1;

        public Color Color { get; set; } = Colors.Red;

        public bool Selected { get; set; } = false;

        public BoardShape()
        {
        }

        public BoardShape(BoardShape shape)
        {
            Thickness = shape.Thickness;
            Color = shape.Color;
            Selected = shape.Selected;
        }

        protected Brush? getBrush()
        {
            if (Thickness <= 0)
            {
                return new SolidColorBrush(Color);
            }
            return null;
        }
        protected Pen? getPen()
        {
            if (Thickness > 0)
            {
                return new Pen(new SolidColorBrush(Color), Thickness);
            }
            return null;
        }

        public abstract void Offset(Point offset);

        public abstract void Render(IRenderEngine engine);

        public abstract bool MouseTest(Point position, double distance);

        public abstract IBoardObject Clone();
    }
}