using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace SHE_Document_converter.User_Controls
{
    public partial class LogViewer : UserControl
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        public void LoadInfoLOG()
        {
            tbLog.Clear();
            tbLog.Text = FileLogger.FileLogger.Instance.ReadLog();
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
            tbLog.Text = FileLogger.FileLogger.Instance.ReadLog();
        }
    }
}
