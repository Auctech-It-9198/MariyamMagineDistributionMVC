using BOL_Business_Objects_Layer_;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_
{
    public class CompanyDb
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

        public List<Company> AllList()
        {
            List<Company> lst = new List<Company>();
            using (SqlConnection conn = DbConnection.getConnection())
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); };                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;               
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_display_tbl_company";                          
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Company
                    {
                        Compcode = Convert.ToInt32(rdr["compcode"]),
                        CompName = rdr["cmpname"].ToString().Trim(),
                        Address1 = rdr["address1"].ToString().Trim(),
                        Address2 = rdr["address2"].ToString().Trim(),
                        GSTin = rdr["gstin"].ToString().Trim(),
                        PAN = rdr["panno"].ToString().Trim(),
                        Mobile = rdr["mobileno"].ToString().Trim(),
                        Email = rdr["email"].ToString().Trim(),
                        Website = rdr["website"].ToString().Trim(),
                        Footer1 = rdr["footer1"].ToString().Trim(),
                        Footer2 = rdr["footer2"].ToString().Trim(),
                        Footer3 = rdr["footer3"].ToString().Trim(),
                        Footer4 = rdr["footer4"].ToString().Trim(),
                        Footer5 = rdr["footer5"].ToString().Trim(),
                    });

                }
                return lst;
            }
        }
    }
}
