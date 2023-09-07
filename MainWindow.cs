using System;
using System.Windows;
using System.Windows.Controls;
using vswpf.Drawer;
using vswpf.BoardObject;

namespace vswpf
{
    public partial class MainWindow : Window
    {
        private VsBoard vsBoard;
        private ShapeModifierPanel shapeModifierPanel;

        public MainWindow()
        {
            Title = "VsBoard";
            Height = 450;
            Width = 800;

            Grid grid = new Grid();

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(60);
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
            vsBoard.ObjectSelected += vsBoard_ObjectSelected;
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

            Button triangleButton = newButton("Triangle", triangleButton_Click);
            panel.Children.Add(triangleButton);

            Button ellipseButton = newButton("Ellipse", ellipseButton_Click);
            panel.Children.Add(ellipseButton);

            Button handButton = newButton("Hand", handButton_Click);
            panel.Children.Add(handButton);

            Button eraseButton = newButton("Erase", eraseButton_Click);
            panel.Children.Add(eraseButton);

            Button clearButton = newButton("Clear", clearButton_Click);
            panel.Children.Add(clearButton);

            shapeModifierPanel = new ShapeModifierPanel();
            shapeModifierPanel.ValueChanged += shapeModifierPanel_ValueChanged;
            panel.Children.Add(shapeModifierPanel);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.SetDrawer(new LineDrawer());
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.SetDrawer(new RectangleDrawer());
        }
        private void triangleButton_Click(object sender, RoutedEventArgs e)
        {
            vsBoard.SetDrawer(new TriangleDrawer());
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

        private void vsBoard_ObjectSelected(VsBoard sender, IBoardObject obj)
        {
            if (obj == null)
            {
                return;
            }

            shapeModifierPanel.SetObject(obj);
        }

        private void shapeModifierPanel_ValueChanged(object sender, EventArgs e)
        {
            vsBoard.InvalidateVisual();
        }
    }
}
