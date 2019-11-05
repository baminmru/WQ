namespace IntersectionTest
{
    partial class frmMain
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
            this.cmdMap = new System.Windows.Forms.Button();
            this.cmdLoad = new System.Windows.Forms.Button();
            this.cmdCalcINterval = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdMap
            // 
            this.cmdMap.Location = new System.Drawing.Point(21, 13);
            this.cmdMap.Name = "cmdMap";
            this.cmdMap.Size = new System.Drawing.Size(767, 50);
            this.cmdMap.TabIndex = 0;
            this.cmdMap.Text = "Скрипты для загрузки УДС";
            this.cmdMap.UseVisualStyleBackColor = true;
            this.cmdMap.Click += new System.EventHandler(this.cmdMap_Click);
            // 
            // cmdLoad
            // 
            this.cmdLoad.Location = new System.Drawing.Point(21, 82);
            this.cmdLoad.Name = "cmdLoad";
            this.cmdLoad.Size = new System.Drawing.Size(766, 53);
            this.cmdLoad.TabIndex = 1;
            this.cmdLoad.Text = "Загрузка данных";
            this.cmdLoad.UseVisualStyleBackColor = true;
            this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
            // 
            // cmdCalcINterval
            // 
            this.cmdCalcINterval.Location = new System.Drawing.Point(21, 154);
            this.cmdCalcINterval.Name = "cmdCalcINterval";
            this.cmdCalcINterval.Size = new System.Drawing.Size(766, 53);
            this.cmdCalcINterval.TabIndex = 2;
            this.cmdCalcINterval.Text = "Расчет индивидульных интервалов";
            this.cmdCalcINterval.UseVisualStyleBackColor = true;
            this.cmdCalcINterval.Click += new System.EventHandler(this.cmdCalcINterval_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(767, 50);
            this.button1.TabIndex = 3;
            this.button1.Text = "Скрипты для загрузки OSM";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(21, 299);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(767, 50);
            this.button2.TabIndex = 4;
            this.button2.Text = "Тренды";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdCalcINterval);
            this.Controls.Add(this.cmdLoad);
            this.Controls.Add(this.cmdMap);
            this.Name = "frmMain";
            this.Text = "Расчеты по УДС";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdMap;
        private System.Windows.Forms.Button cmdLoad;
        private System.Windows.Forms.Button cmdCalcINterval;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}