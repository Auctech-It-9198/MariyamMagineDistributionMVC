using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL_Business_Objects_Layer_
{
    public class SqlObject
    {
        public string getNewId(SqlCommand cmd, string tblname, string colname)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "usp_generate_new_id";
            cmd.Parameters.AddWithValue("@tblname", DbType.String).Value = tblname.ToString();
            cmd.Parameters.AddWithValue("@colname", DbType.String).Value = colname;
            return getFromDatabase(cmd, cmd.Connection);
        }
        public string getVno(SqlCommand cmd, string tblname, string colname)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "usp_generate_new_vno";
            cmd.Parameters.AddWithValue("@tblname", DbType.String).Value = tblname.ToString();
            cmd.Parameters.AddWithValue("@colname", DbType.String).Value = colname;
            return getFromDatabase(cmd, cmd.Connection);
        }
        public string getFromDatabase(SqlCommand cmd, SqlConnection conn)
        {
            string ret = "";
            try
            {
                ret = cmd.ExecuteScalar().ToString();
                if (ret == null) { return ""; };
            }
            catch { }
            return ret;
        }
        public string getFromDatabase(SqlCommand cmd)
        {
            string ret = "";
            try
            {
                SqlConnection conn = DbConnection.getConnection();
                if (conn.State == ConnectionState.Closed) { conn.Open(); };
                cmd.Connection = conn;
                ret = cmd.ExecuteScalar().ToString();
                if (ret == null) { ret = ""; };
                if (conn != null) { conn.Close(); };
            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            return ret;
        }

        public string getSubscriberId(SqlCommand cmd)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "usp_generate_mscode";
            return getFromDatabase(cmd, cmd.Connection);
        }



        public string getpartyId(SqlCommand cmd)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "usp_generate_partycode";
            return getFromDatabase(cmd, cmd.Connection);
        }




    }
}
