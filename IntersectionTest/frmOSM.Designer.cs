namespace IntersectionTest
{
    partial class frmOSM
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
            this.txtOUt = new System.Windows.Forms.TextBox();
            this.cmdLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtOUt
            // 
            this.txtOUt.Location = new System.Drawing.Point(12, 57);
            this.txtOUt.Multiline = true;
            this.txtOUt.Name = "txtOUt";
            this.txtOUt.ReadOnly = true;
            this.txtOUt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOUt.Size = new System.Drawing.Size(783, 390);
            this.txtOUt.TabIndex = 0;
            // 
            // cmdLoad
            // 
            this.cmdLoad.Location = new System.Drawing.Point(16, 4);
            this.cmdLoad.Name = "cmdLoad";
            this.cmdLoad.Size = new System.Drawing.Size(209, 47);
            this.cmdLoad.TabIndex = 1;
            this.cmdLoad.Text = "Загрузить";
            this.cmdLoad.UseVisualStyleBackColor = true;
            this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
            // 
            // frmOSM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cmdLoad);
            this.Controls.Add(this.txtOUt);
            this.Name = "frmOSM";
            this.Text = "Получение информации из OSM";
            this.Load += new System.EventHandler(this.frmOSM_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOUt;
        private System.Windows.Forms.Button cmdLoad;
    }
}