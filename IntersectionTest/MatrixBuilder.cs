using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Utils;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Globalization;

namespace IntersectionTest
{
    public class MatrixBuilder
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static CultureInfo ci = new CultureInfo("en-US");

        private string CN;
        public MatrixBuilder( string _CN)
        {
            CN = _CN;
        }

        public String Status { get; set; }

        public void BuildDaylyMatrix(string FileName)
        {
            var workbook = new XLWorkbook();
            System.Data.DataTable dt;
            System.Data.SqlClient.SqlConnection cn;
            List<MatrixItem> matrix = new List<MatrixItem>();

            try
            {
                

                cn = new SqlConnection(CN);
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {



                    dt = new System.Data.DataTable();

                    // получаем список регионов для матрицы
                    {
                        dt = new System.Data.DataTable();
                        //iQryCnt++;
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cn;

                        cmd.CommandText = @"SELECT CODE FROM REGIONG where CODE is not null and code !='' group by code order by convert(int,code) ";
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);

                        try
                        {
                            sda.Fill(dt);
                        }
                        catch (System.Exception ex)
                        {
                            logger.Debug(cmd.CommandText + " " + ex.Message);
                        }


                        sda.Dispose();
                        cmd.Dispose();
                    }

                
                    DateTime startDay = new DateTime(2019, 11, 1);
                    DateTime stopDay = new DateTime(2020, 7, 1);
                    DateTime curDay = startDay;
                    while (curDay < stopDay)
                    {

                        Status = curDay.ToShortDateString();

                        var ws = workbook.Worksheets.Add(curDay.ToShortDateString());

                        var dataMatrix = new List<string[]>();
                        List<string> hdr = new List<string>();
                        hdr.Add("from\\to");
                        for (int f = 0; f < dt.Rows.Count; f++)
                        {
                            hdr.Add(dt.Rows[f]["CODE"].ToString());
                            
                        }
                        dataMatrix.Add(hdr.ToArray());

                        
                        for (int f = 0; f < dt.Rows.Count; f++)
                        {

                            String fCode = dt.Rows[f]["CODE"].ToString();


                            Dictionary<string,string> dictCnt = new Dictionary<string, string>();
                            

                            System.Data.DataTable dtFT = new System.Data.DataTable();

                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cn;

                            cmd.CommandText = @"select count(track) cnt, toRegion from Matrix where  FromRegion ='" + fCode + "' and datepart(year,FromTime) =" + curDay.Year.ToString() + " and datepart(month,FromTime) =" + curDay.Month.ToString() + " and datepart(day,FromTime) =" + curDay.Day.ToString() + " group by toRegion ";
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);

                            try
                            {
                                sda.Fill(dtFT);
                            }
                            catch (System.Exception ex)
                            {
                                logger.Debug(cmd.CommandText + " " + ex.Message);
                            }


                            sda.Dispose();
                            cmd.Dispose();


                            for (int t = 0; t < dtFT.Rows.Count; t++)
                            {
                                dictCnt.Add(dtFT.Rows[t]["toRegion"].ToString(), dtFT.Rows[t]["cnt"].ToString());
                            }

                            List<string> r = new List<string>();
                            r.Add(fCode);
                            for (int t = 0; t < dt.Rows.Count; t++)
                            {
                                String tCode = dt.Rows[t]["CODE"].ToString();
                                if (dictCnt.ContainsKey(tCode))
                                {
                                    r.Add(dictCnt[tCode]);
                                }
                                else
                                {
                                    r.Add("-");
                                }
                            }



                            dataMatrix.Add(r.ToArray());
                            r.Clear();
                            dictCnt.Clear();
                            dtFT.Dispose();

                        }


                        ws.Cell(1, 1).InsertData(dataMatrix);


                        // nex day
                        curDay = curDay.AddDays(1);
                    }

                    Status = "Save file";
                   workbook.SaveAs(FileName);
                    Status = "Done";
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
            }
        }




        public void BuildSummerMatrix(string FileName, int rType)
        {
            var workbook = new XLWorkbook();
            System.Data.DataTable dt;
            System.Data.SqlClient.SqlConnection cn;
            List<MatrixItem> matrix = new List<MatrixItem>();

            try
            {


                cn = new SqlConnection(CN);
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {



                    dt = new System.Data.DataTable();

                    // получаем список регионов для матрицы
                    {
                        dt = new System.Data.DataTable();
                        //iQryCnt++;
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cn;

                        cmd.CommandText = @"SELECT CODE FROM REGIONG where CODE is not null and code !='' group by code order by convert(int,code) ";
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);

                        try
                        {
                            sda.Fill(dt);
                        }
                        catch (System.Exception ex)
                        {
                            logger.Debug(cmd.CommandText + " " + ex.Message);
                        }


                        sda.Dispose();
                        cmd.Dispose();
                    }


                   
                        var ws = workbook.Worksheets.Add("Summer time");

                        var dataMatrix = new List<string[]>();
                        List<string> hdr = new List<string>();
                        hdr.Add("from\\to");
                        for (int f = 0; f < dt.Rows.Count; f++)
                        {
                            hdr.Add(dt.Rows[f]["CODE"].ToString());

                        }
                        dataMatrix.Add(hdr.ToArray());


                        for (int f = 0; f < dt.Rows.Count; f++)
                        {

                            String fCode = dt.Rows[f]["CODE"].ToString();


                            Dictionary<string, string> dictCnt = new Dictionary<string, string>();


                            System.Data.DataTable dtFT = new System.Data.DataTable();

                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cn;

                            //cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)),count(track) cnt from Matrix where   FromRegion ='" + fCode +@"' and datepart(month,FromTime) in (12,1,2)
                            //group by toRegion
                            //having count(distinct datepart(DAYOFYEAR,FromTime)) >=48";


                        switch (rType)
                        {
                            case 1: // workingDay
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,count(track) cnt  from Matrix  join  DayOff
on datepart(DAYOFYEAR,FromTime)=YD and datepart(year,FromTime) = Y
where DAYOFF=0 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (4,5,6,7,8)
group by toRegion
having count(distinct datepart(DAYOFYEAR, FromTime)) >= 34";
                                break;

                            case 2: // holiday
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,count(track) cnt from Matrix  join  DayOff
                                on datepart(DAYOFYEAR,FromTime)=YD and datepart(year,FromTime) = Y
                                where DAYOFF=1 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (4,5,6,7,8)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR, FromTime)) >= 14";
                                break;


                            case 3: // workingDay
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,sum(datediff(second,fromTime,toTime)) / 60.0  minutes,count(track) cnt  from Matrix  join  DayOff
                                on datepart(DAYOFYEAR,FromTime)=YD and datepart(year,FromTime) = Y
                                where valid=1 and DAYOFF=0 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (4,5,6,7,8)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR, FromTime)) >= 34";
                                break;

                            case 4: // holiday
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,sum(datediff(second,fromTime,toTime)) / 60.0  minutes,count(track) cnt from Matrix  join  DayOff
                                on datepart(DAYOFYEAR,FromTime)=YD and datepart(year,FromTime) = Y
                                where valid=1 and DAYOFF=1 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (4,5,6,7,8)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR, FromTime)) >= 14";
                                break;

                            case 5: // any day
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,sum(datediff(second,fromTime,toTime)) / 60.0  minutes,count(track) cnt from Matrix 
                                where valid=1 and  FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (4,5,6,7,8)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR,FromTime)) >=48";
                                break;



                            default: // any day
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,count(track) cnt from Matrix where   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (4,5,6,7,8)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR,FromTime)) >=48";
                                break;

                        }

                        SqlDataAdapter sda = new SqlDataAdapter(cmd);

                            try
                            {
                                sda.Fill(dtFT);
                            }
                            catch (System.Exception ex)
                            {
                                logger.Debug(cmd.CommandText + " " + ex.Message);
                            }


                            sda.Dispose();
                            cmd.Dispose();


                            for (int t = 0; t < dtFT.Rows.Count; t++)
                            {
                                double v;
                                if (rType <= 2)
                                    v = double.Parse(dtFT.Rows[t]["cnt"].ToString()) / double.Parse(dtFT.Rows[t]["days"].ToString());
                                else
                                v = double.Parse(dtFT.Rows[t]["minutes"].ToString()) / double.Parse(dtFT.Rows[t]["cnt"].ToString());
                            
                                dictCnt.Add(dtFT.Rows[t]["toRegion"].ToString(), v.ToString("0.000", ci));
                            }

                            List<string> r = new List<string>();
                            r.Add(fCode);
                            for (int t = 0; t < dt.Rows.Count; t++)
                            {
                                String tCode = dt.Rows[t]["CODE"].ToString();
                                if (dictCnt.ContainsKey(tCode))
                                {
                                    r.Add(dictCnt[tCode]);
                                }
                                else
                                {
                                    r.Add("-");
                                }
                            }



                            dataMatrix.Add(r.ToArray());
                            r.Clear();
                            dictCnt.Clear();
                            dtFT.Dispose();

                        }


                        ws.Cell(1, 1).InsertData(dataMatrix);


                      

                    Status = "Save file";
                    workbook.SaveAs(FileName);
                    Status = "Done";
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
            }
        }


        public void BuildWinterMatrix(string FileName, int rType)
        {
            var workbook = new XLWorkbook();
            System.Data.DataTable dt;
            System.Data.SqlClient.SqlConnection cn;
            List<MatrixItem> matrix = new List<MatrixItem>();

            try
            {


                cn = new SqlConnection(CN);
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {



                    dt = new System.Data.DataTable();

                    // получаем список регионов для матрицы
                    {
                        dt = new System.Data.DataTable();
                        //iQryCnt++;
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cn;

                        cmd.CommandText = @"SELECT CODE FROM REGIONG where CODE is not null and code !='' group by code order by convert(int,code) ";
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);

                        try
                        {
                            sda.Fill(dt);
                        }
                        catch (System.Exception ex)
                        {
                            logger.Debug(cmd.CommandText + " " + ex.Message);
                        }


                        sda.Dispose();
                        cmd.Dispose();
                    }



                    var ws = workbook.Worksheets.Add("Winter time");

                    var dataMatrix = new List<string[]>();
                    List<string> hdr = new List<string>();
                    hdr.Add("from\\to");
                    for (int f = 0; f < dt.Rows.Count; f++)
                    {
                        hdr.Add(dt.Rows[f]["CODE"].ToString());

                    }
                    dataMatrix.Add(hdr.ToArray());


                    for (int f = 0; f < dt.Rows.Count; f++)
                    {

                        String fCode = dt.Rows[f]["CODE"].ToString();


                        Dictionary<string, string> dictCnt = new Dictionary<string, string>();


                        System.Data.DataTable dtFT = new System.Data.DataTable();

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cn;

                        switch (rType)
                        {
                            case 1: // workingDay
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days ,count(track) cnt  from Matrix  join  DayOff
                                on datepart(DAYOFYEAR,FromTime)=YD and datepart(year,FromTime) = Y
                                where DAYOFF=0 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (12,1,2)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR, FromTime)) >= 34";
                                break;

                            case 2: // holiday
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,count(track) cnt from Matrix  join  DayOff
                                on datepart(DAYOFYEAR,FromTime)=YD and datepart(year,FromTime) = Y
                                where DAYOFF=1 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (12,1,2)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR, FromTime)) >= 14"; 
                                break;


                            case 3: // time for workingDay
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days , sum(datediff(second,fromTime,toTime)) / 60.0  minutes, count(track) cnt  from Matrix  join  DayOff
                                on datepart(DAYOFYEAR,FromTime)=YD and datepart(year,FromTime) = Y
                                where valid=1 and  DAYOFF=0 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (12,1,2)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR, FromTime)) >= 34";
                                break;

                            case 4: // time for holiday
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,sum(datediff(second,fromTime,toTime)) / 60.0  minutes,count(track) cnt from Matrix  join  DayOff
                                on datepart(DAYOFYEAR,FromTime)=YD and datepart(year,FromTime) = Y
                                where valid=1 and  DAYOFF=1 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (12,1,2)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR, FromTime)) >= 14";
                                break;

                            case 5: // time for any day
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,sum(datediff(second,fromTime,toTime)) / 60.0  minutes,count(track) cnt from Matrix 
                                where valid=1 and   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (12,1,2)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR,FromTime)) >=48";
                                break;

                            default: // any day =0
                                cmd.CommandText = @"select  toRegion, count(distinct datepart(DAYOFYEAR,FromTime)) days,count(track) cnt from Matrix where   FromRegion ='" + fCode + @"' and datepart(month,FromTime) in (12,1,2)
                                group by toRegion
                                having count(distinct datepart(DAYOFYEAR,FromTime)) >=48";
                                break;

                        }
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);

                        try
                        {
                            sda.Fill(dtFT);
                        }
                        catch (System.Exception ex)
                        {
                            logger.Debug(cmd.CommandText + " " + ex.Message);
                        }


                        sda.Dispose();
                        cmd.Dispose();


                        for (int t = 0; t < dtFT.Rows.Count; t++)
                        {
                            double v;
                            if(rType <=2)
                                v= double.Parse(dtFT.Rows[t]["cnt"].ToString()) / double.Parse(dtFT.Rows[t]["days"].ToString());
                            else
                                v = double.Parse(dtFT.Rows[t]["minutes"].ToString()) / double.Parse(dtFT.Rows[t]["cnt"].ToString());



                            dictCnt.Add(dtFT.Rows[t]["toRegion"].ToString(),  v.ToString("0.000", ci));
                        }

                        List<string> r = new List<string>();
                        r.Add(fCode);
                        for (int t = 0; t < dt.Rows.Count; t++)
                        {
                            String tCode = dt.Rows[t]["CODE"].ToString();
                            if (dictCnt.ContainsKey(tCode))
                            {
                                r.Add(dictCnt[tCode]);
                            }
                            else
                            {
                                r.Add("-");
                            }
                        }



                        dataMatrix.Add(r.ToArray());
                        r.Clear();
                        dictCnt.Clear();
                        dtFT.Dispose();

                    }


                    ws.Cell(1, 1).InsertData(dataMatrix);




                    Status = "Save file";
                    workbook.SaveAs(FileName);
                    Status = "Done";
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
            }
        }

    }
}
