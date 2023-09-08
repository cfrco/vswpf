using System.Windows;
using System.Windows.Media;
using vswpf.BoardObject;
using vswpf.RenderEngine;

namespace vswpf.Drawer
{
    public interface IDrawer
    {
        bool Started { get; }

        void SetAttributes(double thickness, Color color);

        void Start(Point position);
        void Move(Point position);
        bool Click(Point position);

        void Render(IRenderEngine engine);

        IBoardObject? GetBoardObject();
    }
}