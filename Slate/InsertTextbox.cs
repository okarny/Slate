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
    public partial class InsertTextbox : Form
    {
        public InsertTextbox()
        {
            InitializeComponent();
        }

        private void btnChooseFont_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                txtFont.Text = fontDialog.Font.ToString();
            }
        }
    }
}
