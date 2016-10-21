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
using ZedGraph;



namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Style.xaml
    /// </summary>
    public partial class Style : UserControl
    {
        public Style()
        {
            InitializeComponent();
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
    }
}
