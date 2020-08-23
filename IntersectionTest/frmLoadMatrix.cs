using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Data.SqlClient;
using NLog;


namespace IntersectionTest
{
    public partial class frmLoadMatrix : Form
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();



        private static object optLocker = new object();

        public frmLoadMatrix()
        {
            InitializeComponent();
        }


        private void cmdSelectFolder_Click(object sender, EventArgs e)
        {
            if (fb.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = fb.SelectedPath;
            }
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            if (txtCN.Text != "" && txtFolder.Text != "" && txtWildcard.Text != "")
            {
                SqlConnection cn = new SqlConnection(txtCN.Text);
                try
                {
                    cn.Open();
                }
                catch (System.Exception ex)
                {
                    logger.Info(ex.Message + " " + txtCN.Text);
                }
                if (cn.State != ConnectionState.Open)
                {
                    MessageBox.Show("Connection test error", "Wrong parameters");
                    return;
                }


                MatrixOperations.Folder = txtFolder.Text;
                MatrixOperations.CN = txtCN.Text;


                cmdStart.Enabled = false;
                txtCN.Enabled = false;
                txtWildcard.Enabled = false;
                cmdSelectFolder.Enabled = false;

                if (MatrixOperations.ProcessFolder(txtWildcard.Text))
                {
                    timer1.Enabled = true;

                }
                else
                {
                    cmdStart.Enabled = true;
                    txtCN.Enabled = true;
                    txtWildcard.Enabled = true;
                    txtWildcard.Text = "No files - " + txtWildcard.Text;
                    cmdSelectFolder.Enabled = true;
                }

            }
            else
            {
                MessageBox.Show("Folder,Wildcard or Connection is empty.", "Wrong parameters");
                return;
            }



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string s = "";
            if (MatrixOperations.StartTime == DateTime.MinValue)
            {

                s = "Processing File: " + MatrixOperations.fCur.ToString() + " of (" + MatrixOperations.fCnt.ToString() + "). Finished: " + MatrixOperations.fDone.ToString()
                    + "\r\nLines read: " + MatrixOperations.lCnt.ToString("N0");
            }
            else
            {
                DateTime cur = DateTime.Now;
                TimeSpan ts = cur - MatrixOperations.StartTime;
                if (ts.TotalSeconds > 0)
                {
                    s = "Processing File: " + MatrixOperations.fCur.ToString() + " of (" + MatrixOperations.fCnt.ToString() + "). Finished: " + MatrixOperations.fDone.ToString()
                    + "\r\nLines read: " + MatrixOperations.lCnt.ToString("N0")
                    + "\r\nCar: " + MatrixOperations.tCur.ToString("N0") + " of (" + MatrixOperations.tCnt.ToString("N0") + ")" + ". Car Per Second =" + (MatrixOperations.tCur / ts.TotalSeconds).ToString("0.00")
                    + "\r\nTrips: " + MatrixOperations.dCur.ToString("N0") + ". Trip Per Second =" + (MatrixOperations.dCur / ts.TotalSeconds).ToString("0.00")
                    + "\r\nRecords=" + MatrixOperations.ACount.ToString("N0");

                }
            }
            txtLog.Text = s;

            if (MatrixOperations.fCnt > 0 && MatrixOperations.fDone == MatrixOperations.fCnt)
            {
                timer1.Enabled = false;
                cmdStart.Enabled = true;
                txtCN.Enabled = true;
                txtWildcard.Enabled = true;
                txtWildcard.Text = "DONE-" + txtWildcard.Text;
                cmdSelectFolder.Enabled = true;

            }
            Application.DoEvents();
        }
    }
}
