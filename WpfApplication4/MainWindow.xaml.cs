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
using System.Windows.Media.Animation;
using System.IO;
using Microsoft.Win32;
using ZedGraph;
using System.Windows.Forms.Integration;

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
        bool graphButtonStatus = false;
        bool styleButtonStatus = false;
        bool analysisButtonStatus = false;


        private void OpenAnimation()
        {
            DoubleAnimation OpenAnimation = new DoubleAnimation();
            OpenAnimation.From = 0;
            OpenAnimation.To = 170;
            OpenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            configPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, OpenAnimation);
        }
        private void CloseAnimation()
        {
            DoubleAnimation CloseAnimation = new DoubleAnimation();
            CloseAnimation.From = 170;
            CloseAnimation.To = 0;
            CloseAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            configPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, CloseAnimation);
        }

        private void resetColorGraphButton()
        {
            graphButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            graphButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            graphButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
        private void setColorGraphButton()
        {
            graphButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            graphButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            graphButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }
        private void resetColorStyleButton()
        {
            styleButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            styleButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            styleButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
        private void setColorStyleButton()
        {
            styleButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            styleButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            styleButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }
        private void resetColorAnalysisButton()
        {
            analysisButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            analysisButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            analysisButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
        private void setColorrAnalysisButton()
        {
            analysisButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            analysisButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            analysisButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }



        private void graphButton_Click(object sender, RoutedEventArgs e)
        {
            styleButtonStatus = false;
            analysisButtonStatus = false;
            configStackPanel.Children.Remove(styleObj);
            configStackPanel.Children.Remove(analysisObj);
            resetColorStyleButton();
            resetColorAnalysisButton();

            if (graphButtonStatus == false)
            {
                graphButtonStatus = true;
                setColorGraphButton();
                OpenAnimation();
                //configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(graphObj);
                return;
            }
            if (graphButtonStatus == true)
            {
                graphButtonStatus = false;
                resetColorGraphButton();
                CloseAnimation();
                //configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(graphObj);
                return;
            }
        }

        private void styleButton_Click(object sender, RoutedEventArgs e)
        {
            graphButtonStatus = false;
            analysisButtonStatus = false;
            configStackPanel.Children.Remove(graphObj);
            configStackPanel.Children.Remove(analysisObj);
            resetColorGraphButton();
            resetColorAnalysisButton();

            if (styleButtonStatus == false)
            {
                styleButtonStatus = true;
                setColorStyleButton();
                OpenAnimation();
                //configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(styleObj);
                return;
            }
            if (styleButtonStatus == true)
            {
                styleButtonStatus = false;
                resetColorStyleButton();
                CloseAnimation();
                //configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(styleObj);
                return;
            }
        }

        private void analysisButton_Click(object sender, RoutedEventArgs e)
        {
            graphButtonStatus = false;
            styleButtonStatus = false;
            configStackPanel.Children.Remove(graphObj);
            configStackPanel.Children.Remove(styleObj);
            resetColorGraphButton();
            resetColorStyleButton();

            if (analysisButtonStatus == false)
            {
                analysisButtonStatus = true;
                setColorrAnalysisButton();
                OpenAnimation();
                //configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(analysisObj);
                return;
            }
            if (analysisButtonStatus == true)
            {
                analysisButtonStatus = false;
                resetColorAnalysisButton();
                CloseAnimation();
                //configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(analysisObj);
                return;
            }
        }


        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Текстовый файл|*.txt|Comtrade|*.cfg|All files (*.*)|*.*"; // Filter files by extension
            if (ofd.ShowDialog() == true)
            {
                MessageBox.Show("OK");
                try
                {

                }
                catch
                {
                    return;
                }

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WpfApplication4.GraphPanel graph = new WpfApplication4.GraphPanel();
            graph.EnableZoom = false;
            GrPanel.Child = graph;
        }
    }
}
