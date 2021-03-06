﻿namespace SHE_Document_converter.User_Controls
{
    partial class RecoveryOriginal
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btConverter = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbSQL = new System.Windows.Forms.TextBox();
            this.btconnect = new System.Windows.Forms.Button();
            this.dgw = new System.Windows.Forms.DataGridView();
            this.lbInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgw)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btConverter, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btCancel, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(545, 33);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(94, 228);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btConverter
            // 
            this.btConverter.Dock = System.Windows.Forms.DockStyle.Right;
            this.btConverter.Location = new System.Drawing.Point(3, 46);
            this.btConverter.Name = "btConverter";
            this.btConverter.Size = new System.Drawing.Size(88, 24);
            this.btConverter.TabIndex = 1;
            this.btConverter.Text = "Restaurieren";
            this.btConverter.UseVisualStyleBackColor = true;
            this.btConverter.Click += new System.EventHandler(this.btConverter_Click);
            // 
            // btCancel
            // 
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btCancel.Location = new System.Drawing.Point(3, 76);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(88, 24);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Abbrechen";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tbSQL, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btconnect, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgw, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbInfo, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(642, 284);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tbSQL
            // 
            this.tbSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSQL.Location = new System.Drawing.Point(3, 3);
            this.tbSQL.Name = "tbSQL";
            this.tbSQL.Size = new System.Drawing.Size(536, 20);
            this.tbSQL.TabIndex = 0;
            // 
            // btconnect
            // 
            this.btconnect.Dock = System.Windows.Forms.DockStyle.Right;
            this.btconnect.Location = new System.Drawing.Point(545, 3);
            this.btconnect.Name = "btconnect";
            this.btconnect.Size = new System.Drawing.Size(94, 24);
            this.btconnect.TabIndex = 1;
            this.btconnect.Text = "Verbinden";
            this.btconnect.UseVisualStyleBackColor = true;
            this.btconnect.Click += new System.EventHandler(this.btconnect_Click);
            // 
            // dgw
            // 
            this.dgw.AllowUserToAddRows = false;
            this.dgw.AllowUserToDeleteRows = false;
            this.dgw.AllowUserToResizeRows = false;
            this.dgw.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgw.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgw.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgw.Location = new System.Drawing.Point(3, 33);
            this.dgw.MultiSelect = false;
            this.dgw.Name = "dgw";
            this.dgw.ReadOnly = true;
            this.dgw.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgw.Size = new System.Drawing.Size(536, 228);
            this.dgw.TabIndex = 2;
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbInfo.Location = new System.Drawing.Point(3, 264);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(536, 13);
            this.lbInfo.TabIndex = 4;
            this.lbInfo.Text = "Aktuelle Beleg (ID):  (0 von 0)";
            // 
            // RecoveryOriginal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RecoveryOriginal";
            this.Size = new System.Drawing.Size(642, 284);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btConverter;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbSQL;
        private System.Windows.Forms.Button btconnect;
        private System.Windows.Forms.DataGridView dgw;
        private System.Windows.Forms.Label lbInfo;
    }
}
