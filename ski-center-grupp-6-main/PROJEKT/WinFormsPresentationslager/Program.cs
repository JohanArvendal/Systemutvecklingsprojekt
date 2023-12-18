using SkiCenterKontroller;
using System;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoggaIn(new PersonalKontroller(), new BokningKontroller(), new KundKontroller(), new RumKontroller(), new UtrustningKontroller(), new LektionKontroller()));
        }
    }
}
