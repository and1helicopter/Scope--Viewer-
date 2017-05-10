using System;
using System.Collections.Generic;
using System.Linq;
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
        public readonly List<DockPanel> LayoutPanel = new List<DockPanel>();
        public readonly List<Label> NameLabel = new List<Label>();
        public readonly List<Ellipse> ColorEllipse = new List<Ellipse>();
        public readonly List<CheckBox> VisibleCheckBox = new List<CheckBox>();
        public readonly List<CheckBox> SelectCheckBox = new List<CheckBox>();

        public readonly List<ComboBox> TypeTypeComboBox = new List<ComboBox>();
        public readonly List<ComboBox> TypeComboBox = new List<ComboBox>();
        public readonly List<CheckBox> SmoothCheckBox = new List<CheckBox>();

        public readonly List<TextBox> ScaleTextBox = new List<TextBox>();
        private readonly List<double> _scale = new List<double>();

        public readonly List<TextBox> ShiftTextBox = new List<TextBox>();
        private readonly List<double> _shift = new List<double>();

        public readonly List<bool> OpenClose = new List<bool>();
        public readonly List<ComboBox> StepTypeComboBox = new List<ComboBox>();
        public readonly List<CheckBox> WidthCheckBox = new List<CheckBox>();

        private readonly Random _rngColor = new Random();


        private readonly string[] _typeType = {
            "Analog",
            "Digital"
        };

        private readonly string[] _styleType = {
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
            _selectAllCheckBox.Click += SelectAllCheckBox_Click;

            _showAllCheckBox = new CheckBox
            {
                ToolTip = "Отображать все каналы",
                Margin = new Thickness(0, 8, 0, 0),
                IsChecked = true
            };
            _showAllCheckBox.Click += ShowAllCheckBox_Click;

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

        private void ShowAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (_showAllCheckBox.IsChecked == true)
            {
                foreach (var checkBox in VisibleCheckBox)
                {
                    checkBox.IsChecked = true;
                }
            }
            else
            {
                foreach (var checkBox in VisibleCheckBox)
                {
                    checkBox.IsChecked = false;
                }
            }
            for (int i = 0; i < SelectCheckBox.Count; i++)
            {
                click_graph(i);
            }
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (_selectAllCheckBox.IsChecked == true)
            {
                foreach (var checkBox in SelectCheckBox)
                {
                    checkBox.IsChecked = true;
                }
            }
            else
            {
                foreach (var checkBox in SelectCheckBox)
                {
                    checkBox.IsChecked = false;
                }
            }
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
            nameLabel.Foreground = typeChannel ? new SolidColorBrush(Color.FromRgb(255, 50, 50)) : new SolidColorBrush(Color.FromRgb(0, 0, 0));
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
            visibleCheckBox.Checked += VisibleCheckBox_Checked;
            visibleCheckBox.Unchecked += VisibleCheckBox_Unchecked;
            visibleCheckBox.Click += click_checkedButton;
            VisibleCheckBox.Add(visibleCheckBox);

            CheckBox selectCheckBox = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                Height = 16,
                Width = 16,
                Margin = new Thickness(0, 5, 0, 0),
                ToolTip = "Выбрать"
            };
            selectCheckBox.Checked += selectCheckBox_Checked;
            selectCheckBox.Unchecked += selectCheckBox_Unchecked;
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

            TextBox saleTextBox = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                Width = 83,
                Margin = new Thickness(-287, 100, 0, 0),
                TextAlignment = TextAlignment.Right,
                Text = "1,00",
                ToolTip = "Масштабирование"
            };
            ScaleTextBox.Add(saleTextBox);
            _scale.Add(1.00);
            ScaleTextBox[ScaleTextBox.Count - 1].Name = "scale" + Convert.ToString(ScaleTextBox.Count - 1);
            ScaleTextBox[ScaleTextBox.Count - 1].KeyUp += OnKeyUp;
            //    WidthCheckBox[WidthCheckBox.Count - 1].Click += click_checkedButton;

            TextBox shiftextBox = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                Width = 83,
                Margin = new Thickness(-117, 100, 0, 0),
                TextAlignment = TextAlignment.Right,
                Text = "0,00",
                ToolTip = "Сдвиг"
            };
            ShiftTextBox.Add(shiftextBox);
            _shift.Add(0.00);
            ShiftTextBox[ShiftTextBox.Count - 1].Name = "shift"  + Convert.ToString(ShiftTextBox.Count - 1);
            ShiftTextBox[ShiftTextBox.Count - 1].KeyUp += OnKeyUp;

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
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(ScaleTextBox[ScaleTextBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(ShiftTextBox[ShiftTextBox.Count - 1]);
            LayoutPanel[LayoutPanel.Count - 1].Children.Add(_panelBorder[_panelBorder.Count - 1]);
        }

        private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            if (((TextBox) sender).ToolTip == "Масштабирование")
            {
                try
                {
                    double temp = Convert.ToDouble(((TextBox)sender).Text.Replace('.', ','));
                    if (temp > 0)
                    {
                        ((TextBox) sender).Text = temp.ToString("F2");
                        _scale[Convert.ToInt32(((TextBox) sender).Name.Replace("scale", ""))] = temp;
                    }
                    else if (temp == 0)
                    {
                        ((TextBox)sender).Text = "0,01";
                        _scale[Convert.ToInt32(((TextBox)sender).Name.Replace("scale", ""))] = 0.01;
                    }
                    else
                    {
                        ((TextBox)sender).Text = _scale[Convert.ToInt32(((TextBox)sender).Name.Replace("scale", ""))].ToString("F2");
                    }
                    //Обработчик 
                    //Convert.ToDouble(((TextBox)sender).Text);
                }
                catch
                {
                    ((TextBox)sender).Text = _scale[Convert.ToInt32(((TextBox)sender).Name.Replace("scale", ""))].ToString("F2");
                    //Обработчик 
                    //Convert.ToDouble(((TextBox) sender).Text);
                }
            }
            else if (((TextBox)sender).ToolTip == "Сдвиг")
            {
                try
                {
                    double temp = Convert.ToDouble(((TextBox)sender).Text.Replace('.', ','));
                    if (temp > 0)
                    {
                        ((TextBox)sender).Text = temp.ToString("F2");
                        _shift[Convert.ToInt32(((TextBox)sender).Name.Replace("shift", ""))] = temp;
                    }
                    else if (temp == 0)
                    {
                        ((TextBox)sender).Text = "0,00";
                        _shift[Convert.ToInt32(((TextBox)sender).Name.Replace("shift", ""))] = 0.00;
                    }
                    else
                    {
                        ((TextBox)sender).Text = _shift[Convert.ToInt32(((TextBox)sender).Name.Replace("shift", ""))].ToString("F2");
                    }
                    //Обработчик 
                    //Convert.ToDouble(((TextBox)sender).Text);
                }
                catch
                {
                    ((TextBox)sender).Text = _shift[Convert.ToInt32(((TextBox)sender).Name.Replace("shift", ""))].ToString("F2");
                    //Обработчик 
                    //Convert.ToDouble(((TextBox) sender).Text);
                }
            }
        }
        
        private void VisibleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _showAllCheckBox.IsChecked = false;
        }

        private void VisibleCheckBox_Checked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (VisibleCheckBox.Count == VisibleCheckBox.Count(checkBox => checkBox.IsChecked == true))
            {
                _showAllCheckBox.IsChecked = true;
            }
        }

        private void selectCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
               _selectAllCheckBox.IsChecked = false;
        }

        private void selectCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectCheckBox.Count == SelectCheckBox.Count(checkBox => checkBox.IsChecked == true))
            {
                _selectAllCheckBox.IsChecked = true;
            }
        }

        private void click_LayoutPanel(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < OpenClose.Count; i++)
            {
                if (LayoutPanel[i].IsMouseOver && OpenClose[i] == false)
                {
                    LayoutPanel[i].Height = 125;
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

        private void click_graph(int j)
        {
            for (int k = 0; k < MainWindow.OscilChannelList.Count; k++)
            {
                if(MainWindow.OscilChannelList[k] == this)
                {
                    var l = k;
                    ChangeSomething(j, TypeComboBox[j].SelectedIndex, StepTypeComboBox[j].SelectedIndex, MainWindow.GraphPanelList[l].Pane.CurveList[j].Color, l);
                }
            }
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

            NameLabel[j].Foreground = TypeTypeComboBox[j].Text == "Digital" ? new SolidColorBrush(Color.FromRgb(0, 0, 0)): new SolidColorBrush(Color.FromRgb(255, 50, 50));
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
