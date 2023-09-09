using System.Windows;
using System.Windows.Media;
using VsWpf.BoardObject;
using VsWpf.RenderEngine;

namespace VsWpf.Drawer
{
    abstract class AbsTwoPointsDrawer : IDrawer
    {
        protected Point start;
        protected Point end;

        protected bool started = false;
        public bool Started
        {
            get { return started; }
        }

        protected double thickness;
        protected Color color;

        public void SetAttributes(double thickness, Color color)
        {
            this.thickness = thickness;
            this.color = color;
        }

        protected Brush? getBrush()
        {
            if (thickness <= 0)
            {
                return new SolidColorBrush(color);
            }
            return null;
        }
        protected Pen? getPen()
        {
            if (thickness > 0)
            {
                return new Pen(new SolidColorBrush(color), thickness);
            }
            return null;
        }

        protected IBoardObject adjustShape(BoardShape shape)
        {
            shape.Thickness = thickness;
            shape.Color = color;
            return shape;
        }

        public void Start(Point position)
        {
            if (started)
            {
                return;
            }

            start = position;
            end = position;
            started = true;
        }

        public void Move(Point position)
        {
            if (!started)
            {
                return;
            }

            end = position;
        }

        public bool Click(Point position)
        {
            if (!started)
            {
                return false;
            }

            end = position;
            started = false;
            return true;
        }

        public virtual IBoardObject? GetBoardObject()
        {
            return null;
        }

        public virtual void Render(IRenderEngine engine)
        {
        }
    }
}