using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
using System.Web;
using NLog;
using System.Net;

namespace IntersectionTest
{
    public partial class frmDayOff : Form
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public frmDayOff()
        {
            InitializeComponent();
        }

        private void cmdDayOff_Click(object sender, EventArgs e)
        {
            if (txtCN.Text != "" )
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

                WebClient wc = new WebClient();
                try
                {
                    txtData.Text = wc.DownloadString("https://isdayoff.ru/api/getdata?year=" + numYear.Value.ToString());
                    if (txtData.Text.Length > 0)
                    {
                        SaveDayOff(txtCN.Text, txtData.Text);
                    }
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex, " Register DayOff");
                }

               

            }
        }


        private  void SaveDayOff(string sCN, String data)
        {


            System.Data.SqlClient.SqlConnection cn;



            try
            {

                cn = new SqlConnection(sCN);
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;


                    cmd.CommandText = "delete from DayOff where Y=" + numYear.Value.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();

                    }
                    catch (System.Exception ex)
                    {
                        logger.Error(ex, "SaveDayOff Y=" + numYear.Value.ToString());
                    }


                    char[] days = data.ToCharArray();
                    for (int dy=0; dy < days.Length;dy++)
                    {
                        string qry = @"INSERT INTO DayOff(Y ,YD,DayOff)VALUES(" +
                        numYear.Value.ToString() + "," + (dy+1).ToString() + ","+ days[dy] + ")";
                        cmd.CommandText = qry;
                        try
                        {
                            cmd.ExecuteNonQuery();

                        }
                        catch (System.Exception ex)
                        {
                            logger.Error(ex, "SaveDayOff Y=" + numYear.Value.ToString() +" DY=" + dy.ToString());
                        }
                    }
                }
                cn.Close();
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "SaveDayOff");
            }
            //ACount--;
            //FinishedCount++;
        }
    }
}
