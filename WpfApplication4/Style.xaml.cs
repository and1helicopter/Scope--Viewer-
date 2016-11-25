using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Style.xaml
    /// </summary>
    public partial class Style : UserControl
    {
        List<DockPanel> _layoutPanel = new List<DockPanel>();
        List<bool> _openClose = new List<bool>();
        List<Border> _panelBorder = new List<Border>();
        List<Label> _nameLabel = new List<Label>();
        CheckBox _showMajor;
        CheckBox _xMinor;
        CheckBox _xMajor;
        CheckBox _yMinor;
        CheckBox _yMajor;
        ComboBox _yMajorDash;
        ComboBox _yMinorDash;
        ComboBox _xMajorDash;
        ComboBox _xMinorDash;

        CheckBox _legend;
        Slider _legendfontSize;
        RadioButton _position00;
        RadioButton _position10;
        RadioButton _position01;
        RadioButton _position11;
        Rectangle _viewDisplay;

        string[] _styleTypeMajor = new string[] {
            "Dot",
            "Dash",
            "Solid"            
        };

        string[] _styleTypeMinor = new string[] {
            "Dot",
            "Dash"
        };

        public Style()
        {
            InitializeComponent();
            StyleGridConfigAdd();
            LegendConfigAdd();
        }

        public void StyleGridConfigAdd()
        {
            int i;
            _layoutPanel.Add(new DockPanel());
            i = _layoutPanel.Count - 1;
            _layoutPanel[i].Width = 190;
            _layoutPanel[i].Height = 30;
            _layoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            _layoutPanel[i].Background = Brushes.White;
            _layoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanel);

            _panelBorder.Add(new System.Windows.Controls.Border());
            _panelBorder[i].BorderBrush = Brushes.DarkGray;
            _panelBorder[i].BorderThickness = new Thickness(1.0);
            _panelBorder[i].Margin = new Thickness(-190, 0, 0, 0);

            _nameLabel.Add(new System.Windows.Controls.Label());
            _nameLabel[i].Content = "Grid";
            _nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            _nameLabel[i].FontSize = 14;
            _nameLabel[i].Height = 30;
            _nameLabel[i].Width = 155;
            _nameLabel[i].ToolTip = "Настройка сетки";
            _nameLabel[i].Margin = new Thickness(0, 0, 0, 0);

            _showMajor = new CheckBox();
            _showMajor.VerticalAlignment = VerticalAlignment.Top;
            _showMajor.Margin = new Thickness(0, 5, 0, 0);
            _showMajor.Height = _showMajor.Width = 16;
            _showMajor.Checked += new RoutedEventHandler(AxisChange_Checked);
            _showMajor.Unchecked += new RoutedEventHandler(AxisChange_Checked);
            _showMajor.IsChecked = true;

            _xMinor = new CheckBox();
            _xMinor.VerticalAlignment = VerticalAlignment.Top;
            _xMinor.Margin = new Thickness(-170, 25, 0, 0);
            _xMinor.Content = "XMinor";
            _xMinor.FontSize = 12;
            _xMinor.Visibility = Visibility.Hidden;
            _xMinor.Checked += new RoutedEventHandler(AxisChange_Checked);
            _xMinor.Unchecked += new RoutedEventHandler(AxisChange_Checked);

            _xMajor = new CheckBox();
            _xMajor.VerticalAlignment = VerticalAlignment.Top;
            _xMajor.Margin = new Thickness(-170, 50, 0, 0);
            _xMajor.Content = "XMajor";
            _xMajor.FontSize = 12;
            _xMajor.Visibility = Visibility.Hidden;
            _xMajor.Checked += new RoutedEventHandler(AxisChange_Checked);
            _xMajor.Unchecked += new RoutedEventHandler(AxisChange_Checked);
            _xMajor.IsChecked = true;

            _yMinor = new CheckBox();
            _yMinor.VerticalAlignment = VerticalAlignment.Top;
            _yMinor.Margin = new Thickness(-170, 75, 0, 0);
            _yMinor.Content = "YMinor";
            _yMinor.FontSize = 12;
            _yMinor.Visibility = Visibility.Hidden;
            _yMinor.Checked += new RoutedEventHandler(AxisChange_Checked);
            _yMinor.Unchecked += new RoutedEventHandler(AxisChange_Checked);

            _yMajor = new CheckBox();
            _yMajor.VerticalAlignment = VerticalAlignment.Top;
            _yMajor.Margin = new Thickness(-170, 100, 0, 0);
            _yMajor.Content = "YMajor";
            _yMajor.FontSize = 12;
            _yMajor.Visibility = Visibility.Hidden;
            _yMajor.Checked += new RoutedEventHandler(AxisChange_Checked);
            _yMajor.Unchecked += new RoutedEventHandler(AxisChange_Checked);
            _yMajor.IsChecked = true;

            _xMinorDash = new ComboBox();
            _xMinorDash.VerticalAlignment = VerticalAlignment.Top;
            _xMinorDash.Width = 80;
            _xMinorDash.ItemsSource = _styleTypeMinor;
            _xMinorDash.SelectedIndex = 0;
            _xMinorDash.Margin = new Thickness(-100, 20, 0, 0);
            _xMinorDash.FontSize = 12;
            _xMinorDash.Visibility = Visibility.Hidden;
            _xMinorDash.SelectionChanged += new SelectionChangedEventHandler(AxisChange_Checked);

            _xMajorDash = new ComboBox();
            _xMajorDash.VerticalAlignment = VerticalAlignment.Top;
            _xMajorDash.Width = 80;
            _xMajorDash.ItemsSource = _styleTypeMajor;
            _xMajorDash.SelectedIndex = 0;
            _xMajorDash.Margin = new Thickness(-100, 45, 0, 0);
            _xMajorDash.FontSize = 12;
            _xMajorDash.Visibility = Visibility.Hidden;
            _xMajorDash.SelectionChanged += new SelectionChangedEventHandler(AxisChange_Checked);

            _yMinorDash = new ComboBox();
            _yMinorDash.VerticalAlignment = VerticalAlignment.Top;
            _yMinorDash.Width = 80;
            _yMinorDash.ItemsSource = _styleTypeMinor;
            _yMinorDash.SelectedIndex = 0;
            _yMinorDash.Margin = new Thickness(-100, 70, 0, 0);
            _yMinorDash.FontSize = 12;
            _yMinorDash.Visibility = Visibility.Hidden;
            _yMinorDash.SelectionChanged += new SelectionChangedEventHandler(AxisChange_Checked);

            _yMajorDash = new ComboBox();
            _yMajorDash.VerticalAlignment = VerticalAlignment.Top;
            _yMajorDash.Width = 80;
            _yMajorDash.ItemsSource = _styleTypeMajor;
            _yMajorDash.SelectedIndex = 0;
            _yMajorDash.Margin = new Thickness(-100, 95, 0, 0);
            _yMajorDash.FontSize = 12;
            _yMajorDash.Visibility = Visibility.Hidden;
            _yMajorDash.SelectionChanged += new SelectionChangedEventHandler(AxisChange_Checked);

            _openClose.Add(new bool());
            _openClose[i] = false;

            _layoutPanel[i].Children.Add(_nameLabel[i]);
            _layoutPanel[i].Children.Add(_showMajor);
            _layoutPanel[i].Children.Add(_xMinor);
            _layoutPanel[i].Children.Add(_xMajor);
            _layoutPanel[i].Children.Add(_yMinor);
            _layoutPanel[i].Children.Add(_yMajor);

            _layoutPanel[i].Children.Add(_xMinorDash);
            _layoutPanel[i].Children.Add(_xMajorDash);
            _layoutPanel[i].Children.Add(_yMinorDash);
            _layoutPanel[i].Children.Add(_yMajorDash);

            _layoutPanel[i].Children.Add(_panelBorder[i]);

            StyleStackPanel.Children.Add(_layoutPanel[i]);
        }

        public void LegendConfigAdd()
        {
            int i;
            _layoutPanel.Add(new DockPanel());
            i = _layoutPanel.Count - 1;
            _layoutPanel[i].Width = 190;
            _layoutPanel[i].Height = 30;
            _layoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            _layoutPanel[i].Background = Brushes.White;
            _layoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanel);

            _panelBorder.Add(new System.Windows.Controls.Border());
            _panelBorder[i].BorderBrush = Brushes.DarkGray;
            _panelBorder[i].BorderThickness = new Thickness(1.0);
            _panelBorder[i].Margin = new Thickness(-190, 0, 0, 0);

            _nameLabel.Add(new System.Windows.Controls.Label());
            _nameLabel[i].Content = "Legend";
            _nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            _nameLabel[i].FontSize = 14;
            _nameLabel[i].Height = 30;
            _nameLabel[i].Width = 155;
            _nameLabel[i].ToolTip = "Легенда";
            _nameLabel[i].Margin = new Thickness(0, 0, 0, 0);

            _legend = new CheckBox();
            _legend.VerticalAlignment = VerticalAlignment.Top;
            _legend.Height = _legend.Width = 16;
            _legend.Margin = new Thickness(0, 5, 0, 0);
            _legend.ToolTip = "Показать легенду";
            _legend.Checked += new RoutedEventHandler(Legend_Click);
            _legend.Unchecked += new RoutedEventHandler(Legend_Click);

            _legendfontSize = new Slider();
            _legendfontSize.VerticalAlignment = VerticalAlignment.Top;
            _legendfontSize.Margin = new Thickness(-200, 30, 0, 0);
            _legendfontSize.ToolTip = "Шрифт";
            _legendfontSize.Value = 7;
            _legendfontSize.Width = 140;
            _legendfontSize.SmallChange = 1;
            _legendfontSize.Minimum = 5;
            _legendfontSize.Maximum = 12;
            _legendfontSize.Visibility = Visibility.Hidden;
            _legendfontSize.ValueChanged += new RoutedPropertyChangedEventHandler<double>(Legend_Changed);

            _viewDisplay = new Rectangle();
            _viewDisplay.VerticalAlignment = VerticalAlignment.Top;
            _viewDisplay.Height = 60;
            _viewDisplay.Width = 85;
            _viewDisplay.Stroke = Brushes.LightSlateGray;
            _viewDisplay.Margin = new Thickness(-227, 50, 0, 0);
            _viewDisplay.Visibility = Visibility.Hidden;
            
            _position00 = new RadioButton();
            _position00.VerticalAlignment = VerticalAlignment.Top;
            _position00.FontSize = 9;
            _position00.ToolTip = "Top Left";
            _position00.GroupName = "Position";
            _position00.Margin = new Thickness(-155, 52, 0, 0);
            _position00.Visibility = Visibility.Hidden;
            _position00.Checked += new RoutedEventHandler(Legend_Click);

            _position10 = new RadioButton();
            _position10.VerticalAlignment = VerticalAlignment.Top;
            _position10.FontSize = 9;
            _position10.ToolTip = "Top Right";
            _position10.GroupName = "Position";
            _position10.Margin = new Thickness(-90, 52, 0, 0);
            _position10.Visibility = Visibility.Hidden;
            _position10.Checked += new RoutedEventHandler(Legend_Click);

            _position01 = new RadioButton();
            _position01.VerticalAlignment = VerticalAlignment.Top;
            _position01.FontSize = 9;
            _position01.ToolTip = "Bottom Left";
            _position01.GroupName = "Position";
            _position01.Margin = new Thickness(-155, 90, 0, 0);
            _position01.Visibility = Visibility.Hidden;
            _position01.Checked += new RoutedEventHandler(Legend_Click);

            _position11 = new RadioButton();
            _position11.VerticalAlignment = VerticalAlignment.Top;
            _position11.FontSize = 9;
            _position11.ToolTip = "Bottom Right";
            _position11.GroupName = "Position";
            _position11.IsChecked = true;
            _position11.Margin = new Thickness(-90, 90, 0, 0);
            _position11.Visibility = Visibility.Hidden;
            _position11.Checked += new RoutedEventHandler(Legend_Click);

            _openClose.Add(new bool());
            _openClose[i] = false;

            _layoutPanel[i].Children.Add(_nameLabel[i]);
            _layoutPanel[i].Children.Add(_legend);
            _layoutPanel[i].Children.Add(_legendfontSize);
            _layoutPanel[i].Children.Add(_viewDisplay);
            _layoutPanel[i].Children.Add(_position00);
            _layoutPanel[i].Children.Add(_position10);
            _layoutPanel[i].Children.Add(_position01);
            _layoutPanel[i].Children.Add(_position11);

            _layoutPanel[i].Children.Add(_panelBorder[i]);

            StyleStackPanel.Children.Add(_layoutPanel[i]);
        }

        private void Legend_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LegendChange();
        }

        private void Legend_Click(object sender, RoutedEventArgs e)
        {
            LegendChange();
        }

        private void LegendChange()
        {
            bool checkedLegend = false;
            int h = 1, v = 1;
            if (_legend.IsChecked == true) checkedLegend = true;
            if (_position00.IsChecked == true) { h = 0; v = 0; }
            if (_position10.IsChecked == true) { h = 1; v = 0; }
            if (_position01.IsChecked == true) { h = 0; v = 1; }
            if (_position11.IsChecked == true) { h = 1; v = 1; }
            MainWindow.Graph.LegendShow(checkedLegend, Convert.ToInt32(_legendfontSize.Value), h, v);
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
            openAnimation.From = 30;
            openAnimation.To = 125;

            openAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            _layoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, openAnimation);
            _openClose[i] = true;
            if(i == 0)
            {
                _xMinor.Visibility = Visibility.Visible;
                _xMajor.Visibility = Visibility.Visible;
                _yMinor.Visibility = Visibility.Visible;
                _yMajor.Visibility = Visibility.Visible;
                _yMajorDash.Visibility = Visibility.Visible;
                _yMinorDash.Visibility = Visibility.Visible;
                _xMajorDash.Visibility = Visibility.Visible;
                _xMinorDash.Visibility = Visibility.Visible;
            }
            if(i == 1)
            {
                _legendfontSize.Visibility = Visibility.Visible;
                _viewDisplay.Visibility = Visibility.Visible;
                _position00.Visibility = Visibility.Visible;
                _position10.Visibility = Visibility.Visible;
                _position01.Visibility = Visibility.Visible;
                _position11.Visibility = Visibility.Visible;
            }
        }
        private void CloseAnimation(int i)
        {
            DoubleAnimation closeAnimation = new DoubleAnimation();
            closeAnimation.From = 125;
            closeAnimation.To = 30;
            closeAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            _layoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, closeAnimation);
            _openClose[i] = false;
            if (i == 0)
            {
                _xMinor.Visibility = Visibility.Hidden;
                _xMajor.Visibility = Visibility.Hidden;
                _yMinor.Visibility = Visibility.Hidden;
                _yMajor.Visibility = Visibility.Hidden;
                _yMajorDash.Visibility = Visibility.Hidden;
                _yMinorDash.Visibility = Visibility.Hidden;
                _xMajorDash.Visibility = Visibility.Hidden;
                _xMinorDash.Visibility = Visibility.Hidden;
            }
            if (i == 1)
            {
                _legendfontSize.Visibility = Visibility.Hidden;
                _viewDisplay.Visibility = Visibility.Hidden;
                _position00.Visibility = Visibility.Hidden;
                _position10.Visibility = Visibility.Hidden;
                _position01.Visibility = Visibility.Hidden;
                _position11.Visibility = Visibility.Hidden;
            }
        }

        public void AxisChange_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_showMajor.IsChecked == true && _showMajor.IsMouseOver == true)
                {
                    _xMajor.IsChecked = true;
                    _yMajor.IsChecked = true;
                }
                if (_showMajor.IsChecked == false && _showMajor.IsMouseOver == true)
                {
                    _xMajor.IsChecked = false;
                    _yMajor.IsChecked = false;
                }
                if (_xMajor.IsChecked == false || _yMajor.IsChecked == false) { _showMajor.IsChecked = false; }

                if (_xMinor.IsChecked == true) MainWindow.Graph.GridAxisChange(true, _xMinorDash.SelectedIndex, 0);
                else MainWindow.Graph.GridAxisChange(false, _xMinorDash.SelectedIndex, 0);
                if (_xMajor.IsChecked == true) MainWindow.Graph.GridAxisChange(true, _xMajorDash.SelectedIndex, 1);
                else MainWindow.Graph.GridAxisChange(false, _xMajorDash.SelectedIndex, 1);
                if (_yMinor.IsChecked == true) MainWindow.Graph.GridAxisChange(true, _yMinorDash.SelectedIndex, 2);
                else MainWindow.Graph.GridAxisChange(false, _yMinorDash.SelectedIndex, 2);
                if (_yMajor.IsChecked == true) MainWindow.Graph.GridAxisChange(true, _yMajorDash.SelectedIndex, 3);
                else MainWindow.Graph.GridAxisChange(false, _yMajorDash.SelectedIndex, 3);
            }
            catch { }
        }
    }
}
