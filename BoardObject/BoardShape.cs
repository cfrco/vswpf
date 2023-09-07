using System.Windows;
using System.Windows.Media;

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

        public abstract void Offset(Point offset);

        public abstract void Render(IRenderEngine engine);

        public abstract double MouseTest(Point position);

        public abstract IBoardObject Clone();
    }
}