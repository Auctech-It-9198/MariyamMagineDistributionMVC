using BOL_Business_Objects_Layer_;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Diagnostics;
using System.Security.Cryptography;

namespace DAL_Data_Access_Layer_
{
    public class MagzineDb
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
        public void Add(Magzine obj)
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
                obj.MagzineId = sqlobj.getNewId(cmd, "tbl_magzine", "mid");
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_magzine";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();
                if (To_tbl_price(cmd, obj.MagzineId, obj) == false) { return; };
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Record has been Added Successfully !";

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
        private void AddCommandParameter(SqlCommand cmd, Magzine obj)
        {
            cmd.Parameters.AddWithValue("@mid", obj.MagzineId);
            cmd.Parameters.AddWithValue("@mcode", obj.MagzineCode);
            cmd.Parameters.AddWithValue("@mname", obj.MagzineName);
            cmd.Parameters.AddWithValue("@mtype", obj.MagzineType);
            cmd.Parameters.AddWithValue("@frequency", obj.Frequency);
            cmd.Parameters.AddWithValue("@gstper", obj.GST);
            cmd.Parameters.AddWithValue("@publishname", obj.PublishName);
            cmd.Parameters.AddWithValue("@isbn", obj.ISBN);
            cmd.Parameters.AddWithValue("@otherdetails", obj.OtherDetails);
            cmd.Parameters.AddWithValue("@mstatus", true);
            cmd.Parameters.AddWithValue("@cdate", System.DateTime.Now.ToString("yyyy-MM-dd"));
        }
        private bool To_tbl_price(SqlCommand cmd, string MagzineId, Magzine obj)
        {
            bool res = false;
            try
            {
                
                foreach (Price data in obj.Prices)
                {
                    if (data.PriceId != "0")
                    {
                        //cmd.Parameters.Clear();
                        //cmd.CommandText = "delete_magzine_pricemaster";
                        //cmd.Parameters.Add("@mid", SqlDbType.Int).Value = MagzineId;
                        //cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cmd.CommandText = "update_magzine_pricemaster";
                        cmd.Parameters.AddWithValue("@priceid", SqlDbType.Int).Value = data.PriceId;
                        cmd.Parameters.AddWithValue("@mid", SqlDbType.Int).Value = MagzineId;
                        if (data.Tenure == "Adhoc(Single Unit)")
                        {
                            data.Tenure = "10";
                        }
                        cmd.Parameters.AddWithValue("@tenure", SqlDbType.NVarChar).Value = data.Tenure.Trim();
                        cmd.Parameters.AddWithValue("@rate", SqlDbType.Decimal).Value = data.Rate;
                        cmd.Parameters.AddWithValue("@cdate", SqlDbType.DateTime).Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                        cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "insert_magzine_pricemaster";
                        cmd.Parameters.AddWithValue("@mid", SqlDbType.Int).Value = MagzineId;
                        if (data.Tenure == "Adhoc(Single Unit)")
                        {
                            data.Tenure = "10";
                        }
                        cmd.Parameters.AddWithValue("@tenure", SqlDbType.NVarChar).Value = data.Tenure.Trim();
                        cmd.Parameters.AddWithValue("@rate", SqlDbType.Decimal).Value = data.Rate;
                        cmd.Parameters.AddWithValue("@cdate", SqlDbType.DateTime).Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                        cmd.ExecuteNonQuery();
                    }
                }
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;
        }

        public List<Magzine> AllListPagewise(JqueryDatatableParam param)
        {
            List<Magzine> lst = new List<Magzine>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_magzine_pagewise", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@displaylength", 10000000);
                com.Parameters.AddWithValue("@displayStart", 0);
                com.Parameters.AddWithValue("@sortcol", param.iSortCol_0);
                com.Parameters.AddWithValue("@sortdir", param.sSortDir_0);
                com.Parameters.AddWithValue("@search", param.sSearch);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Magzine
                    {
                        MagzineId = rdr["mid"].ToString(),
                        MagzineCode = rdr["mcode"].ToString(),
                        MagzineName = rdr["mname"].ToString(),
                        MagzineType = rdr["mtype"].ToString(),
                        Frequency = rdr["frequency"].ToString(),
                        GST = rdr["gstper"].ToString(),
                        PublishName = rdr["publishname"].ToString(),
                        ISBN = rdr["isbn"].ToString(),
                        OtherDetails = rdr["otherdetails"].ToString(),
                        MagzineStatus = rdr["mstatus"].ToString(),
                        Cdate = rdr["cdate"].ToString(),
                        Prices = Price(rdr["mid"].ToString())
                    });

                }
                return lst;
            }
        }

        private List<Price> Price(string mid)
        {
            List<Price> lst = new List<Price>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_get_magzine_pricemaster", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@mid", mid);               
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Price
                    {
                        MagzineId = rdr["mid"].ToString(),
                        PriceId = rdr["priceid"].ToString(),
                        Tenure = rdr["tenure"].ToString(),
                        Rate = rdr["rate"].ToString(),
                        Cdate = rdr["cdate"].ToString()                       
                    });

                }
                return lst;
            }
        }

        public List<Magzine> AllList(string mid)
        {
            List<Magzine> lst = new List<Magzine>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_get_tbl_magzine", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@mid", mid);                
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Magzine
                    {
                        MagzineId = rdr["mid"].ToString(),
                        MagzineCode = rdr["mcode"].ToString(),
                        MagzineName = rdr["mname"].ToString(),
                        MagzineType = rdr["mtype"].ToString(),
                        Frequency = rdr["frequency"].ToString(),
                        GST = rdr["gstper"].ToString(),
                        PublishName = rdr["publishname"].ToString(),
                        ISBN = rdr["isbn"].ToString(),
                        OtherDetails = rdr["otherdetails"].ToString(),
                        MagzineStatus = rdr["mstatus"].ToString(),
                        Cdate = rdr["cdate"].ToString(),
                        Prices = Price(rdr["mid"].ToString())
                    });

                }
                return lst;
            }
        }

        public void Update(Magzine obj)
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
                cmd.CommandText = "usp_update_tbl_magzine";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();
                if (To_tbl_price(cmd, obj.MagzineId, obj) == false) { return; };
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Record has been Added Successfully !";

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

        public void Delete(int magzineId)
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
                cmd.CommandText = "usp_delete_tbl_magzine";
                cmd.Parameters.AddWithValue("@mid", magzineId);
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
                else if (ex.Message.Contains("fk_tbl_magzine_tbl_magzine_release") == true)
                {
                    Message = "This Magzine Code Exist in Released Magazine .";
                }

                
                else
                {
                    //Message = ex.Message;
                    Message = "User take Subscriptions of this Magazine.";
                }
            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }

        public List<MagzineRelease> AllReleaseListPagewise(JqueryDatatableParam param)
        {
            List<MagzineRelease> lst = new List<MagzineRelease>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_magzine_release_pagewise", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@displaylength", 10000000);
                com.Parameters.AddWithValue("@displayStart", 0);
                com.Parameters.AddWithValue("@sortcol", param.iSortCol_0);
                com.Parameters.AddWithValue("@sortdir", param.sSortDir_0);
                com.Parameters.AddWithValue("@search", param.sSearch);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new MagzineRelease
                    {
                        MagzineReleaseId = rdr["mrid"].ToString(),
                        MagzineId = rdr["mid"].ToString(),
                        MagzineNumber = rdr["mcode"].ToString(),
                        MagzineName = rdr["mname"].ToString(),
                        MagzineReleaseTitle = rdr["mrtitle"].ToString(),
                        DisplayTitle = rdr["dtitle"].ToString(),
                        MagzineReleaseCode = rdr["releasecode"].ToString(),
                        Description = rdr["descrp"].ToString(),
                        PdfUrl = rdr["pdf"].ToString(),
                        VideoUrl = rdr["vieourl"].ToString(),
                        CoverImageUrl = rdr["coverimg"].ToString(),
                        ThumbnailUrl = rdr["thumnail"].ToString(),
                        ReleaseDate = rdr["releasedate"].ToString(),
                        ReleaseMonth = rdr["releasedmonth"].ToString(),
                        ReleaseYear = rdr["releaseyear"].ToString(),
                        Publish = rdr["ispublish"].ToString(),
                        Stock = rdr["stock"].ToString()
                    });
                }
                return lst;
            }
        }

        public List<MagzineRelease> GetMagzineReleaseDetails(string mRId)
        {
            List<MagzineRelease> lst = new List<MagzineRelease>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_get_tbl_magzine_release", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@mrid", mRId);                
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new MagzineRelease
                    {
                        MagzineReleaseId = rdr["mrid"].ToString(),
                        MagzineId = rdr["mid"].ToString(),
                        MagzineName = rdr["mname"].ToString(),
                        MagzineReleaseTitle = rdr["mrtitle"].ToString(),
                        DisplayTitle = rdr["dtitle"].ToString(),
                        MagzineReleaseCode = rdr["releasecode"].ToString(),
                        Description = rdr["descrp"].ToString(),
                        PdfUrl = rdr["pdf"].ToString(),
                        VideoUrl = rdr["vieourl"].ToString(),
                        CoverImageUrl = rdr["coverimg"].ToString(),
                        ThumbnailUrl = rdr["thumnail"].ToString(),
                        ReleaseDate = Convert.ToDateTime(rdr["releasedate"]).ToString("yyyy-MM-dd"),
                        ReleaseMonth = rdr["releasedmonth"].ToString(),
                        ReleaseYear = rdr["releaseyear"].ToString(),
                        Stock = rdr["stock"].ToString()
                    });
                }
                return lst;
            }
        }

        public List<MagzineArticle> AllArticleListPagewise(JqueryDatatableParam param ,string mrid)
        {
            List<MagzineArticle> lst = new List<MagzineArticle>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_magzine_article_pagewise", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@displaylength", 10000000);
                com.Parameters.AddWithValue("@displayStart", 0);
                com.Parameters.AddWithValue("@sortcol", param.iSortCol_0);
                com.Parameters.AddWithValue("@sortdir", param.sSortDir_0);
                com.Parameters.AddWithValue("@search", param.sSearch);
                com.Parameters.AddWithValue("@mrid", mrid);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new MagzineArticle
                    {
                        arid = rdr["arid"].ToString(),
                        releasedcode = rdr["releasecode"].ToString(),
                        releadetittle = rdr["mrtitle"].ToString(),
                        artitle = rdr["artitle"].ToString(),
                        atitlecode = rdr["atitlecode"].ToString(),
                        ispublish = rdr["ispublish"].ToString()
                    });
                }
                return lst;
            }
        }

        public void DeleteArticle(int arid)
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
                cmd.CommandText = "usp_delete_tbl_magzine_articles";
                cmd.Parameters.AddWithValue("@arid", arid);
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

        public object GetMagzineArticleDetails(string arid)
        {
            List<MagzineArticle> lst = new List<MagzineArticle>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_get_tbl_magzine_article", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@arid", arid);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new MagzineArticle
                    {
                        arid = rdr["arid"].ToString(),
                        mrid = rdr["mrid"].ToString(),
                        releasedcode = rdr["releasecode"].ToString(),
                        releadetittle = rdr["mrtitle"].ToString(),
                        atitlecode = rdr["atitlecode"].ToString(),
                        artitle = rdr["artitle"].ToString(),
                        descrp = rdr["descrp"].ToString(),
                        arurl = rdr["arurl"].ToString(),
                        CoverImageUrl = rdr["coverimg"].ToString(),
                        ThumbnailUrl = rdr["thumnail"].ToString(),  
                        ispublish = rdr["ispublish"].ToString(),
                        publishdate = Convert.ToDateTime(rdr["releasedate"]).ToString("yyyy-MM-dd"),


                    });
                }
                return lst;
            }
        }

        public void DeleteReleaseMagzine(int mRId)
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
                cmd.CommandText = "usp_delete_tbl_magzine_release";
                cmd.Parameters.AddWithValue("@mrid", mRId);
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
                    Message = "This Released Magazine Exit in Article or In Subscriptions";
                }
            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }

        public void DeletePriceaster(int magzineId, int priceid)
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
                cmd.CommandText = "delete_magzine_pricemaster_new";
                cmd.Parameters.AddWithValue("@mzid", magzineId);
                cmd.Parameters.AddWithValue("@priceid", priceid);
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
                else if (ex.Message.Contains("FK_tbl_magazine_tbl_pricemaster") == true)
                {
                    Message = "This Price Cannot bo delete beacuse it exist in Subscriptions ";
                }
                else
                {
                    Message = "404 Internal Error";
                }
            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }
    }
}
