using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScopeViewer
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph 
    {
        //Заголовки осциллограммы
        readonly List<Label> _oscilName = new List<Label>();
        readonly List<CheckBox> _showAllCheckBox = new List<CheckBox>();
        readonly List<CheckBox> _selectAllCheckBox = new List<CheckBox>();
        readonly List<Rectangle> _closeButton = new List<Rectangle>();
        readonly List<DockPanel> _layoutOscilPanel = new List<DockPanel>();
        readonly List <List<int>> _clearChannel = new List <List<int>>();

        readonly List<DockPanel> _layoutPanel = new List<DockPanel>();
        readonly List<Label> _nameLabel = new List<Label>();
        readonly List<Ellipse> _colorEllipse = new List<Ellipse>();
        readonly List<CheckBox> _visibleCheckBox = new List<CheckBox>();
        readonly List<CheckBox> _selectCheckBox = new List<CheckBox>();

        readonly List<ComboBox> _typeTypeComboBox = new List<ComboBox>();
        readonly List<ComboBox> _typeComboBox = new List<ComboBox>();
        readonly List<CheckBox> _smoothCheckBox = new List<CheckBox>();
        readonly List<Border> _panelBorder = new List<Border>();
        readonly List<bool> _openClose = new List<bool>();
        readonly List<ComboBox> _stepTypeComboBox = new List<ComboBox>();
        readonly List<CheckBox> _widthCheckBox = new List<CheckBox>();

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

        public Graph()
        {
            InitializeComponent();
        }

        public void OscilConfigAdd(string oscilName)
        {
            _layoutOscilPanel.Add(new DockPanel());
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Width = 210;
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Height = 30;
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Margin = new Thickness(-20, 10, 0, 0);
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Background = Brushes.WhiteSmoke;

            _oscilName.Add(new Label());
            _oscilName[_layoutOscilPanel.Count - 1].Content = "Осциллограмма №" + _layoutOscilPanel.Count;
            _oscilName[_layoutOscilPanel.Count - 1].ToolTip = oscilName;
            _oscilName[_layoutOscilPanel.Count - 1].Width = 150;
            _oscilName[_layoutOscilPanel.Count - 1].Margin = new Thickness(0, 3, 0, 0);

            _selectAllCheckBox.Add(new CheckBox());
            _selectAllCheckBox[_layoutOscilPanel.Count - 1].ToolTip = "Выбрать все каналы";
            _selectAllCheckBox[_layoutOscilPanel.Count - 1].Margin = new Thickness(0, 8, 0, 0);
            _selectAllCheckBox[_layoutOscilPanel.Count - 1].Checked += Graph_Checked;
            _selectAllCheckBox[_layoutOscilPanel.Count - 1].Unchecked += Graph_Checked;

            _showAllCheckBox.Add(new CheckBox());
            _showAllCheckBox[_layoutOscilPanel.Count - 1].ToolTip = "Отображать все каналы";
            _showAllCheckBox[_layoutOscilPanel.Count - 1].Margin = new Thickness(0, 8, 0, 0);
            _showAllCheckBox[_layoutOscilPanel.Count - 1].IsChecked = true;
            _showAllCheckBox[_layoutOscilPanel.Count - 1].Checked += Graph_Checked1;
            _showAllCheckBox[_layoutOscilPanel.Count - 1].Unchecked += Graph_Checked1;

            _closeButton.Add(new Rectangle());
            _closeButton[_layoutOscilPanel.Count - 1].ToolTip = "Закрыть осциллограмму";
            _closeButton[_layoutOscilPanel.Count - 1].Width = _closeButton[_layoutOscilPanel.Count - 1].Height = 16;
            _closeButton[_layoutOscilPanel.Count - 1].Fill  = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close Window-48(1).png")));
            _closeButton[_layoutOscilPanel.Count - 1].Margin = new Thickness(0, 0, 0, 0);
            _closeButton[_layoutOscilPanel.Count - 1].Tag = _layoutOscilPanel.Count - 1;
            _closeButton[_layoutOscilPanel.Count - 1].MouseEnter += Graph_MouseEnter;
            _closeButton[_layoutOscilPanel.Count - 1].MouseLeave += Graph_MouseLeave;
            _closeButton[_layoutOscilPanel.Count - 1].MouseDown += Graph_MouseDown;
         
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Children.Add(_oscilName[_layoutOscilPanel.Count - 1]);
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Children.Add(_showAllCheckBox[_layoutOscilPanel.Count - 1]);
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Children.Add(_selectAllCheckBox[_layoutOscilPanel.Count - 1]);
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Children.Add(_closeButton[_layoutOscilPanel.Count - 1]);

            _clearChannel.Add(new List<int>());

            GraphStackPanel.Children.Add(_layoutOscilPanel[_layoutOscilPanel.Count - 1]);
        }

        private void Graph_Checked1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _showAllCheckBox.Count; i++)
            {
                if (_showAllCheckBox[i].IsChecked == true)
                {
                    for (int j = 0; j < _clearChannel[i].Count; j++)
                    {
                        int k = _clearChannel[i][j];
                        _visibleCheckBox[k].IsChecked = true;
                        ChangeSomething(k, _typeComboBox[k].SelectedIndex, _stepTypeComboBox[k].SelectedIndex, GraphPanel.Pane.CurveList[k].Color);
                    }
                }
                else
                {
                    for (int j = 0; j < _clearChannel[i].Count; j++)
                    {
                        int k = _clearChannel[i][j];
                        _visibleCheckBox[_clearChannel[i][j]].IsChecked = false;
                        ChangeSomething(k, _typeComboBox[k].SelectedIndex, _stepTypeComboBox[k].SelectedIndex, GraphPanel.Pane.CurveList[k].Color);
                    }
                }
            }
        }

        private void Graph_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _selectAllCheckBox.Count; i++)
            {
                if (_selectAllCheckBox[i].IsChecked == true)
                {
                    for (int j = 0; j < _clearChannel[i].Count; j++)
                    {
                        _selectCheckBox[_clearChannel[i][j]].IsChecked = true;
                    }
                }
                else
                {
                    for (int j = 0; j < _clearChannel[i].Count; j++)
                    {
                        _selectCheckBox[_clearChannel[i][j]].IsChecked = false;
                    }
                }
            }
        }

        private void Graph_MouseDown(object sender, MouseEventArgs e)
        {
            int i  = Convert.ToInt32(((Rectangle)sender).Tag);
            int count = _clearChannel[i].Count;
            for (int j = _clearChannel[i][_clearChannel[i].Count - 1]; j >= _clearChannel[i][0]; j--)
            {
                //Удаляем панели 
                GraphConfigClear(j);
                //Удаляем графики
                MainWindow.Graph.RemoveGraph(j);
            }
            //Удаляем заголовок
            OscilConfigClear(i);
            _clearChannel[i].Clear();
            _clearChannel.Remove(_clearChannel[i]);
            //Удаляем экземляр объекта осциллограммы 
            MainWindow.OscilList.Remove(MainWindow.OscilList[i]);
            for (int k = _layoutOscilPanel.Count - 1; k >= i; k--)
            {
                for (int j = _clearChannel[k].Count - 1; j >= 0; j--)
                {
                    _clearChannel[k][j] -= count;
                }
                _closeButton[k].Tag = (int)_closeButton[k].Tag - 1;
                _oscilName[k].Content = "Осциллограмма №" + ((int)_closeButton[k].Tag + 1);
            }
        }
        private void Graph_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close Window-48(1).png")));
        }

        private void Graph_MouseEnter(object sender, MouseEventArgs e)
        {
          ((Rectangle)sender).Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close Window-48.png")));
        }

        private void OscilConfigClear(int i)
        {
            _layoutOscilPanel[i].Children.Remove(_oscilName[i]);
            _layoutOscilPanel[i].Children.Remove(_showAllCheckBox[i]);
            _layoutOscilPanel[i].Children.Remove(_selectAllCheckBox[i]);
            _layoutOscilPanel[i].Children.Remove(_closeButton[i]);

            GraphStackPanel.Children.Remove(_layoutOscilPanel[i]);

            _oscilName.Remove(_oscilName[i]);
            _showAllCheckBox.Remove(_showAllCheckBox[i]);
            _selectAllCheckBox.Remove(_selectAllCheckBox[i]);
            _closeButton.Remove(_closeButton[i]);
            _layoutOscilPanel.Remove(_layoutOscilPanel[i]);

            MainWindow.Graph.StampTriggerClear();
            MainWindow.StampTriggerCreate = false;

            MainWindow.Graph.CursorClear();
            MainWindow.AnalysisObj.AnalysisCursorClear();
            MainWindow.CursorCreate = false;

            MainWindow.Graph.ResizeAxis();
        }

        private void GraphConfigClear(int i)
        {

            _layoutPanel[i].Children.Remove(_nameLabel[i]);
            _layoutPanel[i].Children.Remove(_colorEllipse[i]);
            _layoutPanel[i].Children.Remove(_visibleCheckBox[i]);
            _layoutPanel[i].Children.Remove(_selectCheckBox[i]);
            _layoutPanel[i].Children.Remove(_typeTypeComboBox[i]);
            _layoutPanel[i].Children.Remove(_typeComboBox[i]);
            _layoutPanel[i].Children.Remove(_smoothCheckBox[i]);
            _layoutPanel[i].Children.Remove(_stepTypeComboBox[i]);
            _layoutPanel[i].Children.Remove(_panelBorder[i]);
            _layoutPanel[i].Children.Remove(_widthCheckBox[i]);

            GraphStackPanel.Children.Remove(_layoutPanel[i]);

            _openClose.Remove(_openClose[i]);

            _nameLabel.Remove(_nameLabel[i]);
            _colorEllipse.Remove(_colorEllipse[i]);
            _visibleCheckBox.Remove(_visibleCheckBox[i]);
            _selectCheckBox.Remove(_selectCheckBox[i]);
            _typeTypeComboBox.Remove(_typeTypeComboBox[i]);
            _typeComboBox.Remove(_typeComboBox[i]);
            _smoothCheckBox.Remove(_smoothCheckBox[i]);
            _stepTypeComboBox.Remove(_stepTypeComboBox[i]);
            _widthCheckBox.Remove(_widthCheckBox[i]);
            _panelBorder.Remove(_panelBorder[i]);
            _layoutPanel.Remove(_layoutPanel[i]);
        }

        public void GraphConfigAdd(string nameChannel, string dimensionChannel, int oscilNum, bool typeChannel)
        {
            _panelBorder.Add(new Border());
            int i = _panelBorder.Count - 1;
            _panelBorder[i].BorderBrush = Brushes.DarkGray;
            _panelBorder[i].BorderThickness = new Thickness(1.0);
            _panelBorder[i].Margin = new Thickness(-200, 0, 0, 0);

            _layoutPanel.Add(new DockPanel());
            _layoutPanel[i].Width = 200;
            _layoutPanel[i].Height = 25;
            _layoutPanel[i].Margin = new Thickness(-10, 2, 1, 0);
            _layoutPanel[i].Background = Brushes.White;
            _layoutPanel[i].MouseDown += click_LayoutPanel;

            _nameLabel.Add(new Label());
            _nameLabel[i].Content = nameChannel;
            _nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            _nameLabel[i].FontSize = 12;
            _nameLabel[i].Height = 25;
            _nameLabel[i].Width = 140;
            _nameLabel[i].ToolTip = "Название канала:\n" + nameChannel + ", " + dimensionChannel; 
            _nameLabel[i].Margin = new Thickness(0,0,0,0);

            _colorEllipse.Add(new Ellipse());
            _colorEllipse[i].Width = _colorEllipse[i].Height = 20;
            _colorEllipse[i].VerticalAlignment = VerticalAlignment.Top;
            _colorEllipse[i].Margin = new Thickness(0, 2, 0, 0);
            _colorEllipse[i].Fill = new SolidColorBrush(Color.FromArgb(GraphPanel.Pane.CurveList[i].Color.A, GraphPanel.Pane.CurveList[i].Color.R, GraphPanel.Pane.CurveList[i].Color.G, GraphPanel.Pane.CurveList[i].Color.B));
            _colorEllipse[i].ToolTip = "Цвет";
            _colorEllipse[i].MouseDown += (click_ColorEllipse);

            _visibleCheckBox.Add(new CheckBox());
            _visibleCheckBox[i].IsChecked = true;
            _visibleCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            _visibleCheckBox[i].Height = _visibleCheckBox[i].Width = 16;
            _visibleCheckBox[i].Margin = new Thickness(0, 5, 0, 0);
            _visibleCheckBox[i].ToolTip = "Отображать";
            _visibleCheckBox[i].Click += click_checkedButton;

            _selectCheckBox.Add(new CheckBox());
            _selectCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            _selectCheckBox[i].Height = _visibleCheckBox[i].Width = 16;
            _selectCheckBox[i].Margin = new Thickness(0, 5, 0, 0);
            _selectCheckBox[i].ToolTip = "Выбрать";

            _typeTypeComboBox.Add(new ComboBox());
            _typeTypeComboBox[i].Width = 100;
            _typeTypeComboBox[i].VerticalAlignment = VerticalAlignment.Top;
            _typeTypeComboBox[i].Margin = new Thickness(-270, 25, 0, 0);
            _typeTypeComboBox[i].ToolTip = "Тип канала";
            _typeTypeComboBox[i].ItemsSource = _typeType;
            _typeTypeComboBox[i].SelectedIndex = typeChannel ? 1 : 0;
            _typeTypeComboBox[i].SelectionChanged += TypeChange;
            
            _typeComboBox.Add(new ComboBox());
            _typeComboBox[i].Width = 100;
            _typeComboBox[i].VerticalAlignment = VerticalAlignment.Top;
            _typeComboBox[i].Margin = new Thickness(-270, 50, 0, 0);
            _typeComboBox[i].ToolTip = "Тип линии";
            _typeComboBox[i].ItemsSource = _styleType;
            _typeComboBox[i].SelectedIndex = 0;
            _typeComboBox[i].SelectionChanged +=(Change_index);
            
            _smoothCheckBox.Add(new CheckBox());
            _smoothCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            _smoothCheckBox[i].Height = _visibleCheckBox[i].Width = 16;
            _smoothCheckBox[i].Margin = new Thickness(-50, 53, 0, 0);
            _smoothCheckBox[i].ToolTip = "Сглаживание";
            _smoothCheckBox[i].Click += (click_checkedButton);

            _stepTypeComboBox.Add(new ComboBox());
            _stepTypeComboBox[i].Width = 100;
            _stepTypeComboBox[i].VerticalAlignment = VerticalAlignment.Top;
            _stepTypeComboBox[i].Margin = new Thickness(-270, 75, 0, 0);
            _stepTypeComboBox[i].ToolTip = "Ступенчатость";
            _stepTypeComboBox[i].ItemsSource = _stepType;
            _stepTypeComboBox[i].SelectedIndex = 0;
            _stepTypeComboBox[i].SelectionChanged += (Change_index);

            _widthCheckBox.Add(new CheckBox());
            _widthCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            _widthCheckBox[i].Height = _visibleCheckBox[i].Width = 16;
            _widthCheckBox[i].Margin = new Thickness(-50, 78, 0, 0);
            _widthCheckBox[i].ToolTip = "Толщина";
            _widthCheckBox[i].Click += (click_checkedButton);
            
            _clearChannel[oscilNum].Add(i);

            _openClose.Add(new bool());
            _openClose[i] = false;

            _layoutPanel[i].Children.Add(_nameLabel[i]);
            _layoutPanel[i].Children.Add(_colorEllipse[i]);
            _layoutPanel[i].Children.Add(_visibleCheckBox[i]);
            _layoutPanel[i].Children.Add(_selectCheckBox[i]);
            _layoutPanel[i].Children.Add(_typeTypeComboBox[i]);    
            _layoutPanel[i].Children.Add(_typeComboBox[i]);
            _layoutPanel[i].Children.Add(_smoothCheckBox[i]);
            _layoutPanel[i].Children.Add(_stepTypeComboBox[i]);
            _layoutPanel[i].Children.Add(_widthCheckBox[i]);
            _layoutPanel[i].Children.Add(_panelBorder[i]);


            GraphStackPanel.Children.Add(_layoutPanel[i]);
        }

        private void TypeChange(object sender, SelectionChangedEventArgs e)
        {
            int j = 0;
            for (int i = 0; i < _colorEllipse.Count; i++)
            {
                if (_typeTypeComboBox[i].IsMouseOver ){j = i;break;}
            }
            MainWindow.Graph.ChangeDigitalList(j, _typeTypeComboBox[j].SelectedIndex);
        }

        private void Change_index(object sender, SelectionChangedEventArgs e)
        {
            int j = 0;
            for (int i = 0; i < _colorEllipse.Count; i++)
            {
                if (_typeComboBox[i].IsMouseOver || _stepTypeComboBox[i].IsMouseOver) { j = i; break; }
            }
            ChangeSomething(j, _typeComboBox[j].SelectedIndex, _stepTypeComboBox[j].SelectedIndex, GraphPanel.Pane.CurveList[j].Color);
        }
        
        private void click_checkedButton(object sender, EventArgs e)
        {
            int j = 0;
            for (int i = 0; i < _colorEllipse.Count; i++)
            {
                if (_visibleCheckBox[i].IsMouseOver|| _smoothCheckBox[i].IsMouseOver|| _widthCheckBox[i].IsMouseOver ) { j = i; break; }
            }
            ChangeSomething(j, _typeComboBox[j].SelectedIndex, _stepTypeComboBox[j].SelectedIndex, GraphPanel.Pane.CurveList[j].Color);
        }

        private void click_ColorEllipse(object sender, EventArgs e)
        {
            int j = 0;
            for (int i = 0; i < _colorEllipse.Count; i++)
            {
                if (_colorEllipse[i].IsMouseOver) { j = i; break; } 
            }
            var dialog = new System.Windows.Forms.ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var colorColorEllipse = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
                var brushColorEllipse = new SolidColorBrush(colorColorEllipse);
                _colorEllipse[j].Fill = brushColorEllipse;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(brushColorEllipse.Color.A, brushColorEllipse.Color.R, brushColorEllipse.Color.G, brushColorEllipse.Color.B);
                ChangeSomething(j, _typeComboBox[j].SelectedIndex, _stepTypeComboBox[j].SelectedIndex, color);
            }
        }

        private void click_LayoutPanel(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < _openClose.Count; i++)
            {
                if (_layoutPanel[i].IsMouseOver && _openClose[i] == false) OpenAnimation(i);
                else if (_layoutPanel[i].IsMouseOver && _openClose[i]) CloseAnimation(i);
            }
        }

        private void OpenAnimation(int i)
        {
            DoubleAnimation openAnimation = new DoubleAnimation
            {
                From = 25,
                To = 100,
                Duration = new Duration(TimeSpan.FromSeconds(0.1))
            };

            _layoutPanel[i].BeginAnimation(MinHeightProperty, openAnimation);
            _openClose[i] = true;
        }
        private void CloseAnimation(int i)
        {
            DoubleAnimation closeAnimation = new DoubleAnimation
            {
                From = 100,
                To = 25,
                Duration = new Duration(TimeSpan.FromSeconds(0.1))
            };

            _layoutPanel[i].BeginAnimation(MinHeightProperty, closeAnimation);
            _openClose[i] = false;

        }

        private void ChangeSomething(int num, int line, int typeStep, System.Drawing.Color color) {
            bool show = true, smooth = false, width = false;
            if (_visibleCheckBox[num].IsChecked == false) show = false;
            if (_smoothCheckBox[num].IsChecked == true) smooth = true;
            if (_widthCheckBox[num].IsChecked == true) width = true;
            MainWindow.Graph.ChangeLine(num, line, typeStep, width, show, smooth, color);
        }
    }
}
