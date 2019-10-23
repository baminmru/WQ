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
               
                
                BatchOperations.Folder = txtFolder.Text;
                BatchOperations.CN = txtCN.Text;

                cmdStart.Enabled = false;
                txtCN.Enabled = false;
                txtWildcard.Enabled = false;
                cmdSelectFolder.Enabled = false;
                if (BatchOperations.ProcessFolder(txtWildcard.Text))
                {
                    timer1.Enabled = true;
                    
                }
                else
                {
                    cmdStart.Enabled = true;
                    txtCN.Enabled = true;
                    txtWildcard.Enabled = true;
                    txtWildcard.Text="No files - " + txtWildcard.Text;
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
            if (BatchOperations.StartTime == DateTime.MinValue){

                s = "Processing File: " + BatchOperations.fCur.ToString() + " of (" + BatchOperations.fCnt.ToString() + "). Finished: " + BatchOperations.fDone.ToString()
                    + "\r\nLines read: " + BatchOperations.lCnt.ToString("N0");
            }
            else
            {
                DateTime cur = DateTime.Now;
                TimeSpan ts = cur - BatchOperations.StartTime;
                if (ts.TotalSeconds > 0)
                {
                    s = "Processing File: " + BatchOperations.fCur.ToString() + " of (" + BatchOperations.fCnt.ToString() + "). Finished: " + BatchOperations.fDone.ToString()
                    + "\r\nLines read: " + BatchOperations.lCnt.ToString("N0")
                    + "\r\nCar: " + BatchOperations.tCur.ToString("N0") + " of (" + BatchOperations.tCnt.ToString("N0") + ")" + ". Car Per Second =" + (BatchOperations.tCur / ts.TotalSeconds).ToString("0.00")
                    + "\r\nTrips: " + BatchOperations.dCur.ToString("N0") + ". Trip Per Second =" + (BatchOperations.dCur / ts.TotalSeconds).ToString("0.00")
                    + "\r\nRecords=" + BatchOperations.ACount.ToString("N0");
                 
                }
            }
            txtLog.Text = s;

            if(BatchOperations.fCnt >0 && BatchOperations.fDone== BatchOperations.fCnt)
            {
                timer1.Enabled = false;
                cmdStart.Enabled = true;
                txtCN.Enabled = true;
                txtWildcard.Enabled = true;
                txtWildcard.Text="DONE-" + txtWildcard.Text;
                cmdSelectFolder.Enabled = true;

            }
            Application.DoEvents();
        }
    }
}
