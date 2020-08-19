using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SHE_Document_converter.User_Controls;
using Database;

namespace SHE_Document_converter
{
    public partial class Converter : Form
    {
        private readonly Dictionary<string, string> _sqls;
        private BelegConverter _bc;
        private LogViewer _lw;
        private RecoveryOriginal _ro;
        private CheckForOriginal _co;
        private CheckWithoutOriginal _cwo;
        private CovertAgain _ca;
        private RepairBelegSeiten _rbs;

        public Converter()
        {
            InitializeComponent();
            _sqls = ReadXml.ReadSqlFomrXML("SQL_Converter.xml");
            foreach (KeyValuePair<string, string> sql in _sqls)
            {
                Console.WriteLine(@"Function, SQL: {0}, {1}", sql.Key, sql.Value);
            }

            if (InitDatabase() == false)
                return;

            InitTreeview();
            InitUserControls();

        }

        private bool InitDatabase()
        {
            string connectConfig = ReadSettings.ReadAppSettings("Synios.ConfigDatabase.ConnectionString");
            string connectData = ReadSettings.ReadAppSettings("Synios.DataDatabase.ConnectionString");

            if (Database_Data.Instance.InitializedLayers(connectData) == false)
            {
                FileLogger.FileLogger.Instance.WriteMessage("Fehler beim Versucht eine Verbindung zur Config Datenbank herzustellen");
                return false;
            }

            if (Database_Config.Instance.InitializedLayers(connectConfig) == false)
            {
                FileLogger.FileLogger.Instance.WriteMessage("Fehler beim Versucht eine Verbindung zur Data Datenbank herzustellen");
                return false;
            }

            return true;

        }

        private void InitUserControls()
        {
            //Konvertiert das Original in Images und date DynBeleg up
            _bc = new BelegConverter(_sqls["TO_CONVERT"]) {Dock = DockStyle.Fill};
            panel.Controls.Add(_bc);

            //Dyn_Beleg: Erstellt der Original Wert wieder her (Original == null, Anzall in der Regel =1)  
            _ro = new RecoveryOriginal(_sqls["RECOVER_ORIGINAL"]) {Dock = DockStyle.Fill};
            panel.Controls.Add(_ro);

            //Dyn_Beleg wird repariert (Original + Anzahl der Seite von Konvertierte Dokument)
            _co = new CheckForOriginal(_sqls["CHECK_DATA"]) {Dock = DockStyle.Fill};
            panel.Controls.Add(_co);

            //Dyn_Beleg:  setzt Seite = Anzahl der Seite von Konvertierte Dokument
            _cwo = new CheckWithoutOriginal(_sqls["CHECK_DATA_WIHTOUT_ORIGINAL"]) { Dock = DockStyle.Fill };
            panel.Controls.Add(_cwo);

            //
            _ca = new CovertAgain(_sqls["CONVERT_AGAIN"]) { Dock = DockStyle.Fill };
            panel.Controls.Add(_ca);

            _rbs = new RepairBelegSeiten(_sqls["REPAIR_BELEGSEITEN"]) {Dock = DockStyle.Fill};
            panel.Controls.Add(_rbs);

            _lw = new LogViewer {Dock = DockStyle.Fill};
            panel.Controls.Add(_lw);


            panel.Controls[0].Show();
        }

        private void InitTreeview()
        {
            var convert = new TreeNode {Text = @"Konvertieren mit eintrag im DB"};
            tw.Nodes.Add(convert);

            var original = new TreeNode {Text = @"Original wiederherstellen"};
            tw.Nodes.Add(original);

            var checkOriginal = new TreeNode {Text = @"Check Ablegecode für original"};
            tw.Nodes.Add(checkOriginal);

            var check = new TreeNode { Text = @"Check Ablegecode ohne original" };
            tw.Nodes.Add(check);

            var again = new TreeNode { Text = @"Konverieren ohne Eintrag" };
            tw.Nodes.Add(again);

            var repair = new TreeNode { Text = @"BelegSeiten reparieren" };
            tw.Nodes.Add(repair);

            var log = new TreeNode {Text = @"Ereignisprotokoll"};
            tw.Nodes.Add(log);
        }

        private void tw_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (Control c in panel.Controls)
                c.Hide();

            var node = tw.SelectedNode;
            if (string.Compare(node.Text, "Ereignisprotokoll", StringComparison.Ordinal) == 0)
            {
                panel.Controls[6].Show();
            }
            else if (string.Compare(node.Text, "BelegSeiten reparieren", StringComparison.Ordinal) == 0)
            {
                panel.Controls[5].Show();
            }
            else if (string.Compare(node.Text, "Konverieren ohne Eintrag", StringComparison.Ordinal) == 0)
            {
                panel.Controls[4].Show();
            }
            else if (string.Compare(node.Text, "Check Ablegecode ohne original", StringComparison.Ordinal) == 0)
            {
                panel.Controls[3].Show();
            }
            else if (string.Compare(node.Text, "Check Ablegecode für original", StringComparison.Ordinal) == 0)
            {
                panel.Controls[2].Show();
            }
            else if (string.Compare(node.Text, "Original wiederherstellen", StringComparison.Ordinal) == 0)
            {
                panel.Controls[1].Show();
            }
            else if (string.Compare(node.Text, "Konvertieren mit eintrag im DB", StringComparison.Ordinal) == 0)
            {
                panel.Controls[0].Show();
            }
        }
    }
}
