using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
