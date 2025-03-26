using BOL_Business_Objects_Layer_;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;
using System.Reflection.Emit;

namespace DAL_Data_Access_Layer_
{
    public class UsersDb
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
        public void Add(Users obj)
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
                obj.UserId = sqlobj.getNewId(cmd, "tbl_users", "userid");
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_users";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();
                To_tbl_compusers(cmd, obj.UserId, obj);
                if (To_tbl_compusers(cmd, obj.UserId, obj) == false) { return; };
                trn.Commit();
                Result = true;
                Message = "Congratulation ! User has been Added Successfully !";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("pk_tbl_users") == true)
                {
                    Message = "Duplicate Entry Not Allowed. with pk_tbl_users ";
                }
                else if (ex.Message.Contains("uk_tbl_users") == true)
                {
                    Message = "This Account is Already Exist.";
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

        private bool To_tbl_compusers(SqlCommand cmd, string userId, Users obj)
        {
            bool res = false;
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_delete_tbl_compusers";
                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                cmd.ExecuteNonQuery();
                foreach (CompYear data in obj.compyears)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "usp_insert_tbl_compusers";
                    cmd.Parameters.AddWithValue("@userid", SqlDbType.Int).Value = userId;
                    cmd.Parameters.AddWithValue("@compcode", SqlDbType.Int).Value = data.CompCode;
                    cmd.ExecuteNonQuery();
                }
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;            
        }

        private bool chkuserexists(int userId, string password)
        {
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_exists_tbl_users", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", SqlDbType.Int).Value = userId;
                cmd.Parameters.AddWithValue("@passw", SqlDbType.NVarChar).Value = password;
                bool  rdr = Convert.ToBoolean(cmd.ExecuteScalar());                
                return rdr;
            }
        }

        private void AddCommandParameter(SqlCommand cmd, Users obj)
        {
            cmd.Parameters.AddWithValue("@userid", obj.UserId);
            cmd.Parameters.AddWithValue("@username", obj.UserName);
            cmd.Parameters.AddWithValue("@passw", obj.Password);
            cmd.Parameters.AddWithValue("@logintype", obj.UserType);
            cmd.Parameters.AddWithValue("@registered", obj.Registered);
            cmd.Parameters.AddWithValue("@Latest_Activity", obj.LastActivity);

            cmd.Parameters.AddWithValue("@ustatus", obj.UserStatus);
            cmd.Parameters.AddWithValue("@IsLocked", obj.IsLocked);
            cmd.Parameters.AddWithValue("@loginCnt", obj.LoginCount);
        }

        //public List<Login> UserLogin(string username, string password)
        //{           
        //    List<Login> lst = new List<Login>();
        //    using (SqlConnection con = DbConnection.getConnection())
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("usp_login_tbl_users", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@username", SqlDbType.Int).Value = username;
        //        cmd.Parameters.AddWithValue("@passw", SqlDbType.NVarChar).Value = password;
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            if (rdr.GetSchemaTable().Rows.OfType<DataRow>().Any(row => row["ColumnName"].ToString() == "userid"))
        //            {
        //                lst.Add(new Login
        //                {

        //                    UserId = rdr["userid"].ToString(),
        //                    UserName = rdr["username"].ToString(),
        //                    Password = rdr["passw"].ToString(),
        //                    UserType = rdr["logintype"].ToString(),
        //                    CompCode = rdr["compcode"].ToString(),
        //                    CompName = rdr["cmpname"].ToString(),
        //                    Mobile = rdr["mobileno"].ToString(),
        //                    Address1 = rdr["address1"].ToString(),
        //                    Address2 = rdr["address2"].ToString(),
        //                    RES = rdr["RES"].ToString(),
        //                });
        //            }
        //            else
        //            {
        //                lst.Add(new Login
        //                {
        //                    RES = rdr["RES"].ToString(),
        //                });
        //            }
        //        }
        //        return lst;
        //    }
        //}


        public List<Login> UserLogin(string username, string password)
        {
            List<Login> lst = new List<Login>();
            try
            {
                using (SqlConnection con = DbConnection.getConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_login_tbl_users", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 60 // Increase timeout if necessary
                    };

                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar)).Value = username;
                    cmd.Parameters.Add(new SqlParameter("@passw", SqlDbType.NVarChar)).Value = password;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (rdr["userid"] != DBNull.Value)
                            {
                                lst.Add(new Login
                                {
                                    UserId = rdr["userid"].ToString(),
                                    UserName = rdr["username"].ToString(),
                                    Password = rdr["passw"].ToString(),
                                    UserType = rdr["logintype"].ToString(),
                                    CompCode = rdr["compcode"].ToString(),
                                    CompName = rdr["cmpname"].ToString(),
                                    Mobile = rdr["mobileno"].ToString(),
                                    Address1 = rdr["address1"].ToString(),
                                    Address2 = rdr["address2"].ToString(),
                                    RES = rdr["RES"]?.ToString(),
                                });
                            }
                            else
                            {
                                lst.Add(new Login
                                {
                                    RES = rdr["RES"]?.ToString(),
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exception details
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                Console.WriteLine($"Stack Trace: {sqlEx.StackTrace}");
                return lst; // Return empty list in case of error
            }
            catch (Exception ex)
            {
                // Log general exception details
                Console.WriteLine($"General Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return lst; // Return empty list in case of error
            }
            return lst;
        }



        public List<Users> AllList()
        {
            List<Users> lst = new List<Users>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_display_tbl_users", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@username", SqlDbType.Int).Value = username;
                //cmd.Parameters.AddWithValue("@passw", SqlDbType.NVarChar).Value = password;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Users
                    {

                        UserId = rdr["userid"].ToString(),
                        UserName = rdr["username"].ToString(),
                        Password = rdr["passw"].ToString(),
                        UserType = rdr["logintype"].ToString() == "0" ? "Admin" : "Staff",
                        Registered = Convert.ToDateTime(rdr["registered"]).ToString("dd/MM/yyyy hh:mm tt"),
                        LastActivity = Convert.ToDateTime(rdr["Latest_Activity"]).ToString("dd/MM/yyyy hh:mm tt"),
                        UserStatus = Convert.ToBoolean(rdr["ustatus"]) == true ? "Active" : "De-Active",
                        IsLocked = rdr["IsLocked"].ToString(),
                        LoginCount = rdr["loginCnt"].ToString(),
                        compyears = CompYear(rdr["userid"].ToString())
                    });
                }
                con.Close();
                return lst;
            }
        }

        private List<CompYear> CompYear(string UserId)
        {
            List<CompYear> lst = new List<CompYear>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_get_tbl_compusers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", SqlDbType.Int).Value = UserId;                
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new CompYear
                    {
                        UserId = rdr["userid"].ToString(),
                        CompCode = rdr["compcode"].ToString(),
                        CompName = rdr["cmpname"].ToString()
                    });
                }
                return lst;
            }
        }

        public void Update(Users objuser)
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
                cmd.CommandText = "usp_update_tbl_users";
                AddCommandParameter(cmd, objuser);                
                cmd.ExecuteNonQuery();               
                if (To_tbl_compusers(cmd, objuser.UserId, objuser) == false) { return; };
                trn.Commit();
                Result = true;
                Message = "Record Update Successfully";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("uk_tbl_users") == true)
                {
                    Message = "Duplicate entry not allowed!";
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

        public void AddPermission(Menu objmenu)
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
                cmd.CommandText = "usp_delete_permission";
                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = objmenu.UserId;
                cmd.Parameters.Add("@compcode", SqlDbType.Int).Value = objmenu.Compcode;
                cmd.Parameters.Add("@mid", SqlDbType.Int).Value = objmenu.MenuId;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_menu_permission";
                AddMenuParameter(cmd, objmenu);
                cmd.ExecuteNonQuery();
                if (To_tbl_SubMenu(cmd, objmenu.UserId, objmenu.Compcode, objmenu) == false) { return; };
                trn.Commit();
                Result = true;
                Message = "Record Add Successfully";
            }
            catch (Exception ex)
            {
                trn.Rollback();
                if (ex.Message.Contains("uk_tbl_users") == true)
                {
                    Message = "Duplicate entry not allowed!";
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

        private bool To_tbl_SubMenu(SqlCommand cmd, string userId,string compcode, Menu objmenu)
        {
            bool res = false;
            try
            {                
                foreach (SubMenu data in objmenu.submenus)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "usp_insert_tbl_userpermission";
                    cmd.Parameters.AddWithValue("@compcode", SqlDbType.Int).Value = compcode;
                    cmd.Parameters.AddWithValue("@userid", SqlDbType.Int).Value = userId;
                    cmd.Parameters.AddWithValue("@mid", SqlDbType.Int).Value = data.MenuId;
                    cmd.Parameters.AddWithValue("@smid", SqlDbType.Int).Value = data.SubMenuId;
                    cmd.Parameters.AddWithValue("@addper", SqlDbType.Bit).Value = data.AddPer;
                    cmd.Parameters.AddWithValue("@editper", SqlDbType.Bit).Value = data.EditPer;
                    cmd.Parameters.AddWithValue("@deleteper", SqlDbType.Bit).Value = data.DeletePer;
                    cmd.Parameters.AddWithValue("@viewper", SqlDbType.Bit).Value = data.ViewPer;
                    cmd.Parameters.AddWithValue("@printper", SqlDbType.Bit).Value = data.PrintPer;
                    cmd.Parameters.AddWithValue("@menuper", SqlDbType.Bit).Value = data.SubMenuPer;
                    cmd.ExecuteNonQuery();
                }
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;
        }

        private void AddMenuParameter(SqlCommand cmd, Menu objmenu)
        {
            cmd.Parameters.AddWithValue("@compcode", objmenu.Compcode);
            cmd.Parameters.AddWithValue("@userid", objmenu.UserId);
            cmd.Parameters.AddWithValue("@mid", objmenu.MenuId);
            cmd.Parameters.AddWithValue("@menuper", objmenu.MenuPer);
            cmd.Parameters.AddWithValue("@approve", objmenu.IsApprove);
        }
    }
}
