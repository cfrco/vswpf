using System.Windows;
using vswpf.BoardObject;

namespace vswpf.Drawer
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

        public virtual IBoardObject GetBoardObject()
        {
            return null;
        }

        public virtual void Render(IRenderEngine engine)
        {
        }
    }
}