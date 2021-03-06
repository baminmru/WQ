﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntersectionTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void cmdMap_Click(object sender, EventArgs e)
        {
            frmMap fm = new frmMap();
            fm.Show();
        }

        private void cmdLoad_Click(object sender, EventArgs e)
        {
            frmBatch fb = new frmBatch();
            fb.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmOSM fosm = new frmOSM();
            fosm.Show();
        }

        private void cmdCalcINterval_Click(object sender, EventArgs e)
        {
            frmInterval fi = new frmInterval();
            fi.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmTrand fi = new frmTrand();
            fi.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
                frmDayOff fb = new frmDayOff();
                fb.Show();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmMatrix fm = new frmMatrix();
            fm.Show();
        }

        private void cmdLoadMatrixData_Click(object sender, EventArgs e)
        {
            frmLoadMatrix fb = new frmLoadMatrix();
            fb.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMatrixBuilder fb = new frmMatrixBuilder();
            fb.Show();
        }
    }
}
