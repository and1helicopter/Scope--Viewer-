using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScopeViewer
{
    public class OscilGraph
    {
        //Заголовки осциллограммы
        public Label OscilName;
        private CheckBox _showAllCheckBox;
        private CheckBox _selectAllCheckBox;
        private Rectangle _closeButton ;
        public DockPanel LayoutOscilPanel;

        private readonly List<Border> _panelBorder = new List<Border>();
        public List<DockPanel> LayoutPanel = new List<DockPanel>();
        public List<Label> NameLabel = new List<Label>();
        public List<Ellipse> ColorEllipse = new List<Ellipse>();
        public List<CheckBox> VisibleCheckBox = new List<CheckBox>();
        public List<CheckBox> SelectCheckBox = new List<CheckBox>();

        public List<ComboBox> TypeTypeComboBox = new List<ComboBox>();
        public List<ComboBox> TypeComboBox = new List<ComboBox>();
        public List<CheckBox> SmoothCheckBox = new List<CheckBox>();
        
        public List<bool> OpenClose = new List<bool>();
        public List<ComboBox> StepTypeComboBox = new List<ComboBox>();
        public List<CheckBox> WidthCheckBox = new List<CheckBox>();

        readonly Random _rngColor = new Random();


        readonly string[] _typeType = new string[] {
            "Analog",
            "Digital"
        };

        readonly string[] _styleType = new string[] {
            "Solid",
            "Dash",
            "DashDot",
            "DashDotDot",
            "Dot"
        };

        readonly string[] _stepType = new string[] {
            "NonStep",
            "ForwardSegment",
            "ForwardStep",
            "RearwardSegment",
            "RearwardStep"
        };

        private Color GenerateColor(Random rng)
        {
            return Color.FromRgb(Convert.ToByte(rng.Next(0, 255)), Convert.ToByte(rng.Next(0, 255)), Convert.ToByte(rng.Next(0, 255)));
        }


        public void OscilConfigAdd(string oscilName)
        {
            LayoutOscilPanel = new DockPanel
            {
                Width = 210,
                Height = 30,
                Margin = new Thickness(-20, 10, 0, 0),
                Background = Brushes.WhiteSmoke
            };

            OscilName = new Label
            {
                Content = "Осциллограмма №" + (MainWindow.OscilChannelList.Count + 1),
                ToolTip = oscilName,
                Width = 150,
                Margin = new Thickness(0, 3, 0, 0)
            };

            _selectAllCheckBox = new CheckBox
            {
                ToolTip = "Выбрать все каналы",
                Margin = new Thickness(0, 8, 0, 0)
            };
            //  _selectAllCheckBox[LayoutOscilPanel.Count - 1].Checked += Graph_Checked;
            //  _selectAllCheckBox[LayoutOscilPanel.Count - 1].Unchecked += Graph_Checked;

            _showAllCheckBox = new CheckBox
            {
                ToolTip = "Отображать все каналы",
                Margin = new Thickness(0, 8, 0, 0),
                IsChecked = true
            };
            //    _showAllCheckBox[LayoutOscilPanel.Count - 1].Checked += Graph_Checked1;
            //  _showAllCheckBox[LayoutOscilPanel.Count - 1].Unchecked += Graph_Checked1;

            _closeButton = new Rectangle
            {
                ToolTip = "Закрыть осциллограмму",
                Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close Window-48(1).png"))),
                Margin = new Thickness(0, 0, 0, 0),
                Width = 16,
                Height = 16
            };
            _closeButton.MouseEnter += Graph_MouseEnter;
            _closeButton.MouseLeave += Graph_MouseLeave;
            _closeButton.MouseDown += Graph_MouseDown;

            LayoutOscilPanel.Children.Add(OscilName);
            LayoutOscilPanel.Children.Add(_showAllCheckBox);
            LayoutOscilPanel.Children.Add(_selectAllCheckBox);
            LayoutOscilPanel.Children.Add(_closeButton);
        }

        public void GraphConfigAdd(string nameChannel, string dimensionChannel, bool typeChannel)
        {
            Border panelBorder = new Border
            {
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(1.0),
                Margin = new Thickness(-200, 0, 0, 0)
            };
            _panelBorder.Add(panelBorder);

            DockPanel layoutPanel = new DockPanel
            {
                Width = 200,
                Height = 25,
                Margin = new Thickness(-10, 2, 1, 0),
                Background = Brushes.White,
            };
            LayoutPanel.Add(layoutPanel);
            LayoutPanel[LayoutPanel.Count - 1].MouseDown += click_LayoutPanel;

            Label nameLabel = new Label
            {
                Content = nameChannel,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 12,
                Height = 25,
                Width = 140,
                ToolTip = "Название канала:\n" + nameChannel + ", " + dimensionChannel,
                Margin = new Thickness(0, 0, 0, 0)
            };
            NameLabel.Add(nameLabel);

            Ellipse colorEllipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(3, 2, 0, 0),
                ToolTip = "Цвет",
                Fill = new SolidColorBrush(GenerateColor(_rngColor)),
            };
            ColorEllipse.Add(colorEllipse);
            ColorEllipse[ColorEllipse.Count - 1].MouseDown += click_ColorEllipse;

            CheckBox visibleCheckBox = new CheckBox
            {
                IsChecked = true,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 16,
                Width = 16,
                Margin = new Thickness(0, 5, 0, 0),
                ToolTip = "Отображать"
            };
            VisibleCheckBox.Add(visibleCheckBox);
            VisibleCheckBox[VisibleCheckBox.Count - 1].Click += click_checkedButton;

            CheckBox selectCheckBox = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                Height = 16,
                Width = 16,
                Margin = new Thickness(0, 5, 0, 0),
                ToolTip = "Выбрать"
            };
            SelectCheckBox.Add(selectCheckBox);

            ComboBox typeTypeComboBox = new ComboBox
            {
                Width = 100,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(-270, 25, 0, 0),
                ToolTip = "Тип канала",
                ItemsSource = _typeType,
                SelectedIndex = typeChannel ? 1 : 0
            };
            TypeTypeComboBox.Add(typeTypeComboBox);
            TypeTypeComboBox[TypeTypeComboBox.Count - 1].SelectionChanged += TypeChange;

            ComboBox typeComboBox = new ComboBox
            {
                Width = 100,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(-270, 50, 0, 0),
                ToolTip = "Тип линии",
                ItemsSource = _styleType,
                SelectedIndex = 0
            };
            TypeComboBox.Add(typeComboBox);
            TypeComboBox[TypeComboBox.Count - 1].SelectionChanged += Change_index;

            ComboBox stepTypeComboBox = new ComboBox
            {
                Width = 100,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(-270, 75, 0, 0),
                ToolTip = "Ступенчатость",
                ItemsSource = _stepType,
                SelectedIndex = 0
            };
            StepTypeComboBox.Add(stepTypeComboBox);
            StepTypeComboBox[StepTypeComboBox.Count - 1].SelectionChanged += Change_index;

            CheckBox smoothCheckBox = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                Height = 16,
                Width = 16,
                Margin = new Thickness(-50, 53, 0, 0),
                ToolTip = "Сглаживание"
            };
            SmoothCheckBox.Add(smoothCheckBox);
            SmoothCheckBox[SmoothCheckBox.Count - 1].Click += click_checkedButton;

            CheckBox widthCheckBox = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                Height = 16,
                Width = 16,
                Margin = new Thickness(-50, 78, 0, 0),
                ToolTip = "Толщина"
            };
            WidthCheckBox.Add(widthCheckBox);
            WidthCheckBox[WidthCheckBox.Count - 1].Click += click_checkedButton;

            OpenClose.Add(new bool() );

            LayoutPanel[LayoutPanel.Count - 1].Children.Add(NameLabel[NameLabel.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(VisibleCheckBox[VisibleCheckBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(SelectCheckBox[SelectCheckBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(ColorEllipse[ColorEllipse.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(TypeTypeComboBox[TypeTypeComboBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(TypeComboBox[TypeComboBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(SmoothCheckBox[SmoothCheckBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(StepTypeComboBox[StepTypeComboBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(WidthCheckBox[WidthCheckBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(_panelBorder[_panelBorder.Count - 1]);
        }

        private void click_LayoutPanel(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < OpenClose.Count; i++)
            {
                if (LayoutPanel[i].IsMouseOver && OpenClose[i] == false)
                {
                    LayoutPanel[i].Height = 100;
                    OpenClose[i] = true;
                }
                else if (LayoutPanel[i].IsMouseOver && OpenClose[i])
                {
                    LayoutPanel[i].Height = 25;
                    OpenClose[i] = false;
                }
            }
        }

        private void click_ColorEllipse(object sender, EventArgs e)
        {
            //Нужно определить в каком графике произошел вызов 
            int j = 0, l = 0;
            for (int k = 0; k < MainWindow.OscilChannelList.Count; k++)
            {
                for (int i = 0; i < MainWindow.OscilChannelList[k].ColorEllipse.Count; i++)
                {
                    if (MainWindow.OscilChannelList[k].ColorEllipse[i].IsMouseOver)
                    {
                        j = i;
                        l = k;
                        break;
                    }
                }
            }

            var dialog = new System.Windows.Forms.ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var colorColorEllipse = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
                var brushColorEllipse = new SolidColorBrush(colorColorEllipse);
                MainWindow.OscilChannelList[l].ColorEllipse[j].Fill = brushColorEllipse;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(brushColorEllipse.Color.A, brushColorEllipse.Color.R, brushColorEllipse.Color.G, brushColorEllipse.Color.B);
                ChangeSomething(j, TypeComboBox[j].SelectedIndex, StepTypeComboBox[j].SelectedIndex, color, l);
            }
        }

        private void click_checkedButton(object sender, EventArgs e)
        {
            //Нужно определить в каком графике произошел вызов 
            int j = 0, l = 0;
            for (int k = 0; k < MainWindow.OscilChannelList.Count; k++)
            {
                for (int i = 0; i < MainWindow.OscilChannelList[k].VisibleCheckBox.Count; i++)
                {
                    if (MainWindow.OscilChannelList[k].VisibleCheckBox[i].IsMouseOver || MainWindow.OscilChannelList[k].SmoothCheckBox[i].IsMouseOver || MainWindow.OscilChannelList[k].WidthCheckBox[i].IsMouseOver)
                    {
                        j = i;
                        l = k;
                        break;
                    }
                }
            }

            ChangeSomething(j, TypeComboBox[j].SelectedIndex, StepTypeComboBox[j].SelectedIndex, MainWindow.GraphPanelList[l].Pane.CurveList[j].Color, l);
        }

        private void TypeChange(object sender, SelectionChangedEventArgs e)
        {
            //Нужно определить в каком графике произошел вызов 
            int j = 0, l = 0;
            for (int k = 0; k < MainWindow.OscilChannelList.Count; k++)
            {
                for (int i = 0; i < MainWindow.OscilChannelList[k].TypeTypeComboBox.Count; i++)
                {
                    if (MainWindow.OscilChannelList[k].TypeTypeComboBox[i].IsMouseOver)
                    {
                        j = i;
                        l = k;
                        break;
                    }
                }
            }

            MainWindow.GraphPanelList[l].ChangeDigitalList(j, l, TypeTypeComboBox[j].SelectedIndex);
        }

        private void Change_index(object sender, SelectionChangedEventArgs e)
        {
            //Нужно определить в каком графике произошел вызов 
            int j = 0, l = 0;
            for (int k = 0; k < MainWindow.OscilChannelList.Count; k++)
            {
                for (int i = 0; i < MainWindow.OscilChannelList[k].TypeComboBox.Count; i++)
                {
                    if (MainWindow.OscilChannelList[k].TypeComboBox[i].IsMouseOver || MainWindow.OscilChannelList[k].StepTypeComboBox[i].IsMouseOver)
                    {
                        j = i;
                        l = k;
                        break;
                    }
                }
            }
            ChangeSomething(j, TypeComboBox[j].SelectedIndex, StepTypeComboBox[j].SelectedIndex, MainWindow.GraphPanelList[l].Pane.CurveList[j].Color, l);
        }

        private void ChangeSomething(int num, int line, int typeStep, System.Drawing.Color color, int numGraphPane)
        {
            bool show = true, smooth = false, width = false;
            if (VisibleCheckBox[num].IsChecked == false) show = false;
            if (SmoothCheckBox[num].IsChecked == true) smooth = true;
            if (WidthCheckBox[num].IsChecked == true) width = true;
            MainWindow.GraphPanelList[numGraphPane].ChangeLine(num, line, typeStep, width, show, smooth, color);
        }

        private void Graph_MouseDown(object sender, MouseEventArgs e)
        {
            int l = 0;
            for (int k = 0; k < MainWindow.OscilChannelList.Count; k++)
            {
                if (MainWindow.OscilChannelList[k]._closeButton.IsMouseOver)
                {
                    l = k;
                    break;
                }
            }

            MainWindow.DelateOscil(l);
        }

        private void Graph_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close Window-48(1).png")));
        }

        private void Graph_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close Window-48.png")));
        }
    }
}
