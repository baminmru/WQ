using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace IntersectionTest
{
    public partial class frmInterval : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public frmInterval()
        {
            InitializeComponent();
        }

        private void cmdStart_Click(object sender, EventArgs e)
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

            DataTable dt;
            dt = new DataTable();
            
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cmd.CommandText = @"DELETE FROM  UDSinterval";
            cmd.ExecuteNonQuery();


            cmd.CommandText = @"INSERT INTO [UDSinterval]
           ([object_id]
           ,[DAYTYPE]
           ,[DAYINTERVAL]
           ,[HOUR]
           ,[DIRECTION])
     select [object_id]
           ,[DAYTYPE]
           ,[DAYINTERVAL]
           ,[HOUR]
           ,[DIRECTION]
		   from UDSSTDInterval";
            cmd.ExecuteNonQuery();




            cmd.CommandText = @"SELECT OBJECT_ID FROM UDS";
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                sda.Fill(dt);
            }
            catch (System.Exception ex)
            {
                logger.Debug(cmd.CommandText + " " + ex.Message);
            }
            string o;

            

            for(int i=0; i < dt.Rows.Count;i++)
            {
               
                
                o = dt.Rows[i]["object_id"].ToString();

                txtLog.Text = i.ToString() + " (" + dt.Rows.Count.ToString() + ") ->" +o;
                Application.DoEvents();

                string query = @"select daytype,direction,hour,avg(v) V from v_track where object_id=" + o + @"
                group by daytype,direction,hour
                HAVING COUNT(*) >10
                order by daytype,direction,hour";

                DataTable dt2;
                dt2 = new DataTable();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = cn;
                cmd2.CommandText =query;
                SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);

                try
                {
                    sda2.Fill(dt2);
                }
                catch (System.Exception ex)
                {
                    logger.Debug(cmd2.CommandText + " " + ex.Message);
                }

                Dictionary<string, double> data = new Dictionary<string, double>();
                for (int j = 0; j < dt2.Rows.Count;j++)
                {
                    data.Add(dt2.Rows[j]["daytype"].ToString() + dt2.Rows[j]["Direction"].ToString() + dt2.Rows[j]["hour"].ToString(), (double)dt2.Rows[j]["v"]);
                }

                string[] types = { "WG", "WR", "HG", "HR" };
                foreach( string hwgr in types)
                {
                    // i1 
                    try
                    {
                        if (data[hwgr + "7"] + data[hwgr + "8"] + data[hwgr + "9"] + data[hwgr + "10"] > data[hwgr + "11"] + data[hwgr + "8"] + data[hwgr + "9"] + data[hwgr + "10"])
                        {
                            try
                            {
                                cmd.CommandText = @"update UDsInterval set HOUR=11 where object_id=" + o + " and daytype='" + hwgr.Substring(0, 1) + "' and DIRECTION='" +
                                hwgr.Substring(1, 1) + "' and dayinterval=1 and hour=7";
                                cmd.ExecuteNonQuery();
                            }
                            catch (System.Exception ex)
                            {
                                logger.Debug(cmd.CommandText + " " + ex.Message);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        logger.Debug( ex.Message);
                    }

                    //i2
                    try
                    {
                        if (data[hwgr + "12"] + data[hwgr + "13"] + data[hwgr + "14"] < data[hwgr + "13"] + data[hwgr + "14"] + data[hwgr + "15"])
                        {
                            try
                            {
                                cmd.CommandText = @"update UDsInterval set HOUR=15 where object_id=" + o + " and daytype='" + hwgr.Substring(0, 1) + "' and DIRECTION='" +
                                hwgr.Substring(1, 1) + "' and dayinterval=2 and hour=12";
                            cmd.ExecuteNonQuery();
                            }
                            catch (System.Exception ex)
                            {
                                logger.Debug(cmd.CommandText + " " + ex.Message);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        logger.Debug(ex.Message);
                    }

                    //i3
                    try
                    {
                        if (data[hwgr + "17"] + data[hwgr + "18"] + data[hwgr + "19"] > data[hwgr + "18"] + data[hwgr + "19"] + data[hwgr + "20"])
                        {
                            try
                            {
                                cmd.CommandText = @"update UDsInterval set HOUR=20 where object_id=" + o + " and daytype='" + hwgr.Substring(0, 1) + "' and DIRECTION='" +
                                hwgr.Substring(1, 1) + "' and dayinterval=3 and hour=17";
                            cmd.ExecuteNonQuery();
                            }
                            catch (System.Exception ex)
                            {
                                logger.Debug(cmd.CommandText + " " + ex.Message);
                            }
                        }
                        else
                        if (data[hwgr + "17"] + data[hwgr + "18"] + data[hwgr + "19"] > data[hwgr + "19"] + data[hwgr + "20"] + data[hwgr + "21"])
                        {
                            try
                            {
                                cmd.CommandText = @"update UDsInterval set HOUR=20 where object_id=" + o + " and daytype='" + hwgr.Substring(0, 1) + "' and DIRECTION='" +
                            hwgr.Substring(1, 1) + "' and dayinterval=3 and hour=17";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = @"update UDsInterval set HOUR=21 where object_id=" + o + " and daytype='" + hwgr.Substring(0, 1) + "' and DIRECTION='" +
                                hwgr.Substring(1, 1) + "' and dayinterval=3 and hour=18";
                            cmd.ExecuteNonQuery();
                            }
                            catch (System.Exception ex)
                            {
                                logger.Debug(cmd.CommandText + " " + ex.Message);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        logger.Debug( ex.Message);
                    }

                    //i4
                    try
                    {
                        if (data[hwgr + "22"] + data[hwgr + "23"] + data[hwgr + "0"] < data[hwgr + "23"] + data[hwgr + "0"] + data[hwgr + "1"])
                        {
                            try
                            {
                                cmd.CommandText = @"update UDsInterval set HOUR=1 where object_id=" + o + " and daytype='" + hwgr.Substring(0, 1) + "' and DIRECTION='" +
                            hwgr.Substring(1, 1) + "' and dayinterval=4 and hour=22";
                            cmd.ExecuteNonQuery();
                                }
                            catch (System.Exception ex)
                            {
                                logger.Debug(cmd.CommandText + " " + ex.Message);
                            }
                          }       
                    }
                    catch (System.Exception ex)
                    {
                        logger.Debug( ex.Message);
                    }

                }




            }



        }
    }
}
