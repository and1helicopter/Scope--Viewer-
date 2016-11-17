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
        List<DockPanel> LayoutPanel = new List<DockPanel>();
        List<bool> OpenClose = new List<bool>();
        List<Border> panelBorder = new List<Border>();
        List<Label> nameLabel = new List<Label>();
        CheckBox ShowMajor;
        CheckBox XMinor;
        CheckBox XMajor;
        CheckBox YMinor;
        CheckBox YMajor;
        ComboBox YMajorDash;
        ComboBox YMinorDash;
        ComboBox XMajorDash;
        ComboBox XMinorDash;

        CheckBox Legend;
        Slider LegendfontSize;
        RadioButton Position00;
        RadioButton Position10;
        RadioButton Position01;
        RadioButton Position11;
        Rectangle ViewDisplay;

        string[] StyleTypeMajor = new string[] {
            "Dot",
            "Dash",
            "Solid"            
        };

        string[] StyleTypeMinor = new string[] {
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
            LayoutPanel.Add(new DockPanel());
            i = LayoutPanel.Count - 1;
            LayoutPanel[i].Width = 190;
            LayoutPanel[i].Height = 30;
            LayoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            LayoutPanel[i].Background = Brushes.White;
            LayoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanel);

            panelBorder.Add(new System.Windows.Controls.Border());
            panelBorder[i].BorderBrush = Brushes.DarkGray;
            panelBorder[i].BorderThickness = new Thickness(1.0);
            panelBorder[i].Margin = new Thickness(-190, 0, 0, 0);

            nameLabel.Add(new System.Windows.Controls.Label());
            nameLabel[i].Content = "Grid";
            nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            nameLabel[i].FontSize = 14;
            nameLabel[i].Height = 30;
            nameLabel[i].Width = 155;
            nameLabel[i].ToolTip = "Настройка сетки";
            nameLabel[i].Margin = new Thickness(0, 0, 0, 0);

            ShowMajor = new CheckBox();
            ShowMajor.VerticalAlignment = VerticalAlignment.Top;
            ShowMajor.Margin = new Thickness(0, 5, 0, 0);
            ShowMajor.Height = ShowMajor.Width = 16;
            ShowMajor.Checked += new RoutedEventHandler(AxisChange_Checked);
            ShowMajor.Unchecked += new RoutedEventHandler(AxisChange_Checked);

            XMinor = new CheckBox();
            XMinor.VerticalAlignment = VerticalAlignment.Top;
            XMinor.Margin = new Thickness(-170, 25, 0, 0);
            XMinor.Content = "XMinor";
            XMinor.FontSize = 12;
            XMinor.Visibility = Visibility.Hidden;
            XMinor.Checked += new RoutedEventHandler(AxisChange_Checked);
            XMinor.Unchecked += new RoutedEventHandler(AxisChange_Checked);

            XMajor = new CheckBox();
            XMajor.VerticalAlignment = VerticalAlignment.Top;
            XMajor.Margin = new Thickness(-170, 50, 0, 0);
            XMajor.Content = "XMajor";
            XMajor.FontSize = 12;
            XMajor.Visibility = Visibility.Hidden;
            XMajor.Checked += new RoutedEventHandler(AxisChange_Checked);
            XMajor.Unchecked += new RoutedEventHandler(AxisChange_Checked);

            YMinor = new CheckBox();
            YMinor.VerticalAlignment = VerticalAlignment.Top;
            YMinor.Margin = new Thickness(-170, 75, 0, 0);
            YMinor.Content = "YMinor";
            YMinor.FontSize = 12;
            YMinor.Visibility = Visibility.Hidden;
            YMinor.Checked += new RoutedEventHandler(AxisChange_Checked);
            YMinor.Unchecked += new RoutedEventHandler(AxisChange_Checked);

            YMajor = new CheckBox();
            YMajor.VerticalAlignment = VerticalAlignment.Top;
            YMajor.Margin = new Thickness(-170, 100, 0, 0);
            YMajor.Content = "YMajor";
            YMajor.FontSize = 12;
            YMajor.Visibility = Visibility.Hidden;
            YMajor.Checked += new RoutedEventHandler(AxisChange_Checked);
            YMajor.Unchecked += new RoutedEventHandler(AxisChange_Checked);

            XMinorDash = new ComboBox();
            XMinorDash.VerticalAlignment = VerticalAlignment.Top;
            XMinorDash.Width = 80;
            XMinorDash.ItemsSource = StyleTypeMinor;
            XMinorDash.SelectedIndex = 0;
            XMinorDash.Margin = new Thickness(-100, 20, 0, 0);
            XMinorDash.FontSize = 12;
            XMinorDash.Visibility = Visibility.Hidden;
            XMinorDash.SelectionChanged += new SelectionChangedEventHandler(AxisChange_Checked);

            XMajorDash = new ComboBox();
            XMajorDash.VerticalAlignment = VerticalAlignment.Top;
            XMajorDash.Width = 80;
            XMajorDash.ItemsSource = StyleTypeMajor;
            XMajorDash.SelectedIndex = 0;
            XMajorDash.Margin = new Thickness(-100, 45, 0, 0);
            XMajorDash.FontSize = 12;
            XMajorDash.Visibility = Visibility.Hidden;
            XMajorDash.SelectionChanged += new SelectionChangedEventHandler(AxisChange_Checked);

            YMinorDash = new ComboBox();
            YMinorDash.VerticalAlignment = VerticalAlignment.Top;
            YMinorDash.Width = 80;
            YMinorDash.ItemsSource = StyleTypeMinor;
            YMinorDash.SelectedIndex = 0;
            YMinorDash.Margin = new Thickness(-100, 70, 0, 0);
            YMinorDash.FontSize = 12;
            YMinorDash.Visibility = Visibility.Hidden;
            YMinorDash.SelectionChanged += new SelectionChangedEventHandler(AxisChange_Checked);

            YMajorDash = new ComboBox();
            YMajorDash.VerticalAlignment = VerticalAlignment.Top;
            YMajorDash.Width = 80;
            YMajorDash.ItemsSource = StyleTypeMajor;
            YMajorDash.SelectedIndex = 0;
            YMajorDash.Margin = new Thickness(-100, 95, 0, 0);
            YMajorDash.FontSize = 12;
            YMajorDash.Visibility = Visibility.Hidden;
            YMajorDash.SelectionChanged += new SelectionChangedEventHandler(AxisChange_Checked);

            OpenClose.Add(new bool());
            OpenClose[i] = false;

            LayoutPanel[i].Children.Add(nameLabel[i]);
            LayoutPanel[i].Children.Add(ShowMajor);
            LayoutPanel[i].Children.Add(XMinor);
            LayoutPanel[i].Children.Add(XMajor);
            LayoutPanel[i].Children.Add(YMinor);
            LayoutPanel[i].Children.Add(YMajor);

            LayoutPanel[i].Children.Add(XMinorDash);
            LayoutPanel[i].Children.Add(XMajorDash);
            LayoutPanel[i].Children.Add(YMinorDash);
            LayoutPanel[i].Children.Add(YMajorDash);

            LayoutPanel[i].Children.Add(panelBorder[i]);

            StyleStackPanel.Children.Add(LayoutPanel[i]);
        }

        public void LegendConfigAdd()
        {
            int i;
            LayoutPanel.Add(new DockPanel());
            i = LayoutPanel.Count - 1;
            LayoutPanel[i].Width = 190;
            LayoutPanel[i].Height = 30;
            LayoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            LayoutPanel[i].Background = Brushes.White;
            LayoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanel);

            panelBorder.Add(new System.Windows.Controls.Border());
            panelBorder[i].BorderBrush = Brushes.DarkGray;
            panelBorder[i].BorderThickness = new Thickness(1.0);
            panelBorder[i].Margin = new Thickness(-190, 0, 0, 0);

            nameLabel.Add(new System.Windows.Controls.Label());
            nameLabel[i].Content = "Legend";
            nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            nameLabel[i].FontSize = 14;
            nameLabel[i].Height = 30;
            nameLabel[i].Width = 155;
            nameLabel[i].ToolTip = "Легенда";
            nameLabel[i].Margin = new Thickness(0, 0, 0, 0);

            Legend = new CheckBox();
            Legend.VerticalAlignment = VerticalAlignment.Top;
            Legend.Height = Legend.Width = 16;
            Legend.Margin = new Thickness(0, 5, 0, 0);
            Legend.ToolTip = "Показать легенду";
            Legend.Checked += new RoutedEventHandler(Legend_Click);
            Legend.Unchecked += new RoutedEventHandler(Legend_Click);

            LegendfontSize = new Slider();
            LegendfontSize.VerticalAlignment = VerticalAlignment.Top;
            LegendfontSize.Margin = new Thickness(-200, 30, 0, 0);
            LegendfontSize.ToolTip = "Шрифт";
            LegendfontSize.Value = 7;
            LegendfontSize.Width = 140;
            LegendfontSize.SmallChange = 1;
            LegendfontSize.Minimum = 5;
            LegendfontSize.Maximum = 12;
            LegendfontSize.Visibility = Visibility.Hidden;
            LegendfontSize.ValueChanged += new RoutedPropertyChangedEventHandler<double>(Legend_Changed);

            ViewDisplay = new Rectangle();
            ViewDisplay.VerticalAlignment = VerticalAlignment.Top;
            ViewDisplay.Height = 60;
            ViewDisplay.Width = 85;
            ViewDisplay.Stroke = Brushes.LightSlateGray;
            ViewDisplay.Margin = new Thickness(-227, 50, 0, 0);
            ViewDisplay.Visibility = Visibility.Hidden;
            
            Position00 = new RadioButton();
            Position00.VerticalAlignment = VerticalAlignment.Top;
            Position00.FontSize = 9;
            Position00.ToolTip = "Top Left";
            Position00.GroupName = "Position";
            Position00.Margin = new Thickness(-155, 52, 0, 0);
            Position00.Visibility = Visibility.Hidden;
            Position00.Checked += new RoutedEventHandler(Legend_Click);

            Position10 = new RadioButton();
            Position10.VerticalAlignment = VerticalAlignment.Top;
            Position10.FontSize = 9;
            Position10.ToolTip = "Top Right";
            Position10.GroupName = "Position";
            Position10.Margin = new Thickness(-90, 52, 0, 0);
            Position10.Visibility = Visibility.Hidden;
            Position10.Checked += new RoutedEventHandler(Legend_Click);

            Position01 = new RadioButton();
            Position01.VerticalAlignment = VerticalAlignment.Top;
            Position01.FontSize = 9;
            Position01.ToolTip = "Bottom Left";
            Position01.GroupName = "Position";
            Position01.Margin = new Thickness(-155, 90, 0, 0);
            Position01.Visibility = Visibility.Hidden;
            Position01.Checked += new RoutedEventHandler(Legend_Click);

            Position11 = new RadioButton();
            Position11.VerticalAlignment = VerticalAlignment.Top;
            Position11.FontSize = 9;
            Position11.ToolTip = "Bottom Right";
            Position11.GroupName = "Position";
            Position11.IsChecked = true;
            Position11.Margin = new Thickness(-90, 90, 0, 0);
            Position11.Visibility = Visibility.Hidden;
            Position11.Checked += new RoutedEventHandler(Legend_Click);

            OpenClose.Add(new bool());
            OpenClose[i] = false;

            LayoutPanel[i].Children.Add(nameLabel[i]);
            LayoutPanel[i].Children.Add(Legend);
            LayoutPanel[i].Children.Add(LegendfontSize);
            LayoutPanel[i].Children.Add(ViewDisplay);
            LayoutPanel[i].Children.Add(Position00);
            LayoutPanel[i].Children.Add(Position10);
            LayoutPanel[i].Children.Add(Position01);
            LayoutPanel[i].Children.Add(Position11);

            LayoutPanel[i].Children.Add(panelBorder[i]);

            StyleStackPanel.Children.Add(LayoutPanel[i]);
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
            int H = 1, V = 1;
            if (Legend.IsChecked == true) checkedLegend = true;
            if (Position00.IsChecked == true) { H = 0; V = 0; }
            if (Position10.IsChecked == true) { H = 1; V = 0; }
            if (Position01.IsChecked == true) { H = 0; V = 1; }
            if (Position11.IsChecked == true) { H = 1; V = 1; }
            MainWindow.graph.LegendShow(checkedLegend, Convert.ToInt32(LegendfontSize.Value), H, V);
        }

        private void click_LayoutPanel(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < OpenClose.Count; i++)
            {
                if (LayoutPanel[i].IsMouseOver == true && OpenClose[i] == false) OpenAnimation(i);
                else if (LayoutPanel[i].IsMouseOver == true && OpenClose[i] == true) CloseAnimation(i);
            }
        }

        private void OpenAnimation(int i)
        {
            DoubleAnimation OpenAnimation = new DoubleAnimation();
            OpenAnimation.From = 30;
            OpenAnimation.To = 125;

            OpenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            LayoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, OpenAnimation);
            OpenClose[i] = true;
            if(i == 0)
            {
                XMinor.Visibility = Visibility.Visible;
                XMajor.Visibility = Visibility.Visible;
                YMinor.Visibility = Visibility.Visible;
                YMajor.Visibility = Visibility.Visible;
                YMajorDash.Visibility = Visibility.Visible;
                YMinorDash.Visibility = Visibility.Visible;
                XMajorDash.Visibility = Visibility.Visible;
                XMinorDash.Visibility = Visibility.Visible;
            }
            if(i == 1)
            {
                LegendfontSize.Visibility = Visibility.Visible;
                ViewDisplay.Visibility = Visibility.Visible;
                Position00.Visibility = Visibility.Visible;
                Position10.Visibility = Visibility.Visible;
                Position01.Visibility = Visibility.Visible;
                Position11.Visibility = Visibility.Visible;
            }
        }
        private void CloseAnimation(int i)
        {
            DoubleAnimation CloseAnimation = new DoubleAnimation();
            CloseAnimation.From = 125;
            CloseAnimation.To = 30;
            CloseAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            LayoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, CloseAnimation);
            OpenClose[i] = false;
            if (i == 0)
            {
                XMinor.Visibility = Visibility.Hidden;
                XMajor.Visibility = Visibility.Hidden;
                YMinor.Visibility = Visibility.Hidden;
                YMajor.Visibility = Visibility.Hidden;
                YMajorDash.Visibility = Visibility.Hidden;
                YMinorDash.Visibility = Visibility.Hidden;
                XMajorDash.Visibility = Visibility.Hidden;
                XMinorDash.Visibility = Visibility.Hidden;
            }
            if (i == 1)
            {
                LegendfontSize.Visibility = Visibility.Hidden;
                ViewDisplay.Visibility = Visibility.Hidden;
                Position00.Visibility = Visibility.Hidden;
                Position10.Visibility = Visibility.Hidden;
                Position01.Visibility = Visibility.Hidden;
                Position11.Visibility = Visibility.Hidden;
            }
        }

        public void AxisChange_Checked(object sender, RoutedEventArgs e)
        {
            if (ShowMajor.IsChecked == true && ShowMajor.IsMouseOver == true)
            {
                XMajor.IsChecked = true;
                YMajor.IsChecked = true;
            }
            if(ShowMajor.IsChecked == false && ShowMajor.IsMouseOver == true)
            {
                XMajor.IsChecked = false;
                YMajor.IsChecked = false;
            }
            if (XMajor.IsChecked == false || YMajor.IsChecked == false) { ShowMajor.IsChecked = false; }

            if (XMinor.IsChecked == true) MainWindow.graph.GRIDAxisChange(true, XMinorDash.SelectedIndex, 0);
            else MainWindow.graph.GRIDAxisChange(false, XMinorDash.SelectedIndex, 0);
            if (XMajor.IsChecked == true) MainWindow.graph.GRIDAxisChange(true, XMajorDash.SelectedIndex, 1); 
            else MainWindow.graph.GRIDAxisChange(false, XMajorDash.SelectedIndex, 1);
            if (YMinor.IsChecked == true) MainWindow.graph.GRIDAxisChange(true, YMinorDash.SelectedIndex, 2);
            else MainWindow.graph.GRIDAxisChange(false, YMinorDash.SelectedIndex, 2);
            if (YMajor.IsChecked == true) MainWindow.graph.GRIDAxisChange(true, YMajorDash.SelectedIndex, 3);
            else MainWindow.graph.GRIDAxisChange(false, YMajorDash.SelectedIndex, 3);
        }
    }
}
