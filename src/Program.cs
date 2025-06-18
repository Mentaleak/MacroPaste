using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroCopyPaste
{
    internal static class Program
    {
        private static Mutex mutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isNewInstance;
            mutex = new Mutex(true, "MacroCopyPasteApp", out isNewInstance);

            if (!isNewInstance)
            {
                MessageBox.Show("Another instance of the application is already running.", "Instance Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create TrayAppContext with an initial delay value
            TrayAppContext trayAppContext = new TrayAppContext(3); // Default delay value (e.g., 5 seconds)

            // Pass TrayAppContext to Form1
            Form1 form = new Form1(trayAppContext);
            form.ShowDialog();

            Application.Run(trayAppContext);
        }
    }
}