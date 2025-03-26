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
    public class CountryDb
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
        public void Add(Country obj)
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
                cmd.CommandText = "usp_insert_tbl_country";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();                
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Records has been Added Successfully !";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_country") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_country";
                }
                else if (ex.Message.Contains("uk_tbl_country") == true)
                {
                    Message = "This Country is Already Exist.";
                }
                else if (ex.Message.Contains("fk_tbl_country_tbl_company") == true)
                {
                    Message = "Company Not Found (Compcode is Null)";
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
        public void Update(Country obj)
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
                cmd.CommandText = "usp_update_tbl_country";
                cmd.Parameters.AddWithValue("@cid", obj.CountryId);
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();               
                trn.Commit();
                Result = true;
                Message = "Record Update Successfully";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_country") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_country";
                }
                else if (ex.Message.Contains("uk_tbl_country") == true)
                {
                    Message = "This Country is Already Exist.";
                }
                else if (ex.Message.Contains("fk_tbl_country_tbl_company") == true)
                {
                    Message = "Company Not Found (Compcode is Null)";
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
        public List<Country> AllList(string compcode)
        {
            List<Country> lst = new List<Country>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_display_tbl_country", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@compcode", SqlDbType.Int).Value = compcode;               
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Country
                    {

                        CountryId = Convert.ToInt16(rdr["cid"].ToString()),
                        CountryName = rdr["countryname"].ToString(),
                        Compcode = rdr["compcode"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }
        private void AddCommandParameter(SqlCommand cmd, Country obj)
        {
            cmd.Parameters.AddWithValue("@compcode", obj.Compcode);
            cmd.Parameters.AddWithValue("@countryname", obj.CountryName);
        }

        public void Delete(int countryId, string compcode)
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
                cmd.CommandText = "usp_delete_tbl_country";
                cmd.Parameters.AddWithValue("@cid", countryId);
                cmd.Parameters.AddWithValue("@compcode", compcode);
                cmd.ExecuteNonQuery();
                trn.Commit();
                Result = true;
                Message = "Record Delete Successfully";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_country") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_country";
                }
                else if (ex.Message.Contains("uk_tbl_country") == true)
                {
                    Message = "This Country is Already Exist.";
                }
                else if (ex.Message.Contains("fk_tbl_country_tbl_company") == true)
                {
                    Message = "Company Not Found (Compcode is Null)";
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
