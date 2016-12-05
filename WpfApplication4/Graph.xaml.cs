using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph : UserControl
    {
        //Заголовки осциллограммы
        //       List<DockPanel> _layoutPanel = new List<DockPanel>();
        List<Border> _oscilBorder = new List<Border>();
        List<Label> _oscilName = new List<Label>();
        List<CheckBox> _showAllCheckBox = new List<CheckBox>();
        List<CheckBox> _selectAllCheckBox = new List<CheckBox>();
        List<Rectangle> _closeButton = new List<Rectangle>();
        List<DockPanel> _layoutOscilPanel = new List<DockPanel>();


        List<DockPanel> _layoutPanel = new List<DockPanel>();
        List<Label> _nameLabel = new List<Label>();
        List<Ellipse> _colorEllipse = new List<Ellipse>();
        List<CheckBox> _visibleCheckBox = new List<CheckBox>();
        List<CheckBox> _selectCheckBox = new List<CheckBox>();

        List<ComboBox> _typeTypeComboBox = new List<ComboBox>();
        List<ComboBox> _typeComboBox = new List<ComboBox>();
        List<CheckBox> _smoothCheckBox = new List<CheckBox>();
        List<Border> _panelBorder = new List<Border>();
        List<bool> _openClose = new List<bool>();
        List<ComboBox> _stepTypeComboBox = new List<ComboBox>();
        List<CheckBox> _widthCheckBox = new List<CheckBox>();

        string[] _typeType = new string[] {
            "Analog",
            "Digital"
        };

        string[] _styleType = new string[] {
            "Solid",
            "Dash",
            "DashDot",
            "DashDotDot",
            "Dot"
        };

        string[] _stepType = new string[] {
            "NonStep",
            "ForwardSegment",
            "ForwardStep",
            "RearwardSegment",
            "RearwardStep"
        };

       // internal static object pane;

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
            _oscilName[_layoutOscilPanel.Count - 1].Content = "Осциллограмма №" + (_layoutOscilPanel.Count);
            _oscilName[_layoutOscilPanel.Count - 1].ToolTip = oscilName;
            _oscilName[_layoutOscilPanel.Count - 1].Width = 150;
            _oscilName[_layoutOscilPanel.Count - 1].Margin = new Thickness(0, 3, 0, 0);

            _selectAllCheckBox.Add(new CheckBox());
            _selectAllCheckBox[_layoutOscilPanel.Count - 1].ToolTip = "Выбрать все каналы";
            _selectAllCheckBox[_layoutOscilPanel.Count - 1].Margin = new Thickness(0, 8, 0, 0);

            _showAllCheckBox.Add(new CheckBox());
            _showAllCheckBox[_layoutOscilPanel.Count - 1].ToolTip = "Отображать все каналы";
            _showAllCheckBox[_layoutOscilPanel.Count - 1].Margin = new Thickness(0, 8, 0, 0);
            _showAllCheckBox[_layoutOscilPanel.Count - 1].IsChecked = true;

            _closeButton.Add(new Rectangle());
            _closeButton[_layoutOscilPanel.Count - 1].ToolTip = "Закрыть осциллограмму";
            _closeButton[_layoutOscilPanel.Count - 1].Width = _closeButton[_layoutOscilPanel.Count - 1].Height = 16;
            _closeButton[_layoutOscilPanel.Count - 1].Fill  = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close Window_20.png")));
            _closeButton[_layoutOscilPanel.Count - 1].Margin = new Thickness(0, 0, 0, 0);

            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Children.Add(_oscilName[_layoutOscilPanel.Count - 1]);
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Children.Add(_showAllCheckBox[_layoutOscilPanel.Count - 1]);
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Children.Add(_selectAllCheckBox[_layoutOscilPanel.Count - 1]);
            _layoutOscilPanel[_layoutOscilPanel.Count - 1].Children.Add(_closeButton[_layoutOscilPanel.Count - 1]); 

            GraphStackPanel.Children.Add(_layoutOscilPanel[_layoutOscilPanel.Count - 1]);
        }

        public void GraphConfigClear()
        {
            for(int i = _layoutPanel.Count - 1; i >= 0 ; i--)
            {
                _layoutPanel[i].Children.Remove(_nameLabel[i]);
                _layoutPanel[i].Children.Remove(_visibleCheckBox[i]);
                _layoutPanel[i].Children.Remove(_colorEllipse[i]);
                _layoutPanel[i].Children.Remove(_typeComboBox[i]);
                _layoutPanel[i].Children.Remove(_smoothCheckBox[i]);
                _layoutPanel[i].Children.Remove(_panelBorder[i]);
                _layoutPanel[i].Children.Remove(_stepTypeComboBox[i]);
                _layoutPanel[i].Children.Remove(_widthCheckBox[i]);
                GraphStackPanel.Children.Remove(_layoutPanel[i]);

                _nameLabel.Remove(_nameLabel[i]);
                _visibleCheckBox.Remove(_visibleCheckBox[i]);
                _colorEllipse.Remove(_colorEllipse[i]);
                _typeComboBox.Remove(_typeComboBox[i]);
                _smoothCheckBox.Remove(_smoothCheckBox[i]);
                _panelBorder.Remove(_panelBorder[i]);
                _stepTypeComboBox.Remove(_stepTypeComboBox[i]);
                _widthCheckBox.Remove(_widthCheckBox[i]);
            }

            _nameLabel.Clear();
            _visibleCheckBox.Clear();
            _colorEllipse.Clear();
            _typeComboBox.Clear();
            _smoothCheckBox.Clear();
            _panelBorder.Clear();
            _openClose.Clear();
            _stepTypeComboBox.Clear();
            _widthCheckBox.Clear();

            _layoutPanel.Clear();
        }

        public void GraphConfigAdd(string nameChannel, string dimensionChannel)
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
            _layoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanel);

            _nameLabel.Add(new Label());
            _nameLabel[i].Content = nameChannel;
            _nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            _nameLabel[i].FontSize = 12;
            _nameLabel[i].Height = 25;
            _nameLabel[i].Width = 140;
            _nameLabel[i].ToolTip = "Название канала:\n" + nameChannel + ", " + dimensionChannel; ;
            _nameLabel[i].Margin = new Thickness(0,0,0,0);

            _colorEllipse.Add(new Ellipse());
            _colorEllipse[i].Width = _colorEllipse[i].Height = 20;
            _colorEllipse[i].VerticalAlignment = VerticalAlignment.Top;
            _colorEllipse[i].Margin = new Thickness(0, 2, 0, 0);
            _colorEllipse[i].Fill = new SolidColorBrush(Color.FromArgb(GraphPanel.Pane.CurveList[i].Color.A, GraphPanel.Pane.CurveList[i].Color.R, GraphPanel.Pane.CurveList[i].Color.G, GraphPanel.Pane.CurveList[i].Color.B));
            _colorEllipse[i].ToolTip = "Цвет";
            _colorEllipse[i].MouseDown += new MouseButtonEventHandler(click_ColorEllipse);

            _visibleCheckBox.Add(new CheckBox());
            _visibleCheckBox[i].IsChecked = true;
            _visibleCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            _visibleCheckBox[i].Height = _visibleCheckBox[i].Width = 16;
            _visibleCheckBox[i].Margin = new Thickness(0, 5, 0, 0);
            _visibleCheckBox[i].ToolTip = "Отображать";
            _visibleCheckBox[i].Click += new RoutedEventHandler(click_checkedButton);

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
            _typeTypeComboBox[i].SelectedIndex = 0;
            
            _typeComboBox.Add(new ComboBox());
            _typeComboBox[i].Width = 100;
            _typeComboBox[i].VerticalAlignment = VerticalAlignment.Top;
            _typeComboBox[i].Margin = new Thickness(-270, 50, 0, 0);
            _typeComboBox[i].ToolTip = "Тип линии";
            _typeComboBox[i].ItemsSource = _styleType;
            _typeComboBox[i].SelectedIndex = 0;
            _typeComboBox[i].SelectionChanged += new SelectionChangedEventHandler (change_index);
            
            _smoothCheckBox.Add(new CheckBox());
            _smoothCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            _smoothCheckBox[i].Height = _visibleCheckBox[i].Width = 16;
            _smoothCheckBox[i].Margin = new Thickness(-50, 53, 0, 0);
            _smoothCheckBox[i].ToolTip = "Сглаживание";
            _smoothCheckBox[i].Click += new RoutedEventHandler(click_checkedButton);

            _stepTypeComboBox.Add(new ComboBox());
            _stepTypeComboBox[i].Width = 100;
            _stepTypeComboBox[i].VerticalAlignment = VerticalAlignment.Top;
            _stepTypeComboBox[i].Margin = new Thickness(-270, 75, 0, 0);
            _stepTypeComboBox[i].ToolTip = "Ступенчатость";
            _stepTypeComboBox[i].ItemsSource = _stepType;
            _stepTypeComboBox[i].SelectedIndex = 0;
            _stepTypeComboBox[i].SelectionChanged += new SelectionChangedEventHandler(change_index);

            _widthCheckBox.Add(new CheckBox());
            _widthCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            _widthCheckBox[i].Height = _visibleCheckBox[i].Width = 16;
            _widthCheckBox[i].Margin = new Thickness(-50, 78, 0, 0);
            _widthCheckBox[i].ToolTip = "Толщина";
            _widthCheckBox[i].Click += new RoutedEventHandler(click_checkedButton);

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

        private void change_index(object sender, SelectionChangedEventArgs e)
        {
            int j = 0;
            for (int i = 0; i < _colorEllipse.Count; i++)
            {
                if (_typeComboBox[i].IsMouseOver == true || _stepTypeComboBox[i].IsMouseOver == true) { j = i; break; }
            }
            ChangeSomething(j, _typeComboBox[j].SelectedIndex, _stepTypeComboBox[j].SelectedIndex, GraphPanel.Pane.CurveList[j].Color);
        }
        
        private void click_checkedButton(object sender, EventArgs e)
        {
            int j = 0;
            for (int i = 0; i < _colorEllipse.Count; i++)
            {
                if (_visibleCheckBox[i].IsMouseOver == true || _smoothCheckBox[i].IsMouseOver == true || _widthCheckBox[i].IsMouseOver == true) { j = i; break; }
            }
            ChangeSomething(j, _typeComboBox[j].SelectedIndex, _stepTypeComboBox[j].SelectedIndex, GraphPanel.Pane.CurveList[j].Color);
        }

        private void click_ColorEllipse(object sender, EventArgs e)
        {
            int j = 0;
            for (int i = 0; i < _colorEllipse.Count; i++)
            {
                if (_colorEllipse[i].IsMouseOver == true) { j = i; break; } 
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
                if (_layoutPanel[i].IsMouseOver == true && _openClose[i] == false) OpenAnimation(i);
                else if (_layoutPanel[i].IsMouseOver == true && _openClose[i] == true) CloseAnimation(i);
            }
        }

        private void OpenAnimation(int i)
        {
            DoubleAnimation openAnimation = new DoubleAnimation();
            openAnimation.From = 25;
            openAnimation.To = 100;

            openAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            _layoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, openAnimation);
            _openClose[i] = true;
        }
        private void CloseAnimation(int i)
        {
            DoubleAnimation closeAnimation = new DoubleAnimation();
            closeAnimation.From = 100;
            closeAnimation.To = 25;
            closeAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            _layoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, closeAnimation);
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
