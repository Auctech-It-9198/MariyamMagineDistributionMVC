using BOL_Business_Objects_Layer_;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection.Emit;

namespace DAL_Data_Access_Layer_
{
    public class SubscriptionsDb
    {
        private bool _result;
        public bool Result
        {
            get { return this._result; }
            set { this._result = value; }
        }
        private string _message;
        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }
        SqlObject sqlobj = new SqlObject();
        public void Add(Subscriptions obj)
        {
            Result = false;
            SqlTransaction trn = null;
            SqlConnection conn = DbConnection.getConnection();
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); };

                trn = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Transaction = trn;

                cmd.Parameters.Clear();
                string vno = getvno("tbl_subscriptions_master_detials_subscriber", "sbid", cmd);

                //subscriptions detials
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_subscriptions_master_detials_subscriber";
                AddCommandParameter(cmd, obj, vno);
                cmd.ExecuteNonQuery();


                //subcriptions
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_subscriptions";
                AddCommandParameterforsubscriptions(cmd, obj, vno);
                cmd.ExecuteNonQuery();


                trn.Commit();
                Result = true;
                Message = "Congratulation ! Record has been Added Successfully !";

            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_membership") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_membership ";
                }
                else if (ex.Message.Contains("uk_tbl_membership") == true)
                {
                    Message = "This Subscriber Name Already Exist.";
                }
                else if (ex.Message.Contains("fk_tbl_membership_tbl_membertype") == true)
                {
                    Message = "This Member Type not Exists.";
                }
                else
                {
                    Message = ex.Message;
                }
            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }

        private string getvno(string tblname, string coumnname, SqlCommand cmd)
        {
            string rectno = "0";
            cmd.CommandText = "usp_generate_new_id";
            cmd.Parameters.AddWithValue("@tblname", tblname);
            cmd.Parameters.AddWithValue("@colname", coumnname);
            rectno = cmd.ExecuteScalar().ToString();
            return rectno;
        }

        private void AddCommandParameter(SqlCommand cmd, Subscriptions obj, string vno)
        {
            cmd.Parameters.AddWithValue("@sbid", Convert.ToInt32(vno));
            cmd.Parameters.AddWithValue("@msid", Convert.ToInt32(obj.msid));
            cmd.Parameters.AddWithValue("@mscode", obj.mscode);
            cmd.Parameters.AddWithValue("@mzid", Convert.ToInt32(obj.mzid));
            cmd.Parameters.AddWithValue("@mzname", obj.mzname);
            cmd.Parameters.AddWithValue("@mfrequency", obj.mfrequency);
            cmd.Parameters.AddWithValue("@mtenure", obj.mtenure);
            cmd.Parameters.AddWithValue("@substatus", obj.substatus);
            cmd.Parameters.AddWithValue("@expdate", Convert.ToDateTime(obj.todate).ToString("yyyy-MM-dd"));
            DateTime dt = Convert.ToDateTime((obj.todate));
            cmd.Parameters.AddWithValue("@expmonth", Convert.ToDateTime(obj.todate).ToString("MMMM"));
            cmd.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(obj.fromdate).ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@expyear", dt.Year);
            cmd.Parameters.AddWithValue("@gperiods", obj.greaseperiods);


        }



        private void AddCommandParameterforsubscriptions(SqlCommand cmd, Subscriptions obj, string vno)
        {
            cmd.Parameters.AddWithValue("@sbid", Convert.ToInt32(vno));
            cmd.Parameters.AddWithValue("@msid", Convert.ToInt32(obj.msid));
            cmd.Parameters.AddWithValue("@mscode", obj.mscode);
            cmd.Parameters.AddWithValue("@mzid", Convert.ToInt32(obj.mzid));
            cmd.Parameters.AddWithValue("@mzname", obj.mzname);
            cmd.Parameters.AddWithValue("@mfrequency", obj.mfrequency);
            cmd.Parameters.AddWithValue("@mtenure", obj.mtenure);
            cmd.Parameters.AddWithValue("@pricemasterid", Convert.ToInt32(obj.pricemasterid));
            cmd.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(obj.fromdate).ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@todate", Convert.ToDateTime(obj.todate).ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@greaseperiods", obj.greaseperiods);
            cmd.Parameters.AddWithValue("@rate", Convert.ToDecimal(obj.rate));
            cmd.Parameters.AddWithValue("@amt", Convert.ToDecimal(obj.amt));
            cmd.Parameters.AddWithValue("@gst", Convert.ToDecimal(obj.gst));
            cmd.Parameters.AddWithValue("@gstamt", Convert.ToDecimal(obj.gstamt));
            cmd.Parameters.AddWithValue("@dis", Convert.ToDecimal(obj.dis));
            cmd.Parameters.AddWithValue("@disamt", Convert.ToDecimal(obj.disamt));
            cmd.Parameters.AddWithValue("@paybleamt", Convert.ToDecimal(obj.paybleamt));
            cmd.Parameters.AddWithValue("@substatus", obj.substatus);

        }


        public List<Subscriptions> AllListPagewise(JqueryDatatableParam param, int cid, int sid, int cityid, string deactive, string forworder, string mtype, string gender, string mzid, string tenure, string fromdate, string todate, string expmonth, string expyear)
        {
            List<Subscriptions> lst = new List<Subscriptions>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_subscriptions_pagewise", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@displaylength", 10000000);
                com.Parameters.AddWithValue("@displayStart", 0);
                com.Parameters.AddWithValue("@sortcol", param.iSortCol_0);
                com.Parameters.AddWithValue("@sortdir", param.sSortDir_0);
                com.Parameters.AddWithValue("@search", param.sSearch);
                com.Parameters.AddWithValue("@cid", cid);
                com.Parameters.AddWithValue("@sid", sid);
                com.Parameters.AddWithValue("@cityid", cityid);

                com.Parameters.AddWithValue("@deactive", deactive);
                com.Parameters.AddWithValue("@forworder", forworder);
                com.Parameters.AddWithValue("@mtype", mtype);
                com.Parameters.AddWithValue("@gender", gender);
                com.Parameters.AddWithValue("@mzid", mzid);
                com.Parameters.AddWithValue("@tenure", tenure);
                com.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd"));
                com.Parameters.AddWithValue("@todate", Convert.ToDateTime(todate).ToString("yyyy-MM-dd"));
                com.Parameters.AddWithValue("@expmonth", expmonth.Trim());
                com.Parameters.AddWithValue("@expyear", expyear.Trim());
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Subscriptions
                    {
                        sbid = rdr["sbid"].ToString(),
                        mscode = rdr["mscode"].ToString(),
                        Name = rdr["name"].ToString(),
                        Mobile = rdr["mobile"].ToString(),
                        mzname = rdr["mzname"].ToString(),
                        mfrequency = rdr["mfrequency"].ToString(),
                        mtenure = rdr["mtenure"].ToString(),
                        fromdate = Convert.ToDateTime(rdr["fromdate"]).ToString("yyyy-MM-dd"),
                        todate = Convert.ToDateTime(rdr["todate"]).ToString("yyyy-MM-dd"),
                        expmonth= rdr["expmonth"].ToString(),
                        greaseperiods = rdr["gperiods"].ToString(),
                        substatus = rdr["substatus"].ToString(),
                        mzid = rdr["mzid"].ToString(),
                        msid = rdr["msid"].ToString(),

                    });

                }
                return lst;
            }
        }


        public List<Subscriptions> getRecordsbysbid(string sbid, string mzid, string msid)
        {
            List<Subscriptions> lst = new List<Subscriptions>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_getrecordse_fromsubscription_by_sbid", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@sbid", sbid);
                com.Parameters.AddWithValue("@mzid", mzid);
                com.Parameters.AddWithValue("@msid", msid);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Subscriptions
                    {
                        sbid = rdr["sbid"].ToString(),
                        msid = rdr["msid"].ToString(),
                        mzid = rdr["mzid"].ToString(),
                        mscode = rdr["mscode"].ToString(),
                        Name = rdr["name"].ToString(),
                        Mobile = rdr["mobile"].ToString(),
                        mzname = rdr["mzname"].ToString(),
                        mfrequency = rdr["mfrequency"].ToString(),
                        mtenure = rdr["mtenure"].ToString(),
                        substatus = rdr["substatus"].ToString(),
                        fromdate = Convert.ToDateTime(rdr["fromdate"]).ToString("yyyy-MM-dd"),
                        todate = Convert.ToDateTime(rdr["todate"]).ToString("yyyy-MM-dd"),
                        greaseperiods = rdr["gperiods"].ToString(),

                    });

                }
                return lst;
            }
        }




        public void Renew(Subscriptions obj)
        {
            Result = false;
            SqlTransaction trn = null;
            SqlConnection conn = DbConnection.getConnection();
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); };

                trn = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Transaction = trn;

                //subcriptions master table
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_update_tbl_subscriptions_master_detials_subscriber";
                AddCommandParameterforsubscriptionupdaterecords(cmd, obj);
                cmd.ExecuteNonQuery();


                //subcriptions
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_subscriptions";
                AddCommandParameterforsubscriptionsrenew(cmd, obj);
                cmd.ExecuteNonQuery();




                trn.Commit();
                Result = true;
                Message = "Congratulation ! Record has been Added Successfully !";

            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_membership") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_membership ";
                }
                else if (ex.Message.Contains("uk_tbl_membership") == true)
                {
                    Message = "This Subscriber Name Already Exist.";
                }
                else if (ex.Message.Contains("fk_tbl_membership_tbl_membertype") == true)
                {
                    Message = "This Member Type not Exists.";
                }
                else
                {
                    Message = ex.Message;
                }
            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }

        private void AddCommandParameterforsubscriptionupdaterecords(SqlCommand cmd, Subscriptions obj)
        {
            cmd.Parameters.AddWithValue("@sbid", Convert.ToInt32(obj.sbid));
            cmd.Parameters.AddWithValue("@msid", Convert.ToInt32(obj.msid));
            cmd.Parameters.AddWithValue("@mzid", Convert.ToInt32(obj.mzid));
            cmd.Parameters.AddWithValue("@mfrequency", obj.mfrequency);
            cmd.Parameters.AddWithValue("@mtenure", obj.mtenure);
            cmd.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(obj.fromdate).ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@expdate", Convert.ToDateTime(obj.todate).ToString("yyyy-MM-dd"));
            DateTime dt = Convert.ToDateTime((obj.todate));
            cmd.Parameters.AddWithValue("@expmonth", Convert.ToDateTime(obj.todate).ToString("MMMM"));
            cmd.Parameters.AddWithValue("@expyear", dt.Year);

            cmd.Parameters.AddWithValue("@gperiods", obj.greaseperiods);
            cmd.Parameters.AddWithValue("@substatus", obj.substatus);
        }

        private void AddCommandParameterforsubscriptionsrenew(SqlCommand cmd, Subscriptions obj)
        {
            cmd.Parameters.AddWithValue("@sbid", Convert.ToInt32(obj.sbid));
            cmd.Parameters.AddWithValue("@msid", Convert.ToInt32(obj.msid));
            cmd.Parameters.AddWithValue("@mscode", obj.mscode);
            cmd.Parameters.AddWithValue("@mzid", Convert.ToInt32(obj.mzid));
            cmd.Parameters.AddWithValue("@mzname", obj.mzname);
            cmd.Parameters.AddWithValue("@mfrequency", obj.mfrequency);
            cmd.Parameters.AddWithValue("@mtenure", obj.mtenure);
            cmd.Parameters.AddWithValue("@pricemasterid", Convert.ToInt32(obj.pricemasterid));
            cmd.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(obj.fromdate).ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@todate", Convert.ToDateTime(obj.todate).ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@greaseperiods", obj.greaseperiods);
            cmd.Parameters.AddWithValue("@rate", Convert.ToDecimal(obj.rate));
            cmd.Parameters.AddWithValue("@amt", Convert.ToDecimal(obj.amt));
            cmd.Parameters.AddWithValue("@gst", Convert.ToDecimal(obj.gst));
            cmd.Parameters.AddWithValue("@gstamt", Convert.ToDecimal(obj.gstamt));
            cmd.Parameters.AddWithValue("@dis", Convert.ToDecimal(obj.dis));
            cmd.Parameters.AddWithValue("@disamt", Convert.ToDecimal(obj.disamt));
            cmd.Parameters.AddWithValue("@paybleamt", Convert.ToDecimal(obj.paybleamt));
            cmd.Parameters.AddWithValue("@substatus", obj.substatus);
        }
        public void Grace(Subscriptions obj)
        {
            Result = false;
            SqlTransaction trn = null;
            SqlConnection conn = DbConnection.getConnection();
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); };

                trn = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Transaction = trn;

                //subcriptions master table
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_update_tbl_subscriptions_master_detials_subscriber_grace";
                AddCommandParameterforsubscriptionupdaterecordsgrace(cmd, obj);
                cmd.ExecuteNonQuery();



                trn.Commit();
                Result = true;
                Message = "Congratulation ! Record has been Added Successfully !";

            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_membership") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_membership ";
                }
                else if (ex.Message.Contains("uk_tbl_membership") == true)
                {
                    Message = "This Subscriber Name Already Exist.";
                }
                else if (ex.Message.Contains("fk_tbl_membership_tbl_membertype") == true)
                {
                    Message = "This Member Type not Exists.";
                }
                else
                {
                    Message = ex.Message;
                }
            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }

        private void AddCommandParameterforsubscriptionupdaterecordsgrace(SqlCommand cmd, Subscriptions obj)
        {
            cmd.Parameters.AddWithValue("@sbid", Convert.ToInt32(obj.sbid));
            cmd.Parameters.AddWithValue("@msid", Convert.ToInt32(obj.msid));
            cmd.Parameters.AddWithValue("@mzid", Convert.ToInt32(obj.mzid));
            cmd.Parameters.AddWithValue("@gperiods", obj.greaseperiods);

        }
    }
}
