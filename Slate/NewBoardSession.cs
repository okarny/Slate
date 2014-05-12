using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Slate
{
    public partial class NewBoardSession : Form
    {
        public NewBoardSession()
        {
            InitializeComponent();
        }

        private void chkPasswordProtect_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.Enabled = (chkPasswordProtect.Enabled);
        }
    }
}
