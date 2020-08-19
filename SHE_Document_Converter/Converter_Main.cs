using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SHE_Document_converter
{
    static class Converter_Main
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Converter());
        }
    }
}
