//#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Slate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            #if DEBUG 
                Application.Run(new frmMain());
            #else
                Application.Run(new Welcome());
            #endif
        }
    }
}
