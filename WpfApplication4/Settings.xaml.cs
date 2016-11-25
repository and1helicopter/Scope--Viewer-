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
using System.Windows.Shapes;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void PointPerChannel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.Graph.PointPerChannel(Convert.ToInt32(PointPerChannelTextBox.Text));
            }
            catch { MessageBox.Show("Неправильно введены данные"); }
        }

        public void UpdatePointPerChannelTextBox()
        {
            PointPerChannelTextBox.Text = Convert.ToString(GraphPanel.PointCashCount);
        }
    }
}
