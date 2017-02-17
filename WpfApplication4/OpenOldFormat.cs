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
    public partial class OpenOldFormat : Form
    {
        public OpenOldFormat()
        {
            InitializeComponent();
        }

        public static double SampleRate;
        public static double HistoryPercent;

        private void OK_button_Click(object sender, EventArgs e)
        {
            if (KHz4_radioButton.Checked)
            {
                SampleRate = 4000;
            }
            else if (KHz2_radioButton.Checked)
            {
                SampleRate = 2000;
            }
            else if (KHz1_radioButton.Checked)
            {
                SampleRate = 1000;
            }

            if (percent75_radioButton.Checked)
            {
                HistoryPercent = 75;
            }
            else if (percent50_radioButton.Checked)
            {
                HistoryPercent = 50;
            }
            else if (percent25_radioButton.Checked)
            {
                HistoryPercent = 25;
            }
        }
    }
}
