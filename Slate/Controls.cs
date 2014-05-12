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
    public partial class Controls : Form
    {
        public frmMain parent;

        public Controls()
        {
            InitializeComponent();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            parent.nextPage();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            parent.previousPage();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            parent.lastPage();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            parent.firstPage();
        }

        private void txtGoto_Leave(object sender, EventArgs e)
        {
            short number = 0;
            if (Int16.TryParse(txtGoto.Text, out number))
            {
                //textBox value is a number
                parent.gotoPage(number);
            }
            else
            {
                //not a number
                MessageBox.Show("Please insert correct value for weight.");
            }
        }
    }
}
