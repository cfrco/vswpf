using System.Windows;
using vswpf.RenderEngine;

namespace vswpf.BoardObject
{
    public interface IBoardObject
    {
        bool Selected { get; set; }

        void Render(IRenderEngine engine);

        bool MouseTest(Point position, double distance);

        void Offset(Point offset);

        IBoardObject Clone();
    }
}