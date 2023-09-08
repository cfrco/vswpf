using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using vswpf.Drawer;
using vswpf.BoardObject;

namespace vswpf
{
    public partial class MainWindow : Window
    {
        private VsBoard vsBoard;
        private ShapeModifierPanel shapeModifierPanel;

        private JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
        };

        public MainWindow()
        {
            Title = "VsBoard";
            Height = 550;
            Width = 800;

            Grid grid = new Grid();
            AddChild(grid);
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(60)
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            StackPanel sPanel = new StackPanel();
            initSideBar(sPanel);
            grid.AddChild(sPanel, 0, 0);

            vsBoard = new VsBoard();
            vsBoard.ObjectSelected += vsBoard_ObjectSelected;
            grid.AddChild(vsBoard, 0, 1);
        }

        private void initSideBar(StackPanel panel)
        {
            panel.Children.Add(newButton("Line", drawerButton_ClickEvent(new LineDrawer())));
            panel.Children.Add(newButton("Rect", drawerButton_ClickEvent(new RectangleDrawer())));
            panel.Children.Add(newButton("Triangle", drawerButton_ClickEvent(new TriangleDrawer())));
            panel.Children.Add(newButton("Ellipse", drawerButton_ClickEvent(new EllipseDrawer())));
            panel.Children.Add(newButton("Hand", handButton_Click));
            panel.Children.Add(newButton("Erase", eraseButton_Click));
            panel.Children.Add(newButton("Clear", clearButton_Click));

            shapeModifierPanel = new ShapeModifierPanel();
            shapeModifierPanel.ValueChanged += shapeModifierPanel_ValueChanged;
            panel.Children.Add(shapeModifierPanel);

            panel.Children.Add(newButton("Save", saveButton_Click));
            panel.Children.Add(newButton("Load", loadButton_Click));
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
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "BoardFile (*.board)|*.board";
            Nullable<bool> result = sfd.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                return;
            }

            try
            {
                string text = JsonConvert.SerializeObject(vsBoard.GetObjects(), serializerSettings);
                System.IO.File.WriteAllText(sfd.FileName, text);
            }
            catch (Exception ex)
            {
                // TODO: beter error handling
                MessageBox.Show("Fail to save: " + ex.Message);
            }
        }
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "BoardFile (*.board)|*.board";
            Nullable<bool> result = ofd.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                return;
            }

            try
            {
                string text = System.IO.File.ReadAllText(ofd.FileName);
                IBoardObject[] boardObjects = JsonConvert.DeserializeObject<IBoardObject[]>(text, serializerSettings);
                if (boardObjects != null)
                {
                    vsBoard.SetObjects(boardObjects);
                }
            }
            catch (Exception ex)
            {
                // TODO: beter error handling
                MessageBox.Show("Fail to load: " + ex.Message);
            }
        }
        private Button newButton(string content, RoutedEventHandler handler)
        {
            Button button = new Button()
            {
                Content = content,
                Height = 40,
            };
            if (handler != null) 
            {
                button.Click += handler;
            }
            return button;
        }
        private RoutedEventHandler drawerButton_ClickEvent(IDrawer drawer)
        {
            return (sender, e) =>
            {
                drawer.SetAttributes(shapeModifierPanel.Thickness, shapeModifierPanel.Color);
                vsBoard.SetDrawer(drawer);
            };
        }

        private void vsBoard_ObjectSelected(VsBoard sender, IBoardObject obj)
        {
            shapeModifierPanel.SetObject(obj);
        }

        private void shapeModifierPanel_ValueChanged(object? sender, EventArgs e)
        {
            vsBoard.SetDrawerAttributes(shapeModifierPanel.Thickness, shapeModifierPanel.Color);
            vsBoard.InvalidateVisual();
        }
    }
}
