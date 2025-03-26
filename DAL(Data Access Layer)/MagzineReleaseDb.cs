using BOL_Business_Objects_Layer_;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_
{
    public class MagzineReleaseDb
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


        public void Add(MagzineRelease obj)
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
                obj.MagzineReleaseId = sqlobj.getNewId(cmd, "tbl_magzine_release", "mrid");
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_magzine_release";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();


                //delete ledger
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_delete_tbl_master_ledger";
                cmd.Parameters.AddWithValue("@vno", obj.MagzineReleaseId);
                cmd.Parameters.AddWithValue("@vtype", "Si");
                cmd.ExecuteNonQuery();


                // into ledger
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_master_ledger";
                cmd.Parameters.AddWithValue("@ordeno", "0");
                cmd.Parameters.AddWithValue("@vno", obj.MagzineReleaseId);
                cmd.Parameters.AddWithValue("@lvdate", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@lvtype", "Si");
                cmd.Parameters.AddWithValue("@msid", "0");
                cmd.Parameters.AddWithValue("@mscode", null);
                cmd.Parameters.AddWithValue("@mzid", obj.MagzineId);
                cmd.Parameters.AddWithValue("@mzname", obj.MagzineName);
                cmd.Parameters.AddWithValue("@mrid", obj.MagzineReleaseId);
                cmd.Parameters.AddWithValue("@mrname ", obj.DisplayTitle);
                cmd.Parameters.AddWithValue("@selecttype ", "0");
                cmd.Parameters.AddWithValue("@price", 0);
                cmd.Parameters.AddWithValue("@rate", 0);
                cmd.Parameters.AddWithValue("@qty", obj.Stock);
                cmd.Parameters.AddWithValue("@gstper ", 0);
                cmd.Parameters.AddWithValue("@gstamt", 0);
                cmd.Parameters.AddWithValue("@disper", 0);
                cmd.Parameters.AddWithValue("@disamt", 0);
                cmd.Parameters.AddWithValue("@paybleamt ", 0);
                cmd.Parameters.AddWithValue("@totalamt", 0);
                cmd.Parameters.AddWithValue("@lstatsu", 0); //0 means not dispatch
                cmd.Parameters.AddWithValue("@trantype", "s"); 

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
        private void AddCommandParameter(SqlCommand cmd, MagzineRelease obj)
        {
            cmd.Parameters.AddWithValue("@mrid", obj.MagzineReleaseId);
            cmd.Parameters.AddWithValue("@mid", obj.MagzineId);
            cmd.Parameters.AddWithValue("@mrtitle", obj.MagzineReleaseTitle);
            cmd.Parameters.AddWithValue("@dtitle", obj.DisplayTitle);
            cmd.Parameters.AddWithValue("@releasecode", obj.MagzineReleaseCode);
            cmd.Parameters.AddWithValue("@descrp", obj.Description);
            cmd.Parameters.AddWithValue("@pdf", obj.PdfUrl);
            cmd.Parameters.AddWithValue("@vieourl", obj.VideoUrl);
            cmd.Parameters.AddWithValue("@coverimg", obj.CoverImageUrl);
            cmd.Parameters.AddWithValue("@thumnail", obj.ThumbnailUrl);
            cmd.Parameters.AddWithValue("@releasedate", obj.ReleaseDate);
            cmd.Parameters.AddWithValue("@releasedmonth", obj.ReleaseMonth);
            cmd.Parameters.AddWithValue("@releaseyear", obj.ReleaseYear);
            cmd.Parameters.AddWithValue("@ispublish", obj.Publish);
            cmd.Parameters.AddWithValue("@stock", obj.Stock);
        }

        public void UpdateReleaseMagzine(MagzineRelease obj)
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
                cmd.CommandText = "usp_update_tbl_magzine_release";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();



                //delete ledger
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_delete_tbl_master_ledger";
                cmd.Parameters.AddWithValue("@vno", obj.MagzineReleaseId);
                cmd.Parameters.AddWithValue("@vtype", "Si");
                cmd.ExecuteNonQuery();


                // into ledger
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_master_ledger";
                cmd.Parameters.AddWithValue("@ordeno", "0");
                cmd.Parameters.AddWithValue("@vno", obj.MagzineReleaseId);
                cmd.Parameters.AddWithValue("@lvdate", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@lvtype", "Si");
                cmd.Parameters.AddWithValue("@msid", "0");
                cmd.Parameters.AddWithValue("@mscode", null);
                cmd.Parameters.AddWithValue("@mzid", obj.MagzineId);
                cmd.Parameters.AddWithValue("@mzname", obj.MagzineName);
                cmd.Parameters.AddWithValue("@mrid", obj.MagzineReleaseId);
                cmd.Parameters.AddWithValue("@mrname ", obj.DisplayTitle);
                cmd.Parameters.AddWithValue("@selecttype ", "0");
                cmd.Parameters.AddWithValue("@price", 0);
                cmd.Parameters.AddWithValue("@rate", 0);
                cmd.Parameters.AddWithValue("@qty", obj.Stock);
                cmd.Parameters.AddWithValue("@gstper ", 0);
                cmd.Parameters.AddWithValue("@gstamt", 0);
                cmd.Parameters.AddWithValue("@disper", 0);
                cmd.Parameters.AddWithValue("@disamt", 0);
                cmd.Parameters.AddWithValue("@paybleamt ", 0);
                cmd.Parameters.AddWithValue("@totalamt", 0);
                cmd.Parameters.AddWithValue("@lstatsu", 0); //0 means not dispatch
                cmd.Parameters.AddWithValue("@trantype", "s");
                cmd.ExecuteNonQuery();



                trn.Commit();
                Result = true;
                Message = "Congratulation ! Record has been Updated Successfully !";

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
