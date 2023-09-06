using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using vswpf.Drawer;

namespace vswpf
{
    public partial class MainWindow : Window
    {
        private VsBoard vsBoard;

        public MainWindow()
        {
            Title = "VsBoard";
            Height = 450;
            Width = 800;

            Grid grid = new Grid();

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(50);
            ColumnDefinition col2 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);

            RowDefinition row = new RowDefinition();
            grid.RowDefinitions.Add(row);

            AddChild(grid);

            StackPanel sPanel = new StackPanel();
            initSideBar(sPanel);
            Grid.SetColumn(sPanel, 0);
            Grid.SetRow(sPanel, 0);
            grid.Children.Add(sPanel);

            vsBoard = new VsBoard();
            Grid.SetColumn(vsBoard, 1);
            Grid.SetRow(vsBoard, 0);
            grid.Children.Add(vsBoard);
        }

        private void initSideBar(StackPanel panel)
        {
            Button button1 = newButton("Line", button1_Click);
            panel.Children.Add(button1);

            Button button2 = newButton("Rect", button2_Click);
            panel.Children.Add(button2);

            Button ellipseButton = newButton("Ellipse", ellipseButton_Click);
            panel.Children.Add(ellipseButton);

            Button handButton = newButton("Hand", handButton_Click);
            panel.Children.Add(handButton);

            Button eraseButton = newButton("Erase", eraseButton_Click);
            panel.Children.Add(eraseButton);

            Button clearButton = newButton("Clear", clearButton_Click);
            panel.Children.Add(clearButton);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.SetDrawer(new LineDrawer());
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.SetDrawer(new RectangleDrawer());
        }
        private void ellipseButton_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.SetDrawer(new EllipseDrawer());
        }
        private void handButton_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.SetDrawer(null);
            vsBoard.SetHandMode(HandMode.Move);
        }
        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.SetDrawer(null);
            vsBoard.SetHandMode(HandMode.Erase);
        }
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.ClearObjects();
        }

        private Button newButton(string content, RoutedEventHandler handler)
        {
            Button button = new Button();
            button.Content = content;
            button.Height = 50;
            if (handler != null) 
            {
                button.Click += handler;
            }
            return button;
        }
    }
}
