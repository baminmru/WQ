namespace IntersectionTest
{
    partial class frmMatrixBuilder
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
            this.cmdSelectFolder = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fb = new System.Windows.Forms.FolderBrowserDialog();
            this.txtCN = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdSelectFolder
            // 
            this.cmdSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSelectFolder.Location = new System.Drawing.Point(566, 25);
            this.cmdSelectFolder.Name = "cmdSelectFolder";
            this.cmdSelectFolder.Size = new System.Drawing.Size(35, 28);
            this.cmdSelectFolder.TabIndex = 8;
            this.cmdSelectFolder.Text = "...";
            this.cmdSelectFolder.UseVisualStyleBackColor = true;
            this.cmdSelectFolder.Click += new System.EventHandler(this.cmdSelectFolder_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(121, 30);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(439, 20);
            this.txtFolder.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Save to folder:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Connection string:";
            // 
            // txtCN
            // 
            this.txtCN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCN.Location = new System.Drawing.Point(120, 0);
            this.txtCN.Name = "txtCN";
            this.txtCN.Size = new System.Drawing.Size(481, 20);
            this.txtCN.TabIndex = 9;
            this.txtCN.Text = "Server=localhost;Database=WQ3;Trusted_Connection=True;MultipleActiveResultSets=tr" +
    "ue;";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 36);
            this.button1.TabIndex = 10;
            this.button1.Text = "Dayly";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 107);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(135, 36);
            this.button2.TabIndex = 12;
            this.button2.Text = "Winter all";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 146);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(135, 36);
            this.button3.TabIndex = 13;
            this.button3.Text = "Summer all";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(163, 146);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(135, 36);
            this.button5.TabIndex = 16;
            this.button5.Text = "Summer working";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(163, 107);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(135, 36);
            this.button6.TabIndex = 15;
            this.button6.Text = "Winter working";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(315, 146);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(135, 36);
            this.button7.TabIndex = 18;
            this.button7.Text = "Summer holyday";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(315, 107);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(135, 36);
            this.button8.TabIndex = 17;
            this.button8.Text = "Winter holiday";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(318, 249);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(135, 36);
            this.button9.TabIndex = 24;
            this.button9.Text = "Times Summer holyday";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(318, 210);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(135, 36);
            this.button10.TabIndex = 23;
            this.button10.Text = "Times Winter holiday";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(166, 249);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(135, 36);
            this.button11.TabIndex = 22;
            this.button11.Text = "Times Summer working";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(166, 210);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(135, 36);
            this.button12.TabIndex = 21;
            this.button12.Text = "Times Winter working";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(15, 249);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(135, 36);
            this.button13.TabIndex = 20;
            this.button13.Text = "Times Summer all";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(15, 210);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(135, 36);
            this.button14.TabIndex = 19;
            this.button14.Text = "Times Winter all";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // frmMatrixBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 340);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtCN);
            this.Controls.Add(this.cmdSelectFolder);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmMatrixBuilder";
            this.Text = "frmMatrixBuilder";
            this.Load += new System.EventHandler(this.frmMatrixBuilder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdSelectFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog fb;
        private System.Windows.Forms.TextBox txtCN;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
    }
}