using BOL_Business_Objects_Layer_;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace DAL_Data_Access_Layer_
{
    public class DeliveryDb
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
       
     
       
        public List<Delivery> AllListPagewise(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            
            List<Delivery> lst = new List<Delivery>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_subscriptions_dispatch", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@search", search==null ? "": search);
                com.Parameters.AddWithValue("@cid", cityid);
                com.Parameters.AddWithValue("@sid", sid);
                com.Parameters.AddWithValue("@cityid", cityid);
                com.Parameters.AddWithValue("@area", areaid);
                com.Parameters.AddWithValue("@deactive", deactive);
                com.Parameters.AddWithValue("@mtype", mtype);
                com.Parameters.AddWithValue("@gender", gender);
                com.Parameters.AddWithValue("@mzid", mzid);
                com.Parameters.AddWithValue("@mrid", mrid);
                com.Parameters.AddWithValue("@gperiods", gperiods);
                com.Parameters.AddWithValue("@ctstatus", cstatus);
               

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Delivery
                    {
                       
                        sbid = rdr["sbid"].ToString(),
                        msid = rdr["msid"].ToString(),
                        mscode = rdr["mscode"].ToString(),
                        mzid = rdr["mzid"].ToString(),
                        fromdate = rdr["fromdate"].ToString(),
                        expdate = rdr["expdate"].ToString(),
                        gpperiods = rdr["gperiods"].ToString(),
                        expmonth = rdr["expmonth"].ToString(),
                        expyear = rdr["expyear"].ToString(),
                        substatus = rdr["substatus"].ToString(),
                        name = rdr["name"].ToString(),
                        mobile = rdr["mobile"].ToString(),
                        memberType = rdr["membertype"].ToString(),
                        localadress = rdr["localaddress"].ToString(),
                        gender = rdr["gender"].ToString(),
                        expdatewithgrace = Convert.ToDateTime(rdr["expdatewithgrace"].ToString()).ToString("dd-MM-yyyy"),
                        expmonthwithgrace = rdr["expmonthwithgrace"].ToString(),
                        cstatus = rdr["cstatus"].ToString(),
                        cid = rdr["cid"].ToString(),
                        sid = rdr["sid"].ToString(),
                        cityid = rdr["cityid"].ToString(),
                        areaid = rdr["areaid"].ToString(),
                        mid = rdr["mid"].ToString(),
                        mrid = rdr["mrid"].ToString(),
                        membertypeid = rdr["membertypeid"].ToString(),
                    });

                }
                return lst;
            }
        }

        public void Add(List<Delivery> delivery)
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
                int qty = delivery.Count;
                cmd.Parameters.Clear();
                string vno = "";
                     vno = getNewVno(cmd, "tbl_master", "vno", delivery[0].vtype);
                
               
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_master";
                cmd.Parameters.AddWithValue("@vno", vno);
                cmd.Parameters.AddWithValue("@vdate", delivery[0].vdate==null || delivery[0].vdate == null ? System.DateTime.Now.ToString("yyyy-MM-dd") : delivery[0].vdate);
                cmd.Parameters.AddWithValue("@vtype", delivery[0].vtype);

                if (delivery[0].vtype == "Di")
                {
                    cmd.Parameters.AddWithValue("@msid", "0");
                    cmd.Parameters.AddWithValue("@mscode", "0");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@msid", delivery[0].msid);
                    cmd.Parameters.AddWithValue("@mscode", delivery[0].mscode == null ? "0" : delivery[0].mscode);
                }
               
                cmd.Parameters.AddWithValue("@mzid", delivery[0].mzid);
                cmd.Parameters.AddWithValue("@mzname", delivery[0].mzname);
                cmd.Parameters.AddWithValue("@mrid", delivery[0].mrid);
                cmd.Parameters.AddWithValue("@mrname ", delivery[0].mrname);
                cmd.Parameters.AddWithValue("@cid", delivery[0].cid);
                cmd.Parameters.AddWithValue("@sit", delivery[0].sid);
                cmd.Parameters.AddWithValue("@cityid ", delivery[0].cityid);
                cmd.Parameters.AddWithValue("@areaid", delivery[0].areaid);
                cmd.Parameters.AddWithValue("@mtypeid", delivery[0].membertypeid ==null ? null : delivery[0].membertypeid);
                cmd.Parameters.AddWithValue("@gender", delivery[0].gender == null ? "0" : delivery[0].gender);
                cmd.Parameters.AddWithValue("@selecttype ", delivery[0].selecttype == null ? "0" : delivery[0].selecttype);
                cmd.Parameters.AddWithValue("@price", 0);
                cmd.Parameters.AddWithValue("@rate", 0);

                if (delivery[0].vtype == "Sl") // Sale
                {
                    cmd.Parameters.AddWithValue("@qty", delivery[0].qty);
                }
                else if (delivery[0].vtype == "Sr") //Sale Retunr
                {
                    cmd.Parameters.AddWithValue("@qty", delivery[0].qty);
                }
                else if (delivery[0].vtype == "Si")//  this stock come from Magazine
                {
                    cmd.Parameters.AddWithValue("@qty", delivery[0].qty);
                }
                else if (delivery[0].vtype == "Da") //Damage Entry
                {
                    cmd.Parameters.AddWithValue("@qty", delivery[0].qty);
                }
                else if (delivery[0].vtype == "Sn") //stock iN
                {
                    cmd.Parameters.AddWithValue("@qty", delivery[0].qty);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@qty", qty); //dispatch
                }

             
                cmd.Parameters.AddWithValue("@gstper ", 0);
                cmd.Parameters.AddWithValue("@gstamt", 0);
                cmd.Parameters.AddWithValue("@disper", 0);
                cmd.Parameters.AddWithValue("@disamt", 0);
                cmd.Parameters.AddWithValue("@paybleamt ", 0);
                cmd.Parameters.AddWithValue("@totalamt", delivery[0].totalamt ==null ? "0" : delivery[0].totalamt);
                cmd.Parameters.AddWithValue("@trantype", delivery[0].trantype);
                cmd.Parameters.AddWithValue("@partyid", delivery[0].msid);
                cmd.ExecuteNonQuery();

                foreach (Delivery data in delivery)
                {
                    cmd.Parameters.Clear();
                    if (data.vtype == "Di")
                    {
                        data.orderno = usp_generate_orderno(cmd);
                    }
                    else
                    {
                        data.orderno = "0";
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "usp_insert_tbl_master_ledger";
                    cmd.Parameters.AddWithValue("@ordeno", data.orderno);
                    cmd.Parameters.AddWithValue("@vno", vno);
                    cmd.Parameters.AddWithValue("@lvdate", delivery[0].vdate == null || delivery[0].vdate == null ? System.DateTime.Now.ToString("yyyy-MM-dd") : delivery[0].vdate);
                    cmd.Parameters.AddWithValue("@lvtype", data.vtype);
                    cmd.Parameters.AddWithValue("@msid", data.msid);
                    cmd.Parameters.AddWithValue("@mscode", data.mscode);
                    cmd.Parameters.AddWithValue("@mzid", data.mzid);
                    cmd.Parameters.AddWithValue("@mzname", data.mzname);
                    cmd.Parameters.AddWithValue("@mrid", data.mrid);
                    cmd.Parameters.AddWithValue("@mrname ", data.mrname);
                    cmd.Parameters.AddWithValue("@selecttype ", data.selecttype == null ? "0" : data.selecttype);
                    cmd.Parameters.AddWithValue("@price", data.priceid);
                    cmd.Parameters.AddWithValue("@rate", data.rate);
                    cmd.Parameters.AddWithValue("@qty", data.qty);
                    cmd.Parameters.AddWithValue("@gstper ", data.gst);
                    cmd.Parameters.AddWithValue("@gstamt", data.gstamt);
                    cmd.Parameters.AddWithValue("@disper", data.dis);
                    cmd.Parameters.AddWithValue("@disamt", data.disamt);
                    cmd.Parameters.AddWithValue("@paybleamt ", data.paybleamt);
                    cmd.Parameters.AddWithValue("@totalamt", data.totalamt);
                    cmd.Parameters.AddWithValue("@lstatsu", "0"); //0 means not dispatch
                    cmd.Parameters.AddWithValue("@trantype", data.trantype); 
                    cmd.ExecuteNonQuery();

                }
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Records has been Added Successfully !";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                Message = ex.Message;

            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }

        }

        private string usp_generate_orderno(SqlCommand cmd)
        {
            string ret = "";
            cmd.Parameters.Clear();
            cmd.CommandText = "usp_generate_orderno";
            try
            {
                ret = cmd.ExecuteScalar().ToString();
                if (ret == null) { return ""; };
            }
            catch { }
            return ret;
        }

        private string getNewVno(SqlCommand cmd, string tblname, string colname, string vtype)
        {
            string ret = "";
            cmd.Parameters.Clear();
            cmd.CommandText = "usp_generate_new_id_vtype";
            cmd.Parameters.AddWithValue("@tblname", DbType.String).Value = tblname.ToString();
            cmd.Parameters.AddWithValue("@colname", DbType.String).Value = colname;
            cmd.Parameters.AddWithValue("@vtype", DbType.String).Value = vtype;
            try
            {
                ret = cmd.ExecuteScalar().ToString();
                if (ret == null) { return ""; };
            }
            catch { }
            return ret;
          
        }

        public List<Delivery> AllListPagewiselist(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            List<Delivery> lst = new List<Delivery>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_subscriptions_dispatch_list", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@search", search == null ? "" : search);
                com.Parameters.AddWithValue("@cid", cityid);
                com.Parameters.AddWithValue("@sid", sid);
                com.Parameters.AddWithValue("@cityid", cityid);
                com.Parameters.AddWithValue("@area", areaid);
                com.Parameters.AddWithValue("@deactive", deactive);
                com.Parameters.AddWithValue("@mtype", mtype);
                com.Parameters.AddWithValue("@gender", gender);
                com.Parameters.AddWithValue("@mzid", mzid);
                com.Parameters.AddWithValue("@mrid", mrid);
                com.Parameters.AddWithValue("@ctstatus", cstatus);


                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Delivery
                    {

                        vno = rdr["vno"].ToString(),
                        vdate = Convert.ToDateTime(rdr["vdate"].ToString()).ToString("dd-MM-yyyy"),
                        mzname = rdr["mzname"].ToString(),
                        mrname = rdr["mrname"].ToString(),
                        qty = rdr["qty"].ToString(),
                    });

                }
                return lst;
            }
        }

        public void Delete(int vno, string vtype)
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
                cmd.CommandText = "usp_delete_tbl_master";
                cmd.Parameters.AddWithValue("@vno", vno);
                cmd.Parameters.AddWithValue("@vtype", vtype);
                cmd.ExecuteNonQuery();
                trn.Commit();
                Result = true;
                Message = "Record Delete Successfully";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_magzine") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_magzine ";
                }
                else if (ex.Message.Contains("Uk_magzinenNme") == true)
                {
                    Message = "This Magzine Name Already Exist.";
                }
                else if (ex.Message.Contains("Uk_magzineCode") == true)
                {
                    Message = "This Magzine Code Already Exist.";
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

        public List<Delivery> DisplayListforprint(string vno, string vtype, string isfarwarder)
        {
            List<Delivery> lst = new List<Delivery>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_subscriptions_dispatch_list_print", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@vno", vno);
                com.Parameters.AddWithValue("@vtype", vtype);
                com.Parameters.AddWithValue("@isfarwarder", isfarwarder);



                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Delivery
                    {

                        vno = rdr["vno"].ToString(),
                        address = rdr["forwaderaddress"].ToString(),
                        isfarwarder= rdr["isForwarder"].ToString(),
                        orderno = rdr["ordeno"].ToString(),
                        mscode = rdr["mscode"].ToString(),
                        mzid = rdr["mzid"].ToString(),
                        mrid= rdr["mrid"].ToString(),
                        msid = rdr["msid"].ToString(),
                        lvno = rdr["lvno"].ToString(),
                        localadress = rdr["localaddress"].ToString(),
                        farwarderid = rdr["parentid"].ToString(),
                        name = rdr["name"].ToString(),
                        mobile = rdr["mobile"].ToString(),
                        printcount = rdr["printcount"].ToString(),


                    });

                }
                return lst;
            }
        }

        public List<Delivery> AllListPagewiseforprintlables(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            List<Delivery> lst = new List<Delivery>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_subscriptions_dispatch_printlabels", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@search", search == null ? "" : search);
                com.Parameters.AddWithValue("@cid", cityid);
                com.Parameters.AddWithValue("@sid", sid);
                com.Parameters.AddWithValue("@cityid", cityid);
                com.Parameters.AddWithValue("@area", areaid);
                com.Parameters.AddWithValue("@deactive", deactive);
                com.Parameters.AddWithValue("@mtype", mtype);
                com.Parameters.AddWithValue("@gender", gender);
                com.Parameters.AddWithValue("@mzid", mzid);
                com.Parameters.AddWithValue("@mrid", mrid);
                com.Parameters.AddWithValue("@gperiods", gperiods);
                com.Parameters.AddWithValue("@ctstatus", cstatus);


                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Delivery
                    {

                        sbid = rdr["sbid"].ToString(),
                        msid = rdr["msid"].ToString(),
                        mscode = rdr["mscode"].ToString(),
                        mzid = rdr["mzid"].ToString(),
                        fromdate = rdr["fromdate"].ToString(),
                        expdate = rdr["expdate"].ToString(),
                        gpperiods = rdr["gperiods"].ToString(),
                        expmonth = rdr["expmonth"].ToString(),
                        expyear = rdr["expyear"].ToString(),
                        substatus = rdr["substatus"].ToString(),
                        name = rdr["name"].ToString(),
                        mobile = rdr["mobile"].ToString(),
                        memberType = rdr["membertype"].ToString(),
                        localadress = rdr["localaddress"].ToString(),
                        gender = rdr["gender"].ToString(),
                        expdatewithgrace = Convert.ToDateTime(rdr["expdatewithgrace"].ToString()).ToString("dd-MM-yyyy"),
                        expmonthwithgrace = rdr["expmonthwithgrace"].ToString(),
                        cstatus = rdr["cstatus"].ToString(),
                        cid = rdr["cid"].ToString(),
                        sid = rdr["sid"].ToString(),
                        cityid = rdr["cityid"].ToString(),
                        areaid = rdr["areaid"].ToString(),
                        mid = rdr["mid"].ToString(),
                        mrid = rdr["mrid"].ToString(),
                        membertypeid = rdr["membertypeid"].ToString(),
                        isfarwarder = rdr["isForwarder"].ToString(),
                        address = rdr["localaddress"].ToString(),
                        farwarderaddress = rdr["forwaderaddress"].ToString(),
                       

                    });

                }
                return lst;
            }
        }



        public void DispatchAdd(List<Delivery> delivery)
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
                string vno = getNewVno(cmd, "tbl_dispatch", "dvno", "Dh");
                cmd.Parameters.Clear();


                foreach (Delivery data in delivery)
                {
                
                    cmd.Parameters.Clear();
                    cmd.CommandText = "usp_insert_into_tbl_dispatch";
                    cmd.Parameters.AddWithValue("@dvno", vno);
                    cmd.Parameters.AddWithValue("@ordeno", data.orderno);
                    cmd.Parameters.AddWithValue("@lvno", data.lvno);
                    cmd.Parameters.AddWithValue("@ddate", System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@dvtype", "Dh");
                    cmd.Parameters.AddWithValue("@mzid", data.mzid);
                    cmd.Parameters.AddWithValue("@mzname", data.mzname);
                    cmd.Parameters.AddWithValue("@mrid", data.mrid);
                    cmd.Parameters.AddWithValue("@mrname", data.mrname);
                    cmd.Parameters.AddWithValue("@msid", data.msid);
                    cmd.Parameters.AddWithValue("@mscode ", data.mscode);
                    cmd.Parameters.AddWithValue("@dispatchmod ", data.dispatchmod);
                    cmd.Parameters.AddWithValue("@dispatchremarks ", data.dispatchremarks);
                    cmd.Parameters.AddWithValue("@dispatchstatus ", data.dispatchstatus);

                }
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Records has been Added Successfully !";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                Message = ex.Message;

            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }

        }

        public void AddPrint(List<Delivery> delivery)
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

                //Delete Record
                //cmd.Parameters.Clear();
                //cmd.CommandText = "usp_delete_print";
                //cmd.Parameters.AddWithValue("@vno", delivery[0].vno);
                //cmd.Parameters.AddWithValue("@vtype", "Pt");
                //cmd.ExecuteNonQuery();

          

                //get pvno
                cmd.Parameters.Clear();
                string vno = getNewVno(cmd, "tbl_print", "pvno", "Pt");
                cmd.Parameters.Clear();

                int pqty = delivery.Count();
                foreach (Delivery data in delivery)
                {

                    cmd.Parameters.Clear();
                    cmd.CommandText = "insert_into_tbl_print";
                    cmd.Parameters.AddWithValue("@pvno", vno);
                    cmd.Parameters.AddWithValue("@ordeno", data.orderno);
                    cmd.Parameters.AddWithValue("@lvno", data.lvno);
                    cmd.Parameters.AddWithValue("@vno", data.vno);
                    cmd.Parameters.AddWithValue("@pdate", System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@vtype", "Pt");
                    cmd.Parameters.AddWithValue("@mzid", data.mzid);
                    cmd.Parameters.AddWithValue("@mzname", data.mzname);
                    cmd.Parameters.AddWithValue("@mrid", data.mrid);
                    cmd.Parameters.AddWithValue("@mrname", data.mrname);
                    cmd.Parameters.AddWithValue("@msid", data.msid);
                    cmd.Parameters.AddWithValue("@localaddress ", data.localadress);
                    cmd.Parameters.AddWithValue("@isfarwarder ", data.isfarwarder);
                    cmd.Parameters.AddWithValue("@isfarwarderid ", data.farwarderid !=" " ? data.farwarderid : "0");
                    cmd.Parameters.AddWithValue("@isfarwarderstatus ", data.farwarderstatus);
                    cmd.Parameters.AddWithValue("@addresstosend ", data.farwarderaddress);
                    cmd.Parameters.AddWithValue("@printqty ", pqty);
                    cmd.ExecuteNonQuery();


                }
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Records has been Added Successfully !";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                Message = ex.Message;

            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }

      
            public List<Delivery> DisplayPrinlist(JqueryDatatableParam param, string mzid, string mrid)
            {
                List<Delivery> lst = new List<Delivery>();
                using (SqlConnection con = DbConnection.getConnection())
                {
                    con.Open();
                    SqlCommand com = new SqlCommand("usp_display_tbl_print_pagewise", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@displaylength", 10000000);
                    com.Parameters.AddWithValue("@displayStart", 0);
                    com.Parameters.AddWithValue("@sortcol", param.iSortCol_0);
                    com.Parameters.AddWithValue("@sortdir", param.sSortDir_0);
                    com.Parameters.AddWithValue("@search", param.sSearch);
                    com.Parameters.AddWithValue("@mzid", mzid);
                    com.Parameters.AddWithValue("@mrid", mrid);
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new Delivery
                        {
                            vno = rdr["vno"].ToString(),
                            pvno = rdr["pvno"].ToString(),
                            mzname = rdr["mzname"].ToString(),
                            mrname = rdr["mrname"].ToString(),
                            vdate = Convert.ToDateTime(rdr["pdate"].ToString()).ToString("dd-MM-yyyy"),
                            printcount = rdr["printcount"].ToString(),


                        });
                    }
                    return lst;
                }
            }

        public void DeletePrintRecord(int vno, int pvno)
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
                cmd.CommandText = "usp_delete_tbl_print_records";
                cmd.Parameters.AddWithValue("@vno", vno);
                cmd.Parameters.AddWithValue("@pvno", pvno);
                cmd.Parameters.AddWithValue("@vtype", "Pt");
                cmd.ExecuteNonQuery();
                trn.Commit();
                Result = true;
                Message = "Record Delete Successfully";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_magzine") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_magzine ";
                }
                else if (ex.Message.Contains("Uk_magzinenNme") == true)
                {
                    Message = "This Magzine Name Already Exist.";
                }
                else if (ex.Message.Contains("Uk_magzineCode") == true)
                {
                    Message = "This Magzine Code Already Exist.";
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

        public List<Delivery> DisplayListforDispatch(string vno)
        {
            List<Delivery> lst = new List<Delivery>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_subscriptions_dispatch_list_print_dispatch", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@vno", vno);
                com.Parameters.AddWithValue("@vtype", "Di");

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Delivery
                    {

                        vno = rdr["vno"].ToString(),
                        lvno = rdr["lvno"].ToString(),
                        orderno = rdr["ordeno"].ToString(),
                        name = rdr["name"].ToString(),
                        mobile = rdr["mobile"].ToString(),
                        printstatus = rdr["printstatus"].ToString(),
                        pid = rdr["pid"].ToString(),
                        pvno= rdr["pvno"].ToString(),
                        mzid = rdr["mzid"].ToString(),
                        mzname = rdr["mzname"].ToString(),
                        mrid= rdr["mrid"].ToString(),
                        mrname = rdr["mrname"].ToString(),
                        msid = rdr["msid"].ToString(),
                        disptachmode= rdr["dispatchmod"].ToString(),
                        disremarks = rdr["dispatchremarks"].ToString(),
                        dispdate = Convert.ToDateTime(rdr["deldate"].ToString()).ToString("yyyy-MM-dd"),
                        disrecremarks = rdr["dstatusremakrs"].ToString(),
                        distatus = rdr["dispatchstatus"].ToString(),


                    });

                }
                return lst;
            }
        }

        public void Dispatchpostadd(List<Delivery> delivery)
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


                //get pvno
                cmd.Parameters.Clear();
                string vno = getNewVno(cmd, "tbl_dispatch", "dvno", "Dp");
                cmd.Parameters.Clear();

                int pqty = delivery.Count();
                foreach (Delivery data in delivery)
                {

                    cmd.Parameters.Clear();
                    cmd.CommandText = "usp_insert_tbl_dispatch";
                    cmd.Parameters.AddWithValue("@dvno", vno);
                    cmd.Parameters.AddWithValue("@ordeno", data.orderno);
                    cmd.Parameters.AddWithValue("@ddate", System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@vno", data.vno);
                    cmd.Parameters.AddWithValue("@lvno", data.lvno);
                    cmd.Parameters.AddWithValue("@pvno", data.pvno);
                    cmd.Parameters.AddWithValue("@pid", data.pid !="0" ? data.pid :null);
                    cmd.Parameters.AddWithValue("@vtype", "Dp");
                    cmd.Parameters.AddWithValue("@mzid", data.mzid);
                    cmd.Parameters.AddWithValue("@mzname", data.mzname);
                    cmd.Parameters.AddWithValue("@mrid", data.mrid);
                    cmd.Parameters.AddWithValue("@mrname", data.mrname);
                    cmd.Parameters.AddWithValue("@msid", data.msid);
                    cmd.Parameters.AddWithValue("@dispatchmod", data.disptachmode);
                    cmd.Parameters.AddWithValue("@dispatchremarks ", data.disremarks);
                    cmd.Parameters.AddWithValue("@deldate", data.dispdate != "No Option" ? data.dispdate:null );
                    cmd.Parameters.AddWithValue("@dstatusremakrs ", data.disrecremarks);
                    cmd.Parameters.AddWithValue("@dispatchstatus ", data.distatus !="true"? 0 : 1);
                    cmd.ExecuteNonQuery();

                }
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Records has been Added Successfully !";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                Message = ex.Message;

            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }

        public List<Delivery> DisplayListPagewiseforsale(JqueryDatatableParam param, string vtype)
        {
            List<Delivery> lst = new List<Delivery>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_sale_pagewise", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@displaylength", 10000000);
                com.Parameters.AddWithValue("@displayStart", 0);
                com.Parameters.AddWithValue("@sortcol", param.iSortCol_0);
                com.Parameters.AddWithValue("@sortdir", param.sSortDir_0);
                com.Parameters.AddWithValue("@search", param.sSearch);
                com.Parameters.AddWithValue("@vtype", vtype);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Delivery
                    {
                        vno = rdr["vno"].ToString(),
                        vdate = Convert.ToDateTime(rdr["vdate"].ToString()).ToString("yyyy-MM-dd"),
                        name = rdr["name"].ToString(),
                        totalamt = rdr["totalamt"].ToString(),

                    });

                }
                return lst;
            }
        }

        public void DeleteSale(int vno, string vtype)
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
                cmd.CommandText = "usp_delete_tbl_master";
                cmd.Parameters.AddWithValue("@vno", vno);
                cmd.Parameters.AddWithValue("@vtype", vtype);
                cmd.ExecuteNonQuery();
                trn.Commit();
                Result = true;
                Message = "Record Delete Successfully";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_magzine") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_magzine ";
                }
                else if (ex.Message.Contains("Uk_magzinenNme") == true)
                {
                    Message = "This Magzine Name Already Exist.";
                }
                else if (ex.Message.Contains("Uk_magzineCode") == true)
                {
                    Message = "This Magzine Code Already Exist.";
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

        public List<Delivery> GetSaleEntryRecordss(string vno, string vtype)
        {
            List<Delivery> lst = new List<Delivery>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_get_sale_record_edit", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@vno", vno);
                com.Parameters.AddWithValue("@vtype", vtype);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Delivery
                    {
                        vno = rdr["vno"].ToString(),
                        vdate = Convert.ToDateTime(rdr["lvdate"].ToString()).ToString("yyyy-MM-dd"),
                        trantype = rdr["trantype"].ToString().Trim(),
                        msid = rdr["ddl"].ToString().Trim(),

                        mzid = rdr["mzid"].ToString().Trim(),
                        mzname = rdr["mzname"].ToString().Trim(),
                        mrid = rdr["mrid"].ToString().Trim(),
                        mrname = rdr["mrname"].ToString().Trim(),
                        rate = rdr["rate"].ToString().Trim(),
                        qty = rdr["qty"].ToString().Trim(),
                        amt = (Convert.ToDecimal(rdr["rate"]) * Convert.ToInt32(rdr["qty"])).ToString(),
                        dis = rdr["disper"].ToString().Trim(),
                        disamt = rdr["disamt"].ToString().Trim(),
                        gst = rdr["gstper"].ToString().Trim(),
                        gstamt = rdr["gstamt"].ToString().Trim(),
                        paybleamt = rdr["paybleamt"].ToString().Trim(),
                        netamt = rdr["totalamt"].ToString().Trim(),
                        priceid = rdr["price"].ToString().Trim(),

                    });

                }
                return lst;
            }
        }

        public void Update(List<Delivery> delivery)
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
              
                // update master 
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_update_tbl_master";
                cmd.Parameters.AddWithValue("@vno", delivery[0].vno);
                cmd.Parameters.AddWithValue("@vdate", delivery[0].vdate == "" ? System.DateTime.Now.ToString("yyyy-MM-dd") : delivery[0].vdate);
                cmd.Parameters.AddWithValue("@vtype", delivery[0].vtype);
                cmd.Parameters.AddWithValue("@totalamt", delivery[0].totalamt);
                cmd.Parameters.AddWithValue("@trantype", delivery[0].trantype);
                cmd.Parameters.AddWithValue("@partyid", delivery[0].msid);
                cmd.ExecuteNonQuery();


                //delete from masterledger
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_delete_tbl_master_ledger";
                cmd.Parameters.AddWithValue("@vno", delivery[0].vno);
                cmd.Parameters.AddWithValue("@vtype", delivery[0].vtype);
                cmd.ExecuteNonQuery();

                foreach (Delivery data in delivery)
                {
                    
                    
                    cmd.Parameters.Clear();
                    cmd.CommandText = "usp_insert_tbl_master_ledger";
                    cmd.Parameters.AddWithValue("@ordeno", "0");
                    cmd.Parameters.AddWithValue("@vno", data.vno);
                    cmd.Parameters.AddWithValue("@lvdate", delivery[0].vdate == "" ? System.DateTime.Now.ToString("yyyy-MM-dd") : delivery[0].vdate);
                    cmd.Parameters.AddWithValue("@lvtype", data.vtype);
                    cmd.Parameters.AddWithValue("@msid", data.msid);
                    cmd.Parameters.AddWithValue("@mscode", data.mscode);
                    cmd.Parameters.AddWithValue("@mzid", data.mzid);
                    cmd.Parameters.AddWithValue("@mzname", data.mzname);
                    cmd.Parameters.AddWithValue("@mrid", data.mrid);
                    cmd.Parameters.AddWithValue("@mrname ", data.mrname);
                    cmd.Parameters.AddWithValue("@selecttype ", data.selecttype == null ? "0" : data.selecttype);
                    cmd.Parameters.AddWithValue("@price", data.priceid);
                    cmd.Parameters.AddWithValue("@rate", data.rate);
                    cmd.Parameters.AddWithValue("@qty", data.qty);
                    cmd.Parameters.AddWithValue("@gstper ", data.gst);
                    cmd.Parameters.AddWithValue("@gstamt", data.gstamt);
                    cmd.Parameters.AddWithValue("@disper", data.dis);
                    cmd.Parameters.AddWithValue("@disamt", data.disamt);
                    cmd.Parameters.AddWithValue("@paybleamt ", data.paybleamt);
                    cmd.Parameters.AddWithValue("@totalamt", data.totalamt);
                    cmd.Parameters.AddWithValue("@lstatsu", "0"); //0 means not dispatch
                    cmd.Parameters.AddWithValue("@trantype", data.trantype);
                    cmd.ExecuteNonQuery();

                }
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Records has been Added Successfully !";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                Message = ex.Message;

            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }




        //----------------------------------



    }
}
