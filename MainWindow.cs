using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using vswpf.Drawer;
using vswpf.BoardObject;
using System.Collections.Generic;
using System.ComponentModel;

namespace vswpf
{
    public partial class MainWindow : Window
    {
        // Controls, Elements
        private VsBoard vsBoard;
        private ShapeModifierPanel shapeModifierPanel;

        // Board
        private IBoardObject? selectedObject;
        private IBoardObject? copiedObject;

        // Exporting
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
            grid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(24)
            });
            grid.RowDefinitions.Add(new RowDefinition());

            ToolBar toolBar = new ToolBar();
            toolBar.Items.Add(newButton("Save", saveButton_Click));
            toolBar.Items.Add(newButton("Load", loadButton_Click));
            toolBar.Items.Add(new Separator());
            toolBar.Items.Add(newButton("Clear", clearButton_Click));
            toolBar.Items.Add(new Separator());
            toolBar.Items.Add(newButton("Undo", undoButton_Click));
            toolBar.Items.Add(newButton("Redo", redoButton_Click));
            grid.AddChild(toolBar, 0, 0);
            Grid.SetColumnSpan(toolBar, 2);

            StackPanel sPanel = new StackPanel();
            initSideBar(sPanel);
            grid.AddChild(sPanel, 1, 0);

            vsBoard = new VsBoard();
            vsBoard.ObjectSelected += vsBoard_ObjectSelected;
            grid.AddChild(vsBoard, 1, 1);

            KeyDown += onKeyDown;
            KeyUp += onKeyUp;
        }

        private void initSideBar(StackPanel panel)
        {
            panel.Children.Add(newButton("Line", drawerButton_ClickEvent(new LineDrawer())));
            panel.Children.Add(newButton("Rect", drawerButton_ClickEvent(new RectangleDrawer())));
            panel.Children.Add(newButton("Triangle", drawerButton_ClickEvent(new TriangleDrawer())));
            panel.Children.Add(newButton("Ellipse", drawerButton_ClickEvent(new EllipseDrawer())));
            panel.Children.Add(newButton("Hand", handButton_Click));
            panel.Children.Add(newButton("Erase", eraseButton_Click));

            shapeModifierPanel = new ShapeModifierPanel();
            shapeModifierPanel.ValueChanged += shapeModifierPanel_ValueChanged;
            panel.Children.Add(shapeModifierPanel);

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
                IBoardObject[]? boardObjects = JsonConvert.DeserializeObject<IBoardObject[]>(text, serializerSettings);
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
        private void undoButton_Click(object current, RoutedEventArgs e)
        {
            vsBoard.Undo();
            vsBoard.InvalidateVisual();
        }
        private void redoButton_Click(object current, RoutedEventArgs e)
        {
            vsBoard.Redo();
            vsBoard.InvalidateVisual();
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
            selectedObject = obj;
            shapeModifierPanel.SetObject(obj);
        }

        private void shapeModifierPanel_ValueChanged(object? sender, EventArgs e)
        {
            vsBoard.SetDrawerAttributes(shapeModifierPanel.Thickness, shapeModifierPanel.Color);
            vsBoard.InvalidateVisual();
        }

        private HashSet<Key> keyDownSet = new HashSet<Key>();
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (!keyDownSet.Contains(e.Key))
            {
                keyDownSet.Add(e.Key);
            }
        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (keyDownSet.Contains(e.Key))
            {
                keyDownSet.Remove(e.Key);
            }

            if (e.Key == Key.C && keyDownSet.Contains(Key.LeftCtrl))
            {
                if (selectedObject != null)
                {
                    copiedObject = selectedObject.Clone();
                    copiedObject.Selected = false;
                }
            }
            else if (e.Key == Key.V && keyDownSet.Contains(Key.LeftCtrl))
            {
                if (copiedObject != null)
                {
                    copiedObject.Offset(new Point(10, 10));
                    vsBoard.AddObject(copiedObject.Clone());
                    vsBoard.InvalidateVisual();
                }
            }
        }
    }
}
