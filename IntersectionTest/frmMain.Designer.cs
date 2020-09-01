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
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.cmdLoadMatrixData = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
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
            this.cmdLoad.Location = new System.Drawing.Point(22, 59);
            this.cmdLoad.Name = "cmdLoad";
            this.cmdLoad.Size = new System.Drawing.Size(766, 53);
            this.cmdLoad.TabIndex = 1;
            this.cmdLoad.Text = "Загрузка данных";
            this.cmdLoad.UseVisualStyleBackColor = true;
            this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
            // 
            // cmdCalcINterval
            // 
            this.cmdCalcINterval.Location = new System.Drawing.Point(21, 108);
            this.cmdCalcINterval.Name = "cmdCalcINterval";
            this.cmdCalcINterval.Size = new System.Drawing.Size(766, 53);
            this.cmdCalcINterval.TabIndex = 2;
            this.cmdCalcINterval.Text = "Расчет индивидульных интервалов";
            this.cmdCalcINterval.UseVisualStyleBackColor = true;
            this.cmdCalcINterval.Click += new System.EventHandler(this.cmdCalcINterval_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(767, 50);
            this.button1.TabIndex = 3;
            this.button1.Text = "Скрипты для загрузки OSM";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(22, 213);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(767, 50);
            this.button2.TabIndex = 4;
            this.button2.Text = "Тренды";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(22, 269);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(767, 50);
            this.button3.TabIndex = 5;
            this.button3.Text = "Выходные \\ Рабочие дни";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(19, 325);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(228, 50);
            this.button4.TabIndex = 6;
            this.button4.Text = "Скрипты для регионов";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // cmdLoadMatrixData
            // 
            this.cmdLoadMatrixData.Location = new System.Drawing.Point(253, 325);
            this.cmdLoadMatrixData.Name = "cmdLoadMatrixData";
            this.cmdLoadMatrixData.Size = new System.Drawing.Size(225, 50);
            this.cmdLoadMatrixData.TabIndex = 7;
            this.cmdLoadMatrixData.Text = "Загрузка данных для матриц";
            this.cmdLoadMatrixData.UseVisualStyleBackColor = true;
            this.cmdLoadMatrixData.Click += new System.EventHandler(this.cmdLoadMatrixData_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(493, 327);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(293, 47);
            this.button5.TabIndex = 8;
            this.button5.Text = "Матрицы корреспонденции";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 394);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.cmdLoadMatrixData);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdCalcINterval);
            this.Controls.Add(this.cmdLoad);
            this.Controls.Add(this.cmdMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Расчеты по УДС  V.2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdMap;
        private System.Windows.Forms.Button cmdLoad;
        private System.Windows.Forms.Button cmdCalcINterval;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button cmdLoadMatrixData;
        private System.Windows.Forms.Button button5;
    }
}