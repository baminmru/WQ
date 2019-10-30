namespace IntersectionTest
{
    partial class frmBatch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.fb = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCN = new System.Windows.Forms.TextBox();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.cmdSelectFolder = new System.Windows.Forms.Button();
            this.cmdStart = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWildcard = new System.Windows.Forms.TextBox();
            this.chkReorg = new System.Windows.Forms.CheckBox();
            this.txtSaveTo = new System.Windows.Forms.TextBox();
            this.chkSubFolder = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection string:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Data folder:";
            // 
            // txtCN
            // 
            this.txtCN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCN.Location = new System.Drawing.Point(122, 11);
            this.txtCN.Name = "txtCN";
            this.txtCN.Size = new System.Drawing.Size(433, 20);
            this.txtCN.TabIndex = 2;
            this.txtCN.Text = "Server=localhost;Database=WQ;Trusted_Connection=True;MultipleActiveResultSets=tru" +
    "e;";
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(122, 37);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(390, 20);
            this.txtFolder.TabIndex = 3;
            // 
            // cmdSelectFolder
            // 
            this.cmdSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSelectFolder.Location = new System.Drawing.Point(519, 32);
            this.cmdSelectFolder.Name = "cmdSelectFolder";
            this.cmdSelectFolder.Size = new System.Drawing.Size(35, 28);
            this.cmdSelectFolder.TabIndex = 4;
            this.cmdSelectFolder.Text = "...";
            this.cmdSelectFolder.UseVisualStyleBackColor = true;
            this.cmdSelectFolder.Click += new System.EventHandler(this.cmdSelectFolder_Click);
            // 
            // cmdStart
            // 
            this.cmdStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdStart.Location = new System.Drawing.Point(360, 101);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(186, 34);
            this.cmdStart.TabIndex = 5;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(16, 145);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(538, 154);
            this.txtLog.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Wildcard:";
            // 
            // txtWildcard
            // 
            this.txtWildcard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWildcard.Location = new System.Drawing.Point(122, 109);
            this.txtWildcard.Name = "txtWildcard";
            this.txtWildcard.Size = new System.Drawing.Size(232, 20);
            this.txtWildcard.TabIndex = 8;
            this.txtWildcard.Text = "*.zip";
            // 
            // chkReorg
            // 
            this.chkReorg.AutoSize = true;
            this.chkReorg.Location = new System.Drawing.Point(122, 65);
            this.chkReorg.Name = "chkReorg";
            this.chkReorg.Size = new System.Drawing.Size(102, 17);
            this.chkReorg.TabIndex = 9;
            this.chkReorg.Text = "Reorganize only";
            this.chkReorg.UseVisualStyleBackColor = true;
            // 
            // txtSaveTo
            // 
            this.txtSaveTo.Location = new System.Drawing.Point(243, 66);
            this.txtSaveTo.Name = "txtSaveTo";
            this.txtSaveTo.Size = new System.Drawing.Size(302, 20);
            this.txtSaveTo.TabIndex = 10;
            this.txtSaveTo.Text = "c:\\data2019\\";
            // 
            // chkSubFolder
            // 
            this.chkSubFolder.AutoSize = true;
            this.chkSubFolder.Location = new System.Drawing.Point(13, 64);
            this.chkSubFolder.Name = "chkSubFolder";
            this.chkSubFolder.Size = new System.Drawing.Size(74, 17);
            this.chkSubFolder.TabIndex = 11;
            this.chkSubFolder.Text = "SubFolder";
            this.chkSubFolder.UseVisualStyleBackColor = true;
            // 
            // frmBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 311);
            this.Controls.Add(this.chkSubFolder);
            this.Controls.Add(this.txtSaveTo);
            this.Controls.Add(this.chkReorg);
            this.Controls.Add(this.txtWildcard);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.cmdSelectFolder);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.txtCN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(550, 350);
            this.Name = "frmBatch";
            this.Text = "Пакетная обработка";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FolderBrowserDialog fb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCN;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button cmdSelectFolder;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWildcard;
        private System.Windows.Forms.CheckBox chkReorg;
        private System.Windows.Forms.TextBox txtSaveTo;
        private System.Windows.Forms.CheckBox chkSubFolder;
    }
}