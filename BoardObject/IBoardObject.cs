using System.Windows;
using vswpf.RenderEngine;

namespace vswpf.BoardObject
{
    public interface IBoardObject
    {
        void Render(IRenderEngine engine);

        double MouseTest(Point position);

        void Offset(Point offset);

        IBoardObject Clone();
    }
}