using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using VsWpf.BoardObject;

namespace VsWpf
{
    class ShapeModifierPanel : StackPanel
    {
        private Label thicknessLabel;
        private TextBox thicknessText;
        private ScrollBar scrollBar;
        private Label colorLabel;
        private Label colorPanel;

        private BoardShape? selectedShape;
        public event EventHandler? ValueChanged;

        public double Thickness { get { return Math.Round(scrollBar.Value); } }
        public Color Color { get { return getColor(); } }

        public ShapeModifierPanel()
        {
            thicknessLabel = new Label
            {
                FontSize = 10,
                Content = "Thickness"
            };
            Children.Add(thicknessLabel);

            thicknessText = new TextBox
            {
                IsReadOnly = true,
                Text = "1"
            };
            Children.Add(thicknessText);

            scrollBar = new ScrollBar
            {
                Orientation = Orientation.Horizontal,
                SmallChange = 1,
                Minimum = 0,
                Maximum = 15,
                Value = 1
            };
            scrollBar.ValueChanged += scrollBar_ValueChanged;
            Children.Add(scrollBar);

            colorLabel = new Label
            {
                FontSize = 10,
                Content = "Color"
            };
            Children.Add(colorLabel);

            colorPanel = new Label
            {
                Height = 20,
                Background = Brushes.Red
            };
            colorPanel.MouseDoubleClick += colorPanel_MouseDoubleClick;
            Children.Add(colorPanel);
        }

        public void SetObject(IBoardObject? boardObject)
        {
            BoardShape? shape = boardObject as BoardShape;
            if (shape == null)
            {
                selectedShape = null;
                return;
            }
            selectedShape = shape;

            scrollBar.Value = shape.Thickness;
            colorPanel.Background = new SolidColorBrush(shape.Color);
        }

        private void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            thicknessText.Text = scrollBar.Value.ToString("0");

            if (selectedShape != null)
            {
                selectedShape.Thickness = Math.Round(scrollBar.Value);
            }
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void colorPanel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ColorWindow cw = new ColorWindow
            {
                WindowStyle = WindowStyle.SingleBorderWindow,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this)
            };
            Color color = getColor();
            cw.R = color.R;
            cw.G = color.G;
            cw.B = color.B;
            cw.ShowDialog();

            color = Color.FromArgb(255, cw.R, cw.G, cw.B);
            colorPanel.Background = new SolidColorBrush(color);

            if (selectedShape != null)
            {
                selectedShape.Color = color;
            }
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private Color getColor()
        {
            SolidColorBrush? brush = colorPanel.Background as SolidColorBrush;
            if (brush != null)
            {
                return brush.Color;
            }
            return Colors.Red;
        }
    }

    class ColorWindow : Window
    {
        private Label colorPanel = new Label();

        private NumericTextBox rTextBox;
        private NumericTextBox gTextBox;
        private NumericTextBox bTextBox;

        public byte R
        {
            get { return (byte)rTextBox.Value; }
            set { rTextBox.Value = value; }
        }
        public byte G
        {
            get { return (byte)gTextBox.Value; }
            set { gTextBox.Value = value; }
        }
        public byte B
        {
            get { return (byte)bTextBox.Value; }
            set { bTextBox.Value = value; }
        }

        public ColorWindow()
        {
            Height = 66;
            Width = 300;

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            AddChild(grid);

            grid.Children.Add(newLabel("R", 0));
            rTextBox = newTextBox(1);
            grid.Children.Add(rTextBox);
            grid.Children.Add(newLabel("G", 2));
            gTextBox = newTextBox(3);
            grid.Children.Add(gTextBox);
            grid.Children.Add(newLabel("B", 4));
            bTextBox = newTextBox(5);
            grid.Children.Add(bTextBox);

            colorPanel = new Label();
            Grid.SetColumn(colorPanel, 6);
            grid.Children.Add(colorPanel);

            Button button = new Button();
            button.Content = "OK";
            button.Click += button_Click;
            Grid.SetColumn(button, 7);
            grid.Children.Add(button);
        }

        private Label newLabel(string content, int col)
        {
            Label label = new Label();
            label.Content = content;
            Grid.SetColumn(label, col);

            return label;
        }
        private NumericTextBox newTextBox(int col)
        {
            NumericTextBox textBox = new NumericTextBox();
            textBox.ValueChanged += textBox_ValueChanged;
            Grid.SetColumn(textBox, col);
            return textBox;
        }

        private void textBox_ValueChanged(object? sender, EventArgs e)
        {
            colorPanel.Background = new SolidColorBrush(Color.FromRgb((byte)rTextBox.Value, (byte)gTextBox.Value, (byte)bTextBox.Value));
        }

        private void button_Click(object sender, EventArgs e)
        {
            Close();
        }

        class NumericTextBox : TextBox
        {
            private bool changing = false;

            private int innerValue;
            public int Value
            {
                get
                {
                    return innerValue; 
                }
                set
                {
                    // TODO
                    if (value < 0)
                    {
                        value = 0;
                    }
                    else if (value > 255)
                    {
                        value = 255;
                    }

                    innerValue = value;
                    Text = value.ToString();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            public event EventHandler? ValueChanged;

            public NumericTextBox()
            {
                innerValue = 0;
                Text = innerValue.ToString();
                TextChanged += textChanged;
            }

            private void textChanged(object sender, TextChangedEventArgs e)
            {
                if (changing)
                {
                    return;
                }

                int value;
                if (int.TryParse(Text, out value))
                {
                    changing = true;
                    Value = value;
                    changing = false;
                }
            }
        }
    }
}