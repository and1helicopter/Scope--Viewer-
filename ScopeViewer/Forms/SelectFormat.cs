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
    public partial class SelectFormat : Form
    {
        public SelectFormat()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if(radioButton2.Checked == true)
                this.DialogResult = DialogResult.No;
            else 
                this.DialogResult = DialogResult.OK;

        }
    }
}
