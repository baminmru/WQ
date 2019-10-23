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
    public partial class frmBatch : Form
    {


        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        


        private static object optLocker = new object();


        public frmBatch()
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
            if(txtCN.Text !="" && txtFolder.Text != "" && txtWildcard.Text != "")
            {
                SqlConnection cn = new SqlConnection(txtCN.Text);
                try
                {
                    cn.Open();
                }
                catch (System.Exception ex){
                    logger.Info(ex.Message + " " + txtCN.Text);
                        }
                if (cn.State != ConnectionState.Open)
                {
                    MessageBox.Show("Connection test error", "Wrong parameters");
                    return;
                }
                timer1.Enabled = true;
                cmdStart.Enabled = false;
                txtCN.Enabled = false;
                txtWildcard.Enabled = false;
                cmdSelectFolder.Enabled = false;
                
                BatchOperations.Folder = txtFolder.Text;
                BatchOperations.CN = txtCN.Text;
                BatchOperations.ProcessFolder(txtWildcard.Text);

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
            if (BatchOperations.StartTime == DateTime.MinValue){

                s = "Processing File: " + BatchOperations.fCur.ToString() + " of (" + BatchOperations.fCnt.ToString() + ")"
                    + "\r\nLines read: " + BatchOperations.lCnt.ToString();
            }
            else
            {
                DateTime cur = DateTime.Now;
                TimeSpan ts = cur - BatchOperations.StartTime;
                if (ts.TotalSeconds > 0)
                {
                    s = "Processing File: " + BatchOperations.fCur.ToString() + " of (" + BatchOperations.fCnt.ToString() + ")"
                    + "\r\nLines read: " + BatchOperations.lCnt.ToString()
                    + "\r\nCar: " + BatchOperations.tCur.ToString() + " of (" + BatchOperations.tCnt.ToString() + ")" + " CpS =" + (BatchOperations.tCur / ts.TotalSeconds).ToString("0.00")
                    + "\r\nTrips: " + BatchOperations.dCur.ToString() + " TpS =" + (BatchOperations.dCur / ts.TotalSeconds).ToString("0.00")
                    + "\r\nRecords=" + BatchOperations.ACount.ToString();
                 
                }
            }
            txtLog.Text = s;
            Application.DoEvents();
        }
    }
}
