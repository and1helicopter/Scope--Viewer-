using System;
using System.Windows.Forms;

namespace ScopeViewer
{
    public partial class OpenOldFormat : Form
    {
        public OpenOldFormat(double sampleRate)
        {
            InitializeComponent();
            try
            {
                switch (Convert.ToInt32(sampleRate))
                {
                    case 4000:
                        {
                            KHz4_radioButton.Checked = true;
                            groupBox1.Enabled = false;
                            SampleRate = 4000;
                        }
                        break;
                    case 2000:
                        {
                            KHz2_radioButton.Checked = true;
                            groupBox1.Enabled = false;
                            SampleRate = 2000;
                        }
                        break;
                    case 1000:
                        {
                            KHz1_radioButton.Checked = true;
                            groupBox1.Enabled = false;
                            SampleRate = 1000;
                        }
                        break;
                }
            }
            catch
            {
                groupBox1.Enabled = true;
            }


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
