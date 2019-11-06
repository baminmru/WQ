namespace IntersectionTest
{
    partial class frmTrand
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
            this.txtCN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdStart = new System.Windows.Forms.Button();
            this.mapBox1 = new SharpMap.Forms.MapBox();
            this.mapZoomToolStrip1 = new SharpMap.Forms.ToolBar.MapZoomToolStrip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmbInterval = new System.Windows.Forms.ComboBox();
            this.optUDS = new System.Windows.Forms.RadioButton();
            this.optBuf = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // txtCN
            // 
            this.txtCN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCN.Location = new System.Drawing.Point(342, 31);
            this.txtCN.Name = "txtCN";
            this.txtCN.Size = new System.Drawing.Size(446, 20);
            this.txtCN.TabIndex = 4;
            this.txtCN.Text = "Server=localhost;Database=WQ2;Trusted_Connection=True;MultipleActiveResultSets=tr" +
    "ue;";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Connection string:";
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(10, 27);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(72, 27);
            this.cmdStart.TabIndex = 5;
            this.cmdStart.Text = "Load";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // mapBox1
            // 
            this.mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
            this.mapBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapBox1.CustomTool = null;
            this.mapBox1.FineZoomFactor = 10D;
            this.mapBox1.Location = new System.Drawing.Point(-3, 115);
            this.mapBox1.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.LayerByIndex;
            this.mapBox1.Name = "mapBox1";
            this.mapBox1.QueryGrowFactor = 5F;
            this.mapBox1.QueryLayerIndex = 0;
            this.mapBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.ShowProgressUpdate = false;
            this.mapBox1.Size = new System.Drawing.Size(791, 334);
            this.mapBox1.TabIndex = 1;
            this.mapBox1.Text = "mapBox1";
            this.mapBox1.WheelZoomMagnitude = -2D;
            // 
            // mapZoomToolStrip1
            // 
            this.mapZoomToolStrip1.Enabled = false;
            this.mapZoomToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.mapZoomToolStrip1.MapControl = this.mapBox1;
            this.mapZoomToolStrip1.Name = "mapZoomToolStrip1";
            this.mapZoomToolStrip1.Size = new System.Drawing.Size(800, 25);
            this.mapZoomToolStrip1.TabIndex = 6;
            this.mapZoomToolStrip1.Text = "mapZoomToolStrip1";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cmbInterval
            // 
            this.cmbInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInterval.Enabled = false;
            this.cmbInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbInterval.FormattingEnabled = true;
            this.cmbInterval.Location = new System.Drawing.Point(10, 65);
            this.cmbInterval.Name = "cmbInterval";
            this.cmbInterval.Size = new System.Drawing.Size(777, 28);
            this.cmbInterval.TabIndex = 7;
            this.cmbInterval.SelectedIndexChanged += new System.EventHandler(this.cmbInterval_SelectedIndexChanged);
            // 
            // optUDS
            // 
            this.optUDS.AutoSize = true;
            this.optUDS.Checked = true;
            this.optUDS.Location = new System.Drawing.Point(100, 34);
            this.optUDS.Name = "optUDS";
            this.optUDS.Size = new System.Drawing.Size(49, 17);
            this.optUDS.TabIndex = 8;
            this.optUDS.TabStop = true;
            this.optUDS.Text = "УДС";
            this.optUDS.UseVisualStyleBackColor = true;
            // 
            // optBuf
            // 
            this.optBuf.AutoSize = true;
            this.optBuf.Location = new System.Drawing.Point(155, 34);
            this.optBuf.Name = "optBuf";
            this.optBuf.Size = new System.Drawing.Size(57, 17);
            this.optBuf.TabIndex = 9;
            this.optBuf.Text = "Буфер";
            this.optBuf.UseVisualStyleBackColor = true;
            // 
            // frmTrand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.optBuf);
            this.Controls.Add(this.optUDS);
            this.Controls.Add(this.cmbInterval);
            this.Controls.Add(this.mapZoomToolStrip1);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.txtCN);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mapBox1);
            this.Name = "frmTrand";
            this.Text = "Тренды качества обслуживания";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTrand_FormClosing);
            this.Load += new System.EventHandler(this.frmTrand_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpMap.Forms.MapBox mapBox1;
        private System.Windows.Forms.TextBox txtCN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdStart;
        private SharpMap.Forms.ToolBar.MapZoomToolStrip mapZoomToolStrip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cmbInterval;
        private System.Windows.Forms.RadioButton optUDS;
        private System.Windows.Forms.RadioButton optBuf;
    }
}