using System;
using System.Data;
using System.Windows.Forms;
using Database;
using System.Threading;
using Converter;


namespace SHE_Document_converter.User_Controls
{
    public partial class BelegConverter : UserControl
    {
        private Thread _coverterThread;
        private volatile bool _shouldStop = true;

        delegate void SetDataTableCallback(DataTable table);
        delegate void SetLabelTextCallback(string id, int currentnumber, int totalNumber);
        private SetLabelTextCallback _slCall;

        public BelegConverter(string sql)
        {
            InitializeComponent();
            tbSQL.Text = sql;
        }

        private void btconnect_Click(object sender, EventArgs e)
        {
            GetBelege();
        }

        private void GetBelege()
        {
            DataTable table = Database_Data.Instance.ExecuteQuery(tbSQL.Text);
            if (table == null || table.Rows.Count == 0)
            {
                FileLogger.FileLogger.Instance.WriteMessage("Für die SQL Abfrage wurden kein Ergebnisse gefunden");
                return;
            }

            InitGrid(table);

        }

        private void InitGrid(DataTable table)
        {
            if (dgw.InvokeRequired)
            {
                SetDataTableCallback d = InitGrid;
                Invoke(d, table);
            }
            else
            {
                table.Columns.Add(new DataColumn("Konvertiert", typeof(bool)));

                foreach (DataRow row in table.Rows)
                {
                    row["Konvertiert"] = false;
                }

                table.AcceptChanges();

                dgw.DataSource = null;
                dgw.DataSource = table;
                dgw.ColumnHeadersVisible = true;
                // Resize the DataGridView columns to fit the newly loaded content.
                dgw.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                SetLabelText("0", 0, table.Rows.Count);
            }

            
        }

        private void btConverter_Click(object sender, EventArgs e)
        {
            StartConverter();
        }

        private void StartConverter()
        {
            if (dgw.Rows.Count == 0)
                return;

            btconnect.Enabled = false;
            btConverter.Enabled = false;
            _shouldStop = false;
            _coverterThread = new Thread(CallConverter);
            _coverterThread.Start();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            btConverter.Enabled = true;
            btconnect.Enabled = true;
            _shouldStop = true;
        }

        private void CallConverter()
        {
            int index = 0;
            foreach (DataGridViewRow row in dgw.Rows)
            {
                if (_shouldStop == true)
                    break;

                ++index;
                SetLabelText(row.Cells["Beleg_ID"].Value.ToString(), index, dgw.Rows.Count);

                try
                {
                    EConverterStatus status = SheConverter.Instance.ConvertFilesIfNeeded((int)row.Cells["Beleg_ID"].Value, row.Cells["Ablagecode"].Value.ToString());

                    if (status == EConverterStatus.ConverterError)
                    {
                        FileLogger.FileLogger.Instance.WriteMessage(
                            $"Beleg könnte nicht konvertiert werden (Beleg_ID ={row.Cells["Beleg_ID"].Value} )");
                    }
                    else if (status == EConverterStatus.Converted)
                    {
                        row.Cells["Konvertiert"].Value = true;
                        FileLogger.FileLogger.Instance.WriteMessage(
                            $"Beleg wurde konvertiert (Beleg_ID ={row.Cells["Beleg_ID"].Value} )");
                    }
                    else
                    {
                        FileLogger.FileLogger.Instance.WriteMessage(
                            $"Keine passendne Dokumenten gefuden (Beleg_ID ={row.Cells["Beleg_ID"].Value} )");
                    }
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    FileLogger.FileLogger.Instance.WriteExeption(e);
                    Console.WriteLine(e);
                }
                
            }
            _coverterThread = null;
            SetLabelText("Completed", index, dgw.Rows.Count);
        }

        private void SetLabelText(string id, int currentnumber, int totalNumber)
        {
            if (lbInfo.InvokeRequired)
            {
                if(_slCall == null)
                    _slCall = SetLabelText;

                Invoke(_slCall, id, currentnumber, totalNumber);
            }
            else
                lbInfo.Text = $"Aktuelle Beleg (ID): {id} ({currentnumber} von {totalNumber})";
          
        }

        private void tbSQL_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgw_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
