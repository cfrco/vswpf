using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace VsWpf
{
    static class GridExtensions
    {
        public static void AddChild(this Grid grid, UIElement child, int row, int col)
        {
            Grid.SetRow(child, row);
            Grid.SetColumn(child, col);
            grid.Children.Add(child);
        }
    }
}