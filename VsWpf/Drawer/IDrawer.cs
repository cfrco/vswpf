using System.Windows;
using System.Windows.Media;
using VsWpf.BoardObject;
using VsWpf.RenderEngine;

namespace VsWpf.Drawer
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