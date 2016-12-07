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

        string[] _styleTypeMajor = new string[] {
            "Dot",
            "Dash",
            "Solid"
        };

        string[] _styleTypeMinor = new string[] {
            "Dot",
            "Dash"
        };

        public Settings()
        {
            InitializeComponent();
        }

        public void UpdatePointPerChannelTextBox()
        {
            PointPerChannelTextBox.Text = Convert.ToString(GraphPanel.PointCashCount);
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            saveSetting();
            updateSetting();
        }
        private void updateSetting()
        {
            try
            {
                MainWindow.Graph.PointPerChannel(Convert.ToInt32(PointPerChannelTextBox.Text));
            }
            catch { MessageBox.Show("Неправильно введены данные"); }
        }
        private void saveSetting()
        {

        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {     
            tavControl.SelectedIndex = ((TreeViewItem)e.NewValue).TabIndex;
        }
    }
}
