using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL_Business_Objects_Layer_
{
    public class DbConnection
    {
        public SqlConnection con = new SqlConnection();
        public SqlDataAdapter adp = new SqlDataAdapter();
        public static SqlConnection getConnection()
        {
            string strcon = @"Data Source='103.228.113.77';Initial Catalog=Maryamsoft; user id=maryamsoft;password=formaryamsoft;";
            //string strcon = @"Data Source=DESKTOP-URKDLG0\SQLEXPRESS;Initial Catalog=Mariyam_Web_DB;Integrated Security=True;";

            SqlConnection con = new SqlConnection(strcon);
            return con;
        }

    }
}
