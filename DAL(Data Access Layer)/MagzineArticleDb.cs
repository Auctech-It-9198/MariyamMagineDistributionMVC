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
    public class MagzineArticleDb
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
        public void Add(MagzineArticle obj)
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
                obj.arid = sqlobj.getNewId(cmd, "tbl_magzine_articles", "arid");
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_magzine_articles";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();
              
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
        private void AddCommandParameter(SqlCommand cmd, MagzineArticle obj)
        {
            cmd.Parameters.AddWithValue("@arid", obj.arid);
            cmd.Parameters.AddWithValue("@mrid", obj.mrid);
            cmd.Parameters.AddWithValue("@atitlecode", obj.atitlecode);
            cmd.Parameters.AddWithValue("@artitle", obj.artitle);
            cmd.Parameters.AddWithValue("@coverimg", obj.CoverImageUrl);
            cmd.Parameters.AddWithValue("@thumnail", obj.ThumbnailUrl);
            cmd.Parameters.AddWithValue("@descrp", obj.descrp);
            cmd.Parameters.AddWithValue("@arurl", obj.arurl);
            cmd.Parameters.AddWithValue("@releasedate", obj.publishdate);
            cmd.Parameters.AddWithValue("@ispublish", obj.ispublish);
          
        }
       


    

        public void UpdateMagzineArticle(MagzineArticle obj)
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
                cmd.CommandText = "usp_update_tbl_magzine_articles";
                cmd.Parameters.AddWithValue("@arid", obj.arid);
                cmd.Parameters.AddWithValue("@mrid", obj.mrid);
                cmd.Parameters.AddWithValue("@atitlecode", obj.atitlecode);
                cmd.Parameters.AddWithValue("@artitle", obj.artitle);
                cmd.Parameters.AddWithValue("@coverimg", obj.CoverImageUrl);
                cmd.Parameters.AddWithValue("@thumnail", obj.ThumbnailUrl);
                cmd.Parameters.AddWithValue("@descrp", obj.descrp);
                cmd.Parameters.AddWithValue("@arurl", obj.arurl);
                cmd.Parameters.AddWithValue("@releasedate", obj.publishdate);
                cmd.Parameters.AddWithValue("@ispublish", obj.ispublish);
                cmd.ExecuteNonQuery();

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
    }
}
