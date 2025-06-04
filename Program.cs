using System;
using System.Windows.Forms;

namespace MyFramelessApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyFramelessForm(900, 700));
        }
    }
}

