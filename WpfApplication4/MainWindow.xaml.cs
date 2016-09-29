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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        Graph graphObj = new Graph();
        Style styleObj = new Style();
        Analysis analysisObj = new Analysis();

        private void graphButton_Click(object sender, RoutedEventArgs e)
        {
            styleButton.IsChecked = false;
            analysisButton.IsChecked = false;
            configStackPanel.Children.Remove(styleObj);
            configStackPanel.Children.Remove(analysisObj);

            if (graphButton.IsChecked == true)
            {
                configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(graphObj);
                return;
            }
            if (graphButton.IsChecked == false)
            {
                configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(graphObj);
                return;
            }
        }

        private void styleButton_Click(object sender, RoutedEventArgs e)
        {
            graphButton.IsChecked = false;
            analysisButton.IsChecked = false;
            configStackPanel.Children.Remove(graphObj);
            configStackPanel.Children.Remove(analysisObj);

            if (styleButton.IsChecked == true)
            {
                configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(styleObj);
                return;
            }
            if (styleButton.IsChecked == false)
            {
                configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(styleObj);
                return;
            }
        }

        private void analysisButton_Click(object sender, RoutedEventArgs e)
        {
            graphButton.IsChecked = false;
            styleButton.IsChecked = false;
            configStackPanel.Children.Remove(graphObj);
            configStackPanel.Children.Remove(styleObj);

            if (analysisButton.IsChecked == true)
            {
                configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(analysisObj);
                return;
            }
            if (analysisButton.IsChecked == false)
            {
                configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(analysisObj);
                return;
            }
        }
    }
}
