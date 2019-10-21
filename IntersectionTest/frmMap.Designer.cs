namespace IntersectionTest
{
    partial class frmMap
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
            this.mapBox1 = new SharpMap.Forms.MapBox();
            this.mapZoomToolStrip1 = new SharpMap.Forms.ToolBar.MapZoomToolStrip(this.components);
            this.SuspendLayout();
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
            this.mapBox1.Location = new System.Drawing.Point(6, 28);
            this.mapBox1.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.LayerByIndex;
            this.mapBox1.Name = "mapBox1";
            this.mapBox1.QueryGrowFactor = 5F;
            this.mapBox1.QueryLayerIndex = 0;
            this.mapBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.ShowProgressUpdate = false;
            this.mapBox1.Size = new System.Drawing.Size(782, 410);
            this.mapBox1.TabIndex = 0;
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
            this.mapZoomToolStrip1.TabIndex = 1;
            this.mapZoomToolStrip1.Text = "mapZoomToolStrip1";
            // 
            // frmMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mapZoomToolStrip1);
            this.Controls.Add(this.mapBox1);
            this.Name = "frmMap";
            this.Text = "frmMap";
            this.Load += new System.EventHandler(this.FrmMap_Load);
            this.Resize += new System.EventHandler(this.FrmMap_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpMap.Forms.MapBox mapBox1;
        private SharpMap.Forms.ToolBar.MapZoomToolStrip mapZoomToolStrip1;
    }
}