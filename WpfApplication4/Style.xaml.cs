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
using ZedGraph;



namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Style.xaml
    /// </summary>
    public partial class Style : UserControl
    {
        List<DockPanel> LayoutPanel = new List<DockPanel>();
        List<bool> OpenClose = new List<bool>(); 
        System.Windows.Controls.Border panelBorder;
        System.Windows.Controls.Label nameLabel;
        CheckBox XMinor;
        CheckBox XMajor;
        CheckBox YMinor;
        CheckBox YMajor;
        ComboBox YMajorDash;
        ComboBox YMinorDash;
        ComboBox XMajorDash;
        ComboBox XMinorDash;

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
        }

        public void StyleGridConfigAdd()
        {
            int i;
            LayoutPanel.Add(new DockPanel());
            i = LayoutPanel.Count - 1;
            LayoutPanel[i].Width = 175;
            LayoutPanel[i].Height = 30;
            LayoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            LayoutPanel[i].Background = Brushes.White;
            LayoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanel);

            panelBorder = new System.Windows.Controls.Border();
            panelBorder.BorderBrush = Brushes.DarkGray;
            panelBorder.BorderThickness = new Thickness(1.0);
            panelBorder.Margin = new Thickness(-155, 0, 0, 0);

            nameLabel = new System.Windows.Controls.Label();
            nameLabel.Content = "Grid";
            nameLabel.VerticalAlignment = VerticalAlignment.Top;
            nameLabel.FontSize = 14;
            nameLabel.Height = 25;
            nameLabel.Width = 155;
            nameLabel.ToolTip = "Настройка сетки";
            nameLabel.Margin = new Thickness(0, 0, 0, 0);

            XMinor = new CheckBox();
            XMinor.VerticalAlignment = VerticalAlignment.Top;
            XMinor.Margin = new Thickness(-150, 25, 0, 0);
            XMinor.Content = "XMinor";
            XMinor.FontSize = 12;
            XMinor.Visibility = Visibility.Hidden;
            XMinor.Checked += new RoutedEventHandler(XMinor_Checked);
            XMinor.Unchecked += new RoutedEventHandler(XMinor_Checked);

            XMajor = new CheckBox();
            XMajor.VerticalAlignment = VerticalAlignment.Top;
            XMajor.Margin = new Thickness(-150, 50, 0, 0);
            XMajor.Content = "XMajor";
            XMajor.FontSize = 12;
            XMajor.Visibility = Visibility.Hidden;
            XMajor.Checked += new RoutedEventHandler(XMajor_Checked);
            XMajor.Unchecked += new RoutedEventHandler(XMajor_Checked);

            YMinor = new CheckBox();
            YMinor.VerticalAlignment = VerticalAlignment.Top;
            YMinor.Margin = new Thickness(-150, 75, 0, 0);
            YMinor.Content = "YMinor";
            YMinor.FontSize = 12;
            YMinor.Visibility = Visibility.Hidden;
            YMinor.Checked += new RoutedEventHandler(YMinor_Checked);
            YMinor.Unchecked += new RoutedEventHandler(YMinor_Checked);

            YMajor = new CheckBox();
            YMajor.VerticalAlignment = VerticalAlignment.Top;
            YMajor.Margin = new Thickness(-150, 100, 0, 0);
            YMajor.Content = "YMajor";
            YMajor.FontSize = 12;
            YMajor.Visibility = Visibility.Hidden;
            YMajor.Checked += new RoutedEventHandler(YMajor_Checked);
            YMajor.Unchecked += new RoutedEventHandler(YMajor_Checked);


            XMinorDash = new ComboBox();
            XMinorDash.VerticalAlignment = VerticalAlignment.Top;
            XMinorDash.Width = 80;
            XMinorDash.ItemsSource = StyleTypeMinor;
            XMinorDash.SelectedIndex = 0;
            XMinorDash.Margin = new Thickness(-80, 15, 0, 0);
            XMinorDash.FontSize = 12;
            XMinorDash.Visibility = Visibility.Hidden;
            XMinorDash.SelectionChanged += new SelectionChangedEventHandler(SelectionXMinorDash);

            XMajorDash = new ComboBox();
            XMajorDash.VerticalAlignment = VerticalAlignment.Top;
            XMajorDash.Width = 80;
            XMajorDash.ItemsSource = StyleTypeMajor;
            XMajorDash.SelectedIndex = 0;
            XMajorDash.Margin = new Thickness(-80, 40, 0, 0);
            XMajorDash.FontSize = 12;
            XMajorDash.Visibility = Visibility.Hidden;
            XMajorDash.SelectionChanged += new SelectionChangedEventHandler(SelectionXMajorDash);

            YMinorDash = new ComboBox();
            YMinorDash.VerticalAlignment = VerticalAlignment.Top;
            YMinorDash.Width = 80;
            YMinorDash.ItemsSource = StyleTypeMinor;
            YMinorDash.SelectedIndex = 0;
            YMinorDash.Margin = new Thickness(-80, 65, 0, 0);
            YMinorDash.FontSize = 12;
            YMinorDash.Visibility = Visibility.Hidden;
            YMinorDash.SelectionChanged += new SelectionChangedEventHandler(SelectionYMinorDash);

            YMajorDash = new ComboBox();
            YMajorDash.VerticalAlignment = VerticalAlignment.Top;
            YMajorDash.Width = 80;
            YMajorDash.ItemsSource = StyleTypeMajor;
            YMajorDash.SelectedIndex = 0;
            YMajorDash.Margin = new Thickness(-80, 90, 0, 0);
            YMajorDash.FontSize = 12;
            YMajorDash.Visibility = Visibility.Hidden;
            YMajorDash.SelectionChanged += new SelectionChangedEventHandler(SelectionYMajorDash);

            OpenClose.Add(new bool());
            OpenClose[i] = false;

            LayoutPanel[i].Children.Add(nameLabel);
            LayoutPanel[i].Children.Add(XMinor);
            LayoutPanel[i].Children.Add(XMajor);
            LayoutPanel[i].Children.Add(YMinor);
            LayoutPanel[i].Children.Add(YMajor);

            LayoutPanel[i].Children.Add(XMinorDash);
            LayoutPanel[i].Children.Add(XMajorDash);
            LayoutPanel[i].Children.Add(YMinorDash);
            LayoutPanel[i].Children.Add(YMajorDash);

            LayoutPanel[i].Children.Add(panelBorder);

            StyleStackPanel.Children.Add(LayoutPanel[i]);
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
        }
        private void CloseAnimation(int i)
        {
            DoubleAnimation CloseAnimation = new DoubleAnimation();
            CloseAnimation.From = 125;
            CloseAnimation.To = 30;
            CloseAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            LayoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, CloseAnimation);
            OpenClose[i] = false;

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
        }

        public void XMinor_Checked(object sender, RoutedEventArgs e)
        {
            if (XMinor.IsChecked == true)
            {
                MainWindow.graph.XMinorGRIDAxisChange(true);
            }
            else MainWindow.graph.XMinorGRIDAxisChange(false);
        }
        
        private void XMajor_Checked(object sender, RoutedEventArgs e)
        {
            if (XMajor.IsChecked == true)
            {
                MainWindow.graph.XMajorGRIDAxisChange(true);
            }
            else MainWindow.graph.XMajorGRIDAxisChange(false);
        }
        
        public void YMinor_Checked(object sender, RoutedEventArgs e)
        {
            if (YMinor.IsChecked == true)
            {
                MainWindow.graph.YMinorGRIDAxisChange(true);
            }
            else MainWindow.graph.YMinorGRIDAxisChange(false);
        }

        private void YMajor_Checked(object sender, RoutedEventArgs e)
        {
            if (YMajor.IsChecked == true)
            {
                MainWindow.graph.YMajorGRIDAxisChange(true);
            }
            else MainWindow.graph.YMajorGRIDAxisChange(false);
        }
        
         private void SelectionYMajorDash(object sender, SelectionChangedEventArgs e)
         {
             if (YMajor.IsChecked == true)
             {
                 MainWindow.graph.AxisDash(YMajorDash.SelectedIndex, 0);
             }
         }

         private void SelectionYMinorDash(object sender, SelectionChangedEventArgs e)
         {
             if (YMinor.IsChecked == true)
             {
                 MainWindow.graph.AxisDash(YMinorDash.SelectedIndex, 1);
             }
         }

         private void SelectionXMajorDash(object sender, SelectionChangedEventArgs e)
         {
             if (XMajor.IsChecked == true)
             {
                 MainWindow.graph.AxisDash(XMajorDash.SelectedIndex, 2);
             }
         }

        private void SelectionXMinorDash(object sender, SelectionChangedEventArgs e)
        {
            if (XMinor.IsChecked == true)
            {
                MainWindow.graph.AxisDash(XMinorDash.SelectedIndex, 3);
            }
        }
    }
}
