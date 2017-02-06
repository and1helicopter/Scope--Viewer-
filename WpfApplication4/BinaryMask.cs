using System;
using System.Windows.Forms;

namespace ScopeViewer
{
    public partial class BinaryMask : Form
    {
        public BinaryMask()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        public static int BinnaryMask;

        private void OK_button_Click(object sender, EventArgs e)
        {
            BinnaryMask = comboBox1.SelectedIndex;
        }
    }
}
