namespace IntersectionTest
{
    partial class frmInterval
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
            this.txtCN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cmdStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCN
            // 
            this.txtCN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCN.Location = new System.Drawing.Point(121, 17);
            this.txtCN.Name = "txtCN";
            this.txtCN.Size = new System.Drawing.Size(536, 20);
            this.txtCN.TabIndex = 4;
            this.txtCN.Text = "Server=localhost;Database=WQ3;Trusted_Connection=True;MultipleActiveResultSets=tr" +
    "ue;";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Connection string:";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(16, 95);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(641, 127);
            this.txtLog.TabIndex = 8;
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(16, 55);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(186, 34);
            this.cmdStart.TabIndex = 7;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // frmInterval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 234);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.txtCN);
            this.Controls.Add(this.label1);
            this.Name = "frmInterval";
            this.Text = "Расчет интервалов";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button cmdStart;
    }
}