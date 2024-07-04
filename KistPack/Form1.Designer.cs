using System.Windows.Forms;

namespace KistPack
{
    partial class Form1
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cblMerkmale = new System.Windows.Forms.CheckedListBox();
            this.btnFinishCharge = new System.Windows.Forms.Button();
            this.btnDeleteEntry = new System.Windows.Forms.Button();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tbFallScann = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnApplyNewBox = new System.Windows.Forms.Button();
            this.btnNextBox = new System.Windows.Forms.Button();
            this.cbMandant = new System.Windows.Forms.ComboBox();
            this.dgvAkten = new System.Windows.Forms.DataGridView();
            this.btnNewCharge = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCharge = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbKiste = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRegenCSV = new System.Windows.Forms.Button();
            this.btnRegenPDF = new System.Windows.Forms.Button();
            this.dgvSearchResults = new System.Windows.Forms.DataGridView();
            this.tbSearchText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAkten)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1460, 865);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cblMerkmale);
            this.tabPage1.Controls.Add(this.btnFinishCharge);
            this.tabPage1.Controls.Add(this.btnDeleteEntry);
            this.tabPage1.Controls.Add(this.tbStatus);
            this.tabPage1.Controls.Add(this.tbFallScann);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnApplyNewBox);
            this.tabPage1.Controls.Add(this.btnNextBox);
            this.tabPage1.Controls.Add(this.cbMandant);
            this.tabPage1.Controls.Add(this.dgvAkten);
            this.tabPage1.Controls.Add(this.btnNewCharge);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbCharge);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbKiste);
            this.tabPage1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1452, 834);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kiste packen";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cblMerkmale
            // 
            this.cblMerkmale.CheckOnClick = true;
            this.cblMerkmale.FormattingEnabled = true;
            this.cblMerkmale.Location = new System.Drawing.Point(859, 134);
            this.cblMerkmale.Name = "cblMerkmale";
            this.cblMerkmale.Size = new System.Drawing.Size(585, 92);
            this.cblMerkmale.TabIndex = 14;
            this.cblMerkmale.SelectedIndexChanged += new System.EventHandler(this.clbMerkmale_SelectedIndexChanged);
            // 
            // btnFinishCharge
            // 
            this.btnFinishCharge.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinishCharge.Location = new System.Drawing.Point(8, 783);
            this.btnFinishCharge.Name = "btnFinishCharge";
            this.btnFinishCharge.Size = new System.Drawing.Size(1436, 37);
            this.btnFinishCharge.TabIndex = 13;
            this.btnFinishCharge.Text = "Charge abschließen";
            this.btnFinishCharge.UseVisualStyleBackColor = true;
            this.btnFinishCharge.Click += new System.EventHandler(this.btnFinishCharge_Click);
            // 
            // btnDeleteEntry
            // 
            this.btnDeleteEntry.BackColor = System.Drawing.Color.Wheat;
            this.btnDeleteEntry.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteEntry.Location = new System.Drawing.Point(3, 136);
            this.btnDeleteEntry.Name = "btnDeleteEntry";
            this.btnDeleteEntry.Size = new System.Drawing.Size(90, 30);
            this.btnDeleteEntry.TabIndex = 12;
            this.btnDeleteEntry.TabStop = false;
            this.btnDeleteEntry.Text = "Löschen";
            this.btnDeleteEntry.UseVisualStyleBackColor = false;
            this.btnDeleteEntry.Click += new System.EventHandler(this.btnDeleteEntry_Click);
            // 
            // tbStatus
            // 
            this.tbStatus.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStatus.Location = new System.Drawing.Point(859, 12);
            this.tbStatus.Multiline = true;
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(585, 104);
            this.tbStatus.TabIndex = 11;
            this.tbStatus.TabStop = false;
            this.tbStatus.Text = "Status";
            // 
            // tbFallScann
            // 
            this.tbFallScann.Enabled = false;
            this.tbFallScann.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFallScann.Location = new System.Drawing.Point(425, 134);
            this.tbFallScann.Name = "tbFallScann";
            this.tbFallScann.Size = new System.Drawing.Size(428, 37);
            this.tbFallScann.TabIndex = 10;
            this.tbFallScann.TextChanged += new System.EventHandler(this.tbFallScann_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(114, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(305, 29);
            this.label3.TabIndex = 9;
            this.label3.Text = "Fallnummer Scannen:";
            // 
            // btnApplyNewBox
            // 
            this.btnApplyNewBox.Enabled = false;
            this.btnApplyNewBox.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyNewBox.Location = new System.Drawing.Point(592, 79);
            this.btnApplyNewBox.Name = "btnApplyNewBox";
            this.btnApplyNewBox.Size = new System.Drawing.Size(261, 37);
            this.btnApplyNewBox.TabIndex = 7;
            this.btnApplyNewBox.Text = "Verwenden";
            this.btnApplyNewBox.UseVisualStyleBackColor = true;
            this.btnApplyNewBox.Click += new System.EventHandler(this.btnApplyNewBox_Click);
            // 
            // btnNextBox
            // 
            this.btnNextBox.Enabled = false;
            this.btnNextBox.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextBox.Location = new System.Drawing.Point(3, 79);
            this.btnNextBox.Name = "btnNextBox";
            this.btnNextBox.Size = new System.Drawing.Size(211, 37);
            this.btnNextBox.TabIndex = 5;
            this.btnNextBox.Text = "Neue Kiste";
            this.btnNextBox.UseVisualStyleBackColor = true;
            this.btnNextBox.Click += new System.EventHandler(this.btnNextBox_Click);
            // 
            // cbMandant
            // 
            this.cbMandant.Enabled = false;
            this.cbMandant.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMandant.FormattingEnabled = true;
            this.cbMandant.Items.AddRange(new object[] {
            "FN",
            "TT"});
            this.cbMandant.Location = new System.Drawing.Point(225, 17);
            this.cbMandant.Name = "cbMandant";
            this.cbMandant.Size = new System.Drawing.Size(123, 37);
            this.cbMandant.TabIndex = 2;
            this.cbMandant.SelectedIndexChanged += new System.EventHandler(this.cbMandant_SelectedIndexChanged);
            // 
            // dgvAkten
            // 
            this.dgvAkten.AllowUserToAddRows = false;
            this.dgvAkten.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAkten.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAkten.Location = new System.Drawing.Point(8, 232);
            this.dgvAkten.MultiSelect = false;
            this.dgvAkten.Name = "dgvAkten";
            this.dgvAkten.ReadOnly = true;
            this.dgvAkten.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAkten.ShowEditingIcon = false;
            this.dgvAkten.Size = new System.Drawing.Size(1436, 532);
            this.dgvAkten.TabIndex = 5;            
            //this.dgvAkten.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvAkten_CellMouseClick); // toDo : alte methode - test mir "AddContextMenu"
            

            // 
            // btnNewCharge
            // 
            this.btnNewCharge.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCharge.Location = new System.Drawing.Point(3, 18);
            this.btnNewCharge.Name = "btnNewCharge";
            this.btnNewCharge.Size = new System.Drawing.Size(211, 37);
            this.btnNewCharge.TabIndex = 1;
            this.btnNewCharge.Text = "Neue Charge";
            this.btnNewCharge.UseVisualStyleBackColor = true;
            this.btnNewCharge.Click += new System.EventHandler(this.btnNewCharge_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(356, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Charge:";
            // 
            // tbCharge
            // 
            this.tbCharge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbCharge.Enabled = false;
            this.tbCharge.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCharge.Location = new System.Drawing.Point(459, 12);
            this.tbCharge.Name = "tbCharge";
            this.tbCharge.Size = new System.Drawing.Size(394, 41);
            this.tbCharge.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(220, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kiste:";
            // 
            // tbKiste
            // 
            this.tbKiste.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbKiste.Enabled = false;
            this.tbKiste.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbKiste.Location = new System.Drawing.Point(309, 79);
            this.tbKiste.MaxLength = 8;
            this.tbKiste.Name = "tbKiste";
            this.tbKiste.Size = new System.Drawing.Size(277, 37);
            this.tbKiste.TabIndex = 6;
            this.tbKiste.TextChanged += new System.EventHandler(this.tbKiste_TextChanged);
            this.tbKiste.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbKiste_KeyPress);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.btnRegenCSV);
            this.tabPage2.Controls.Add(this.btnRegenPDF);
            this.tabPage2.Controls.Add(this.dgvSearchResults);
            this.tabPage2.Controls.Add(this.tbSearchText);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1452, 834);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Suche";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(387, 18);
            this.label5.TabIndex = 5;
            this.label5.Text = "! Es werden maximal 100 Einträge gelistet !";
            // 
            // btnRegenCSV
            // 
            this.btnRegenCSV.Location = new System.Drawing.Point(626, 6);
            this.btnRegenCSV.Name = "btnRegenCSV";
            this.btnRegenCSV.Size = new System.Drawing.Size(124, 78);
            this.btnRegenCSV.TabIndex = 4;
            this.btnRegenCSV.Text = "Chargen CSV neu erstellen";
            this.btnRegenCSV.UseVisualStyleBackColor = true;
            this.btnRegenCSV.Click += new System.EventHandler(this.btnRegenCSV_Click);
            // 
            // btnRegenPDF
            // 
            this.btnRegenPDF.Location = new System.Drawing.Point(496, 6);
            this.btnRegenPDF.Name = "btnRegenPDF";
            this.btnRegenPDF.Size = new System.Drawing.Size(124, 78);
            this.btnRegenPDF.TabIndex = 3;
            this.btnRegenPDF.Text = "Chargen PDF neu erstellen";
            this.btnRegenPDF.UseVisualStyleBackColor = true;
            this.btnRegenPDF.Click += new System.EventHandler(this.btnRegenPDF_Click);
            // 
            // dgvSearchResults
            // 
            this.dgvSearchResults.AllowUserToAddRows = false;
            this.dgvSearchResults.AllowUserToDeleteRows = false;
            this.dgvSearchResults.AllowUserToResizeRows = false;
            this.dgvSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSearchResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchResults.Location = new System.Drawing.Point(9, 90);
            this.dgvSearchResults.Name = "dgvSearchResults";
            this.dgvSearchResults.ReadOnly = true;
            this.dgvSearchResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvSearchResults.Size = new System.Drawing.Size(1437, 741);
            this.dgvSearchResults.TabIndex = 2;
            // 
            // tbSearchText
            // 
            this.tbSearchText.Location = new System.Drawing.Point(81, 22);
            this.tbSearchText.Name = "tbSearchText";
            this.tbSearchText.Size = new System.Drawing.Size(380, 27);
            this.tbSearchText.TabIndex = 1;
            this.tbSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearchText_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "Suche:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1460, 865);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "KistPack";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAkten)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbKiste;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCharge;
        private System.Windows.Forms.Button btnNewCharge;
        private System.Windows.Forms.DataGridView dgvAkten;
        private System.Windows.Forms.ComboBox cbMandant;
        private System.Windows.Forms.Button btnApplyNewBox;
        private System.Windows.Forms.Button btnNextBox;
        private System.Windows.Forms.TextBox tbFallScann;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Button btnDeleteEntry;
        private System.Windows.Forms.Button btnFinishCharge;
        private System.Windows.Forms.DataGridView dgvSearchResults;
        private System.Windows.Forms.TextBox tbSearchText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRegenPDF;
        private System.Windows.Forms.Button btnRegenCSV;
        private System.Windows.Forms.CheckedListBox cblMerkmale;
        private System.Windows.Forms.Label label5;
    }
}

