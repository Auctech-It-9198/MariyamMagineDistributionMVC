using BOL_Business_Objects_Layer_;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection.Emit;

namespace DAL_Data_Access_Layer_
{
    public class CommanDb
    {
        public List<Company> Display_CompanyList()
        {
            List<Company> lst = new List<Company>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("usp_display_tbl_company", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Company
                    {
                        UserId = Convert.ToInt32(rdr["userid"]),
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

        public List<Menu> Display_MenuList(int UserId, int Compcode)
        {
            List<Menu> lst = new List<Menu>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_display_tbl_menu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@compcode", SqlDbType.Int).Value = Compcode;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                   
                    lst.Add(new Menu
                    {
                        MenuId = rdr["mid"].ToString(),
                        MenuName = rdr["mname"].ToString(),
                        Li_Class = rdr["li_class"].ToString(),
                        A_Class = rdr["a_class"].ToString(),
                        URL = rdr["url"].ToString(),
                        Fa_Icon = rdr["fa_icon"].ToString(),
                        Arrow = rdr["arrow"].ToString(),
                        MenuOrder = rdr["morder"].ToString(),
                        MenuPer = rdr["menuper"].ToString(),
                        IsApprove = rdr["approve"].ToString(),
                        Compcode = rdr["compcode"].ToString(),
                        submenus = SubMenu(Compcode, UserId, rdr["mid"].ToString())
                    });
                }
                con.Close();
                return lst;
            }
        }

        private List<SubMenu> SubMenu(int UserId, int Compcode,string mid)
        {
            List<SubMenu> lst = new List<SubMenu>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_display_tbl_smenu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@compcode", SqlDbType.Int).Value = Compcode;
                cmd.Parameters.AddWithValue("@mid", SqlDbType.Int).Value = mid;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new SubMenu
                    {
                        MenuId = rdr["mid"].ToString(),
                        SubMenuId = rdr["smid"].ToString(),
                        SubMenuName = rdr["smname"].ToString(),
                        Li_Class = rdr["li_class"].ToString(),
                        A_Class = rdr["a_class"].ToString(),
                        URL = rdr["url"].ToString(),
                        Fa_Icon = rdr["fa_icon"].ToString(),
                        Arrow = rdr["arrow"].ToString(),
                        AddPer = rdr["addper"].ToString(),
                        EditPer = rdr["editper"].ToString(),
                        DeletePer = rdr["deleteper"].ToString(),
                        ViewPer = rdr["viewper"].ToString(),
                        PrintPer = rdr["printper"].ToString(),
                        SubMenuPer = rdr["menuper"].ToString(),
                        SubMenuOrder = rdr["smorder"].ToString(),
                        Compcode = rdr["compcode"].ToString()
                    });
                }
                return lst;
            }
        }

        public List<Users> Display_UserList()
        {
            List<Users> lst = new List<Users>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_display_tbl_users", con);
                cmd.CommandType = CommandType.StoredProcedure;                
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

        public List<Menu> GetMenuList_for_Permission(int userId, int compcode, int menuId)
        {
            List<Menu> lst = new List<Menu>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_get_tbl_menu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", SqlDbType.Int).Value = userId;
                cmd.Parameters.AddWithValue("@compcode", SqlDbType.Int).Value = compcode;
                cmd.Parameters.AddWithValue("@mid", SqlDbType.Int).Value = menuId;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Menu
                    {
                        MenuId = rdr["mid"].ToString(),
                        MenuName = rdr["mname"].ToString(),
                        Li_Class = rdr["li_class"].ToString(),
                        A_Class = rdr["a_class"].ToString(),
                        URL = rdr["url"].ToString(),
                        Fa_Icon = rdr["fa_icon"].ToString(),
                        Arrow = rdr["arrow"].ToString(),
                        MenuOrder = rdr["morder"].ToString(),
                        MenuPer = rdr["menuper"].ToString(),
                        IsApprove = rdr["approve"].ToString(),
                        Compcode = rdr["compcode"].ToString(),
                        submenus = SubMenu(compcode, userId, menuId.ToString())
                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<Menu> GetMenuListByUserId(int userId, int compcode)
        {
            List<Menu> lst = new List<Menu>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_get_tbl_menu_by_userid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", SqlDbType.Int).Value = userId;
                cmd.Parameters.AddWithValue("@compcode", SqlDbType.Int).Value = compcode;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Menu
                    {
                        MenuId = rdr["mid"].ToString(),
                        MenuName = rdr["mname"].ToString(),
                        Li_Class = rdr["li_class"].ToString(),
                        A_Class = rdr["a_class"].ToString(),
                        URL = rdr["url"].ToString(),
                        Fa_Icon = rdr["fa_icon"].ToString(),
                        Arrow = rdr["arrow"].ToString(),
                        MenuOrder = rdr["morder"].ToString(),
                        MenuPer = rdr["menuper"].ToString(),
                        IsApprove = rdr["approve"].ToString(),
                        Compcode = rdr["compcode"].ToString(),
                        submenus = SubMenu(compcode, userId, rdr["mid"].ToString())

                    });


                }
                con.Close();
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
        public List<Magzine> DisplayMagzineList()
        {
            List<Magzine> lst = new List<Magzine>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_magzine", con);
                com.CommandType = CommandType.Text;                
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Magzine
                    {
                        MagzineId = rdr["mid"].ToString(),
                        MagzineCode = rdr["mcode"].ToString(),
                        MagzineName = rdr["mname"].ToString(),
                        MagzineType = rdr["mtype"].ToString(),
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

        public List<MemberType> Display_MemberTypeList()
        {
            List<MemberType> lst = new List<MemberType>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select membertypeid,membertype from tbl_membertype", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new MemberType
                    {
                        MemberTypeId = Convert.ToInt16(rdr["membertypeid"].ToString()),
                        MemberTypeName = rdr["membertype"].ToString()
                    });
                }
                return lst;
            }
        }

        public List<Country> Display_CountryList()
        {
            List<Country> lst = new List<Country>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_country where cstatus='1'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Country
                    {
                        CountryId = Convert.ToInt16(rdr["cid"].ToString()),
                        CountryName = rdr["cname"].ToString()
                    });
                }
                return lst;
            }
        }

        public List<MagzineArticleReleased> Getmagreleadesd(string searchText)
        {
            List<MagzineArticleReleased> lst = new List<MagzineArticleReleased>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_get_relesedcode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SearchText", SqlDbType.NVarChar).Value = searchText;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MagzineArticleReleased
                    {
                        title = rdr["title"].ToString(),
                        mrid = rdr["mrid"].ToString(),
                        
                    });
                }
                con.Close();
                return lst;
            }
        }

        public string GenerateMsCode()
        {
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_generate_mscode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                string mscode = cmd.ExecuteScalar().ToString();               
                con.Close();
                return mscode;
            }
        }

        public List<Country> SearchCountry(string searchText)
        {
            List<Country> lst = new List<Country>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_country where cname like '%" + searchText + "%' and cstatus='1'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Country
                    {
                        CountryId = Convert.ToInt16(rdr["cid"].ToString()),
                        CountryName = rdr["cname"].ToString().Trim()
                    });
                }
                return lst;
            }
        }

        public List<State> SearchState(string searchText, string cid)
        {
            List<State> lst = new List<State>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_state where statename like '%" + searchText + "%' and cid=" + cid + " and sstatus='1'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new State
                    {
                        StateId = Convert.ToInt16(rdr["sid"].ToString()),
                        StateName = rdr["statename"].ToString().Trim()
                    });
                }
                return lst;
            }
        }

        public List<City> SearchCity(string searchText, string cid, string sid)
        {
            List<City> lst = new List<City>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_city where cityname like '%" + searchText + "%' and cid=" + cid + " and sid=" + sid + " and citystatus='1'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new City
                    {
                        CityId = Convert.ToInt16(rdr["cityid"].ToString()),
                        CityName = rdr["cityname"].ToString().Trim()
                    });
                }
                return lst;
            }
        }

        public List<Area> SearchArea(string searchText, string cid, string sid, string cityid)
        {
            List<Area> lst = new List<Area>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_area where areaname like '%" + searchText + "%' and cid=" + cid + " and sid=" + sid + " and cityid=" + cityid + " and areastatus='1'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Area
                    {
                        AreaId = Convert.ToInt16(rdr["areaid"].ToString()),
                        AreaName = rdr["areaname"].ToString().Trim()
                    });
                }
                return lst;
            }
        }

        public List<Subscriber> Searchparent(string searchText)
        {
            List<Subscriber> lst = new List<Subscriber>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_membership where parentid='' and name like '%" + searchText + "%'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Subscriber
                    {
                        SubscriberId = rdr["mscode"].ToString(),
                        Name = rdr["name"].ToString().Trim()
                    });
                }
                return lst;
            }
        }

        public List<State> Display_StateList(int cid)
        {
            List<State> lst = new List<State>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_state where cid=" + cid + " and sstatus='1'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new State
                    {
                        StateId = Convert.ToInt16(rdr["sid"].ToString()),
                        StateName = rdr["statename"].ToString().Trim()
                    });
                }
                return lst;
            }
        }

        public List<City> Display_CityList(int cid, int sid)
        {
            List<City> lst = new List<City>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_city where cid=" + cid + " and sid=" + sid + " and citystatus='1'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new City
                    {
                        CityId = Convert.ToInt16(rdr["cityid"].ToString()),
                        CityName = rdr["cityname"].ToString().Trim()
                    });
                }
                return lst;
            }
        }

        //----------sajid----------------//


        public List<Member> Membersearch(string searchText)
        {
            List<Member> lst = new List<Member>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_get_membercode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SearchText", SqlDbType.NVarChar).Value = searchText;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Member
                    {

                        memberid = rdr["id"].ToString().Trim(),
                        membername = rdr["mscode"].ToString() + "," + rdr["name"].ToString() + "," + rdr["mobile"].ToString(),

                    });
                }
                return lst;
            }
        }

        public List<Magzine> Magzinebind()
        {
            List<Magzine> lst = new List<Magzine>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select mid,mname,frequency from tbl_magzine", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Magzine
                    {
                        MagzineId = rdr["mid"].ToString(),
                        MagzineName = rdr["mname"].ToString().Trim(),
                        Frequency = rdr["frequency"].ToString().Trim(),
                    });
                }
                return lst;
            }
        }



        public List<Magzine> Magzinefrequency(string mzid)
        {
            List<Magzine> lst = new List<Magzine>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select mid,mname,frequency from tbl_magzine where mid=" + mzid + "", con);
                com.CommandType = CommandType.Text;

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Magzine
                    {
                        MagzineId = rdr["mid"].ToString(),
                        Frequency = rdr["frequency"].ToString().Trim(),
                    });
                }
                return lst;
            }
        }

        public List<Price> Magzinetenure(string mzid)
        {
            List<Price> lst = new List<Price>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select priceid,tenure,rate,gstper,p.mid from tbl_magzine_price_master as p left join tbl_magzine as m on (p.mid=m.mid) where p.mid='"+mzid+"' order by tenure", con);
                com.CommandType = CommandType.Text;

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Price
                    {
                       
                        PriceId = rdr["priceid"].ToString().Trim()+ "-" + rdr["gstper"].ToString(),
                        Tenure = rdr["tenure"].ToString().Trim() +"-" + rdr["rate"].ToString(),
                    });
                }
                return lst;
            }
        }

        public List<Magzine> Relmagzinebind()
        {
            List<Magzine> lst = new List<Magzine>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select mrid,dtitle  from tbl_magzine_release where ispublish=1", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Magzine
                    {
                        MagzineId = rdr["mrid"].ToString(),
                        MagzineName = rdr["dtitle"].ToString().Trim(),
                        
                    });
                }
                return lst;
            }

        }

        public List<Magzine> ReleasedMagazine(string mzid)
        {
            List<Magzine> lst = new List<Magzine>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select mrid,dtitle from tbl_magzine_release where mid=" + mzid + "", con);
                com.CommandType = CommandType.Text;

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Magzine
                    {
                        MagzineId = rdr["mrid"].ToString(),
                        MagzineName = rdr["dtitle"].ToString().Trim(),
                    });
                }
                return lst;
            }

        }

        public List<Area> Display_AreaList(int cid, int sid,int cityid)
        {
            List<Area> lst = new List<Area>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from tbl_area where cid=" + cid + " and sid=" + sid + " and cityid="+ cityid +" and areastatus='1'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Area
                    {
                        AreaId = Convert.ToInt16(rdr["areaid"].ToString()),
                        AreaName = rdr["areaname"].ToString().Trim()
                    });
                }
                return lst;
            }
        }

        public int AvailStock(int mrid)
        {
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                int inNum = 0;
                try
                {
                   

                    SqlCommand cmd = new SqlCommand("usp_get_stock_mrelease", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@mrid", SqlDbType.NVarChar).Value = mrid;
                    inNum = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    return inNum;
                }
                catch(Exception ex)
                {
                    return inNum;
                }
            }
        }

        public string Generatepartycode()
        {
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_generate_partycode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                string mscode = cmd.ExecuteScalar().ToString();
                con.Close();
                return mscode;
            }
        }



        public List<Member> bindvendor()
        {
            List<Member> lst = new List<Member>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select id,name,mscode from tbl_membership where partytype='p'", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Member
                    {
                        memberid = rdr["id"].ToString()+"-"+ rdr["mscode"].ToString(),
                        membername = rdr["name"].ToString().Trim(),
                      
                    });
                }
                return lst;
            }
        }

        public List<Price> Magazinerate(string mzid)
        {
            List<Price> lst = new List<Price>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand com = new SqlCommand("select p.priceid, rate,gstper from tbl_magzine as m left join tbl_magzine_price_master as p on (m.mid=p.mid) where m.mid=" + mzid+" and tenure=10", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Price
                    {
                        PriceId = rdr["priceid"].ToString(),
                        Rate = rdr["rate"].ToString().Trim(),
                        gst = rdr["gstper"].ToString().Trim(),

                    });
                }
                return lst;
            }
        }

        public List<Price> Magazineavlstock(string mzid, string mrid)
        {
            List<Price> lst = new List<Price>();
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_check_avlstock", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mzid", DbType.String).Value = mzid;
                cmd.Parameters.AddWithValue("@mrid", DbType.String).Value = mrid;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Price
                    {
                        stock = rdr["stock"].ToString()
                     
                    });
                }
                return lst;
            }
        }

        public string Getvno(string vtype)
        {
            var vno = "";
            using (SqlConnection con = DbConnection.getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_generate_new_id_vtype", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblname", DbType.String).Value = "tbl_master";
                cmd.Parameters.AddWithValue("@colname", DbType.String).Value = "vno";
                cmd.Parameters.AddWithValue("@vtype", DbType.String).Value = vtype;
                try
                {
                    vno = cmd.ExecuteScalar().ToString();
                    if (vno == null) { return ""; };
                }
                catch { }
                vno = cmd.ExecuteScalar().ToString();
                con.Close();
                return vno;
            }
        }
    }
}
