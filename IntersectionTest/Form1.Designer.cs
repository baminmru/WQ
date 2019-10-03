namespace IntersectionTest
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.opf = new System.Windows.Forms.OpenFileDialog();
            this.cmdCSV = new System.Windows.Forms.Button();
            this.grpParam = new System.Windows.Forms.GroupBox();
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStopTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMinV = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblCnt = new System.Windows.Forms.Label();
            this.cmsShowMap = new System.Windows.Forms.Button();
            this.grpParam.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 470);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(248, 51);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(453, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 90);
            this.label1.TabIndex = 1;
            // 
            // cmdCSV
            // 
            this.cmdCSV.Location = new System.Drawing.Point(31, 202);
            this.cmdCSV.Name = "cmdCSV";
            this.cmdCSV.Size = new System.Drawing.Size(416, 60);
            this.cmdCSV.TabIndex = 2;
            this.cmdCSV.Text = "Open CSV";
            this.cmdCSV.UseVisualStyleBackColor = true;
            this.cmdCSV.Click += new System.EventHandler(this.Button2_Click);
            // 
            // grpParam
            // 
            this.grpParam.Controls.Add(this.txtDistance);
            this.grpParam.Controls.Add(this.label4);
            this.grpParam.Controls.Add(this.txtStopTime);
            this.grpParam.Controls.Add(this.label3);
            this.grpParam.Controls.Add(this.txtMinV);
            this.grpParam.Controls.Add(this.label2);
            this.grpParam.Location = new System.Drawing.Point(31, 12);
            this.grpParam.Name = "grpParam";
            this.grpParam.Size = new System.Drawing.Size(416, 184);
            this.grpParam.TabIndex = 4;
            this.grpParam.TabStop = false;
            this.grpParam.Text = "Параметры разбивки трека";
            // 
            // txtDistance
            // 
            this.txtDistance.Location = new System.Drawing.Point(232, 113);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(138, 20);
            this.txtDistance.TabIndex = 9;
            this.txtDistance.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(199, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Расстояние от точки остановки, метр";
            // 
            // txtStopTime
            // 
            this.txtStopTime.Location = new System.Drawing.Point(232, 74);
            this.txtStopTime.Name = "txtStopTime";
            this.txtStopTime.Size = new System.Drawing.Size(138, 20);
            this.txtStopTime.TabIndex = 7;
            this.txtStopTime.Text = "5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Время простоя, минут";
            // 
            // txtMinV
            // 
            this.txtMinV.Location = new System.Drawing.Point(232, 29);
            this.txtMinV.Name = "txtMinV";
            this.txtMinV.Size = new System.Drawing.Size(138, 20);
            this.txtMinV.TabIndex = 5;
            this.txtMinV.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Минимальная скорость, км/ч";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // lblCnt
            // 
            this.lblCnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCnt.Location = new System.Drawing.Point(464, 251);
            this.lblCnt.Name = "lblCnt";
            this.lblCnt.Size = new System.Drawing.Size(123, 54);
            this.lblCnt.TabIndex = 5;
            this.lblCnt.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // cmsShowMap
            // 
            this.cmsShowMap.Location = new System.Drawing.Point(31, 268);
            this.cmsShowMap.Name = "cmsShowMap";
            this.cmsShowMap.Size = new System.Drawing.Size(416, 60);
            this.cmsShowMap.TabIndex = 6;
            this.cmsShowMap.Text = "Show MAP";
            this.cmsShowMap.UseVisualStyleBackColor = true;
            this.cmsShowMap.Click += new System.EventHandler(this.CmsShowMap_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 334);
            this.Controls.Add(this.cmsShowMap);
            this.Controls.Add(this.lblCnt);
            this.Controls.Add(this.grpParam);
            this.Controls.Add(this.cmdCSV);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Работа с GPS данными";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.grpParam.ResumeLayout(false);
            this.grpParam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog opf;
        private System.Windows.Forms.Button cmdCSV;
        private System.Windows.Forms.GroupBox grpParam;
        private System.Windows.Forms.TextBox txtDistance;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStopTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMinV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblCnt;
        private System.Windows.Forms.Button cmsShowMap;
    }
}

