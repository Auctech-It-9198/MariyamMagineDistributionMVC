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
    public class SubscriberDb
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
        public void Add(Subscriber obj)
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


                if (obj.partytype == "p")
                {
                    obj.SubscriberId = sqlobj.getpartyId(cmd);
                }
                else
                {
                    obj.SubscriberId = sqlobj.getSubscriberId(cmd);
                }

                cmd.Parameters.Clear();
                cmd.CommandText = "usp_insert_tbl_membership";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();
                if (To_tbl_address(cmd, obj.SubscriberId, obj) == false) { return; };
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
        private void AddCommandParameter(SqlCommand cmd, Subscriber obj)
        {
            cmd.Parameters.AddWithValue("@mscode", obj.SubscriberId);
            cmd.Parameters.AddWithValue("@name", obj.Name.Trim());
            cmd.Parameters.AddWithValue("@mobile", obj.Mobile.Trim());
            cmd.Parameters.AddWithValue("@WhatsAppNo", obj.WhatsAppNo.Trim());
            cmd.Parameters.AddWithValue("@dob", obj.DOB );
            cmd.Parameters.AddWithValue("@email", obj.Email != null ? obj.Email.Trim() : "");
            cmd.Parameters.AddWithValue("@education", obj.Education != null ? obj.Education.Trim() : "");
            cmd.Parameters.AddWithValue("@jobprofile", obj.JobProfile != null ? obj.JobProfile.Trim() : "");
            cmd.Parameters.AddWithValue("@gender", obj.Gender != null ? obj.Gender : "0");
            cmd.Parameters.AddWithValue("@age", obj.Age != null ? obj.Age : "0");
            cmd.Parameters.AddWithValue("@marital", obj.Marital != null ? obj.Marital : "0");
            cmd.Parameters.AddWithValue("@membertypeid", obj.MemberTypeId !=null ? obj.MemberTypeId : null);
            cmd.Parameters.AddWithValue("@isForwarder", obj.IsForwarder !=null ? obj.IsForwarder: "true");
            cmd.Parameters.AddWithValue("@parentid", obj.ParentId !=null ? obj.ParentId : "");
            cmd.Parameters.AddWithValue("@IsDeActive", obj.IsDeActive);
            cmd.Parameters.AddWithValue("@discount", obj.Discount);

            cmd.Parameters.AddWithValue("@partytype", obj.partytype.Trim());
            cmd.Parameters.AddWithValue("@gstin", obj.gstin.Trim());
            cmd.Parameters.AddWithValue("@contactperson", obj.contactperson.Trim());


        }
        private bool To_tbl_address(SqlCommand cmd, string SubscriberId, Subscriber obj)
        {
            bool res = false;
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "usp_delete_tbl_membership_address";
                cmd.Parameters.Add("@mscode", SqlDbType.NVarChar).Value = SubscriberId;
                cmd.ExecuteNonQuery();
                foreach (Address data in obj.address)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "usp_insert_tbl_membership_address";
                    cmd.Parameters.AddWithValue("@mscode", SqlDbType.NVarChar).Value = SubscriberId;
                    cmd.Parameters.AddWithValue("@countryid", SqlDbType.Int).Value = data.CountryId;
                    cmd.Parameters.AddWithValue("@stateid", SqlDbType.Int).Value = data.StateId;
                    cmd.Parameters.AddWithValue("@cityid", SqlDbType.Int).Value = data.CityId;
                    cmd.Parameters.AddWithValue("@areaid", SqlDbType.Int).Value = data.AreaId;
                    cmd.Parameters.AddWithValue("@pincode", SqlDbType.NVarChar).Value = data.Pincode;
                    cmd.Parameters.AddWithValue("@landkamr", SqlDbType.NVarChar).Value = data.LandMark;
                    cmd.Parameters.AddWithValue("@localaddress", SqlDbType.NVarChar).Value = data.LocalAddress;                            
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

        public List<Subscriber> AllListPagewise(JqueryDatatableParam param, int cid, int sid, int cityid, string deactive, string forworder, string mtype, string gender, string ptype)
        {
            List<Subscriber> lst = new List<Subscriber>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_membership_pagewise", con);
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
                com.Parameters.AddWithValue("@ptype", ptype);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Subscriber
                    {
                        SubscriberId = rdr["mscode"].ToString(),
                        Name = rdr["name"].ToString(),
                        Mobile = rdr["mobile"].ToString(),
                        WhatsAppNo = rdr["WhatsAppNo"].ToString(),
                        DOB = rdr["dob"].ToString(),
                        Email = rdr["email"].ToString(),
                        Education = rdr["education"].ToString(),
                        JobProfile = rdr["jobprofile"].ToString(),
                        Gender = rdr["gender"].ToString(),
                        Age = rdr["age"].ToString(),
                        Marital = rdr["marital"].ToString(),
                        MemberTypeId = rdr["membertypeid"].ToString(),
                        MemberType = rdr["membertype"].ToString(),
                        IsForwarder = rdr["isForwarder"].ToString(),
                        ParentId = rdr["parentid"].ToString(),
                        IsDeActive = rdr["IsDeActive"].ToString(),
                        Discount = rdr["discount"].ToString(),
                        address = Address(rdr["mscode"].ToString())
                    });

                }
                return lst;
            }
        }

        private List<Address> Address(string mscode)
        {
            List<Address> lst = new List<Address>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_get_tbl_membership_address", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@mscode", mscode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Address
                    {
                        CountryId = rdr["countryid"].ToString(),
                        CountryName = rdr["cname"].ToString(),
                        StateId = rdr["stateid"].ToString(),
                        StateName = rdr["statename"].ToString(),
                        CityId = rdr["cityid"].ToString(),
                        CityName = rdr["cityname"].ToString(),
                        AreaId = rdr["areaid"].ToString(),
                        AreaName = rdr["areaname"].ToString(),
                        Pincode = rdr["pincode"].ToString(),
                        LandMark = rdr["landkamr"].ToString(),
                        LocalAddress = rdr["localaddress"].ToString()
                    });

                }
                return lst;
            }
        }

        public List<Subscriber> AllList(string mscode)
        {
            List<Subscriber> lst = new List<Subscriber>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_get_tbl_membership", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@mscode", mscode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Subscriber
                    {
                        SubscriberId = rdr["mscode"].ToString().Trim(),
                        Name = rdr["name"].ToString().Trim(),
                        Mobile = rdr["mobile"].ToString().Trim(),
                        WhatsAppNo = rdr["WhatsAppNo"].ToString().Trim(),
                        DOB = rdr["dob"].ToString(),
                        Email = rdr["email"].ToString(),
                        Education = rdr["education"].ToString(),
                        JobProfile = rdr["jobprofile"].ToString(),
                        Gender = rdr["gender"].ToString(),
                        Age = rdr["age"].ToString(),
                        Marital = rdr["marital"].ToString(),
                        MemberTypeId = rdr["membertypeid"].ToString(),
                        MemberType = rdr["membertype"].ToString(),
                        IsForwarder = rdr["isForwarder"].ToString(),
                        ParentId = rdr["parentid"].ToString(),
                        IsDeActive = rdr["IsDeActive"].ToString(),
                        Discount = rdr["discount"].ToString(),
                        address = Address(rdr["mscode"].ToString()),
                        gstin = rdr["gstin"].ToString(),
                        contactperson = rdr["contactperson"].ToString(),

                    });

                }
                return lst;
            }
        }

        public void Update(Subscriber obj)
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
                cmd.CommandText = "usp_update_tbl_membership";
                AddCommandParameter(cmd, obj);
                cmd.ExecuteNonQuery();
                if (To_tbl_address(cmd, obj.SubscriberId, obj) == false) { return; };
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

        public void Delete(string mscode)
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
                cmd.CommandText = "usp_delete_tbl_membership";
                cmd.Parameters.AddWithValue("@mscode", mscode);
                cmd.ExecuteNonQuery();
                trn.Commit();
                Result = true;
                Message = "Congratulation ! Record Delete Successfully";
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
                    Message = "This Subscriber exists in other records.";
                }
            }
            finally
            {
                if (conn != null) { conn.Close(); };
            }
        }

        public List<Subscriber> GetForwarderChildListDetails(string mscode)
        {
            List<Subscriber> lst = new List<Subscriber>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_get_tbl_membership_child", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@mscode", mscode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Subscriber
                    {
                        SubscriberId = rdr["mscode"].ToString(),
                        Name = rdr["name"].ToString(),
                        Mobile = rdr["mobile"].ToString(),
                        WhatsAppNo = rdr["WhatsAppNo"].ToString(),
                        DOB = rdr["dob"].ToString(),
                        Email = rdr["email"].ToString(),
                        Education = rdr["education"].ToString(),
                        JobProfile = rdr["jobprofile"].ToString(),
                        Gender = rdr["gender"].ToString(),
                        Age = rdr["age"].ToString(),
                        Marital = rdr["marital"].ToString(),
                        MemberTypeId = rdr["membertypeid"].ToString(),
                        MemberType = rdr["membertype"].ToString(),
                        IsForwarder = rdr["isForwarder"].ToString(),
                        ParentId = rdr["parentid"].ToString(),
                        IsDeActive = rdr["IsDeActive"].ToString(),
                        Discount = rdr["discount"].ToString(),
                        address = Address(rdr["mscode"].ToString())
                    });

                }
                return lst;
            }
        }
    }
}
