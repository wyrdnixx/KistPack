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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbFallScann = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnApplyNewBox = new System.Windows.Forms.Button();
            this.btnNextBox = new System.Windows.Forms.Button();
            this.btnCreateCharge = new System.Windows.Forms.Button();
            this.cbMandant = new System.Windows.Forms.ComboBox();
            this.dgvAkten = new System.Windows.Forms.DataGridView();
            this.btnNewCharge = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCharge = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbKiste = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAkten)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1179, 861);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbStatus);
            this.tabPage1.Controls.Add(this.tbFallScann);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnApplyNewBox);
            this.tabPage1.Controls.Add(this.btnNextBox);
            this.tabPage1.Controls.Add(this.btnCreateCharge);
            this.tabPage1.Controls.Add(this.cbMandant);
            this.tabPage1.Controls.Add(this.dgvAkten);
            this.tabPage1.Controls.Add(this.btnNewCharge);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbCharge);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbKiste);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1171, 835);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kiste packen";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbFallScann
            // 
            this.tbFallScann.Enabled = false;
            this.tbFallScann.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFallScann.Location = new System.Drawing.Point(401, 153);
            this.tbFallScann.Name = "tbFallScann";
            this.tbFallScann.Size = new System.Drawing.Size(261, 41);
            this.tbFallScann.TabIndex = 10;
            this.tbFallScann.TextChanged += new System.EventHandler(this.tbFallScann_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(319, 34);
            this.label3.TabIndex = 9;
            this.label3.Text = "Fallnummer Scannen:";
            // 
            // btnApplyNewBox
            // 
            this.btnApplyNewBox.Enabled = false;
            this.btnApplyNewBox.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyNewBox.Location = new System.Drawing.Point(705, 80);
            this.btnApplyNewBox.Name = "btnApplyNewBox";
            this.btnApplyNewBox.Size = new System.Drawing.Size(211, 41);
            this.btnApplyNewBox.TabIndex = 7;
            this.btnApplyNewBox.Text = "Verwenden";
            this.btnApplyNewBox.UseVisualStyleBackColor = true;
            this.btnApplyNewBox.Click += new System.EventHandler(this.btnApplyNewBox_Click);
            // 
            // btnNextBox
            // 
            this.btnNextBox.Enabled = false;
            this.btnNextBox.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextBox.Location = new System.Drawing.Point(3, 85);
            this.btnNextBox.Name = "btnNextBox";
            this.btnNextBox.Size = new System.Drawing.Size(211, 41);
            this.btnNextBox.TabIndex = 5;
            this.btnNextBox.Text = "Neue Kiste";
            this.btnNextBox.UseVisualStyleBackColor = true;
            this.btnNextBox.Click += new System.EventHandler(this.btnNextBox_Click);
            // 
            // btnCreateCharge
            // 
            this.btnCreateCharge.Enabled = false;
            this.btnCreateCharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateCharge.Location = new System.Drawing.Point(401, 16);
            this.btnCreateCharge.Name = "btnCreateCharge";
            this.btnCreateCharge.Size = new System.Drawing.Size(261, 41);
            this.btnCreateCharge.TabIndex = 3;
            this.btnCreateCharge.Text = "Charge Erzeugen";
            this.btnCreateCharge.UseVisualStyleBackColor = true;
            this.btnCreateCharge.Click += new System.EventHandler(this.btnCreateCharge_Click);
            // 
            // cbMandant
            // 
            this.cbMandant.Enabled = false;
            this.cbMandant.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMandant.FormattingEnabled = true;
            this.cbMandant.Items.AddRange(new object[] {
            "FN",
            "TT"});
            this.cbMandant.Location = new System.Drawing.Point(220, 18);
            this.cbMandant.Name = "cbMandant";
            this.cbMandant.Size = new System.Drawing.Size(175, 39);
            this.cbMandant.TabIndex = 2;
            this.cbMandant.SelectedIndexChanged += new System.EventHandler(this.cbMandant_SelectedIndexChanged);
            // 
            // dgvAkten
            // 
            this.dgvAkten.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAkten.Location = new System.Drawing.Point(3, 214);
            this.dgvAkten.Name = "dgvAkten";
            this.dgvAkten.Size = new System.Drawing.Size(1162, 615);
            this.dgvAkten.TabIndex = 5;
            // 
            // btnNewCharge
            // 
            this.btnNewCharge.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCharge.Location = new System.Drawing.Point(3, 18);
            this.btnNewCharge.Name = "btnNewCharge";
            this.btnNewCharge.Size = new System.Drawing.Size(211, 41);
            this.btnNewCharge.TabIndex = 1;
            this.btnNewCharge.Text = "Neue Charge";
            this.btnNewCharge.UseVisualStyleBackColor = true;
            this.btnNewCharge.Click += new System.EventHandler(this.btnNewCharge_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(668, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 33);
            this.label2.TabIndex = 3;
            this.label2.Text = "Chargennummer:";
            // 
            // tbCharge
            // 
            this.tbCharge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbCharge.Enabled = false;
            this.tbCharge.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCharge.Location = new System.Drawing.Point(874, 18);
            this.tbCharge.Name = "tbCharge";
            this.tbCharge.Size = new System.Drawing.Size(252, 41);
            this.tbCharge.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(220, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kistennummer:";
            // 
            // tbKiste
            // 
            this.tbKiste.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbKiste.Enabled = false;
            this.tbKiste.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbKiste.Location = new System.Drawing.Point(401, 81);
            this.tbKiste.MaxLength = 6;
            this.tbKiste.Name = "tbKiste";
            this.tbKiste.Size = new System.Drawing.Size(261, 41);
            this.tbKiste.TabIndex = 6;
            this.tbKiste.TextChanged += new System.EventHandler(this.tbKiste_TextChanged);
            this.tbKiste.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbKiste_KeyPress);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1171, 835);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Suche";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbStatus
            // 
            this.tbStatus.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStatus.Location = new System.Drawing.Point(705, 127);
            this.tbStatus.Multiline = true;
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(460, 81);
            this.tbStatus.TabIndex = 11;
            this.tbStatus.TabStop = false;
            this.tbStatus.Text = "Status";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 865);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "KistPack";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAkten)).EndInit();
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
        private System.Windows.Forms.Button btnCreateCharge;
        private System.Windows.Forms.TextBox tbFallScann;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbStatus;
    }
}

