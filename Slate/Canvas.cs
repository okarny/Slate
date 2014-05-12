using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Slate
{
    public partial class Canvas : Panel
    {
        public Canvas()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.BackColor = Color.White;

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
