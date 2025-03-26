using BOL_Business_Objects_Layer_;
using DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_
{
    public class CommanBs
    {
        private CommanDb lobj;
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
        public CommanBs()
        {
            lobj = new CommanDb();
        }

        public List<Company> Display_CompanyList()
        {
            return lobj.Display_CompanyList();
        }

        public List<Users> Display_UserList()
        {
            return lobj.Display_UserList();
        }

        public List<Menu> Display_MenuList(int UserId, int Compcode)
        {
            return lobj.Display_MenuList(UserId, Compcode);
        }

        public List<Menu> GetMenuList_for_Permission(int userId, int compcode, int menuId)
        {
            return lobj.GetMenuList_for_Permission(userId, compcode, menuId);
        }

        public List<Menu> GetMenuListByUserId(int userId, int compcode)
        {
            return lobj.GetMenuListByUserId(userId, compcode);
        }

        public List<Magzine> DisplayMagzineList()
        {
            return lobj.DisplayMagzineList();
        }

        public List<MemberType> Display_MemberTypeList()
        {
            return lobj.Display_MemberTypeList();
        }

        public List<Country> Display_CountryList()
        {
            return lobj.Display_CountryList();
        }

        public List<MagzineArticleReleased> Getmagreleadesd(string searchText)
        {
            return lobj.Getmagreleadesd(searchText);
        }

        public string GenerateMsCode()
        {
            return lobj.GenerateMsCode();
        }

        public List<Country> SearchCountry(string searchText)
        {
            return lobj.SearchCountry(searchText);
        }

        public List<State> SearchState(string searchText, string cid)
        {
            return lobj.SearchState(searchText, cid);
        }

        public List<City> SearchCity(string searchText, string cid, string sid)
        {
            return lobj.SearchCity(searchText, cid, sid);
        }

        public List<Area> SearchArea(string searchText, string cid, string sid, string cityid)
        {
            return lobj.SearchArea(searchText, cid, sid, cityid);
        }

        public List<Subscriber> Searchparent(string searchText)
        {
            return lobj.Searchparent(searchText);
        }

        public List<State> Display_StateList(int cid)
        {
            return lobj.Display_StateList(cid);
        }

        public List<City> Display_CityList(int cid, int sid)
        {
            return lobj.Display_CityList(cid,sid);
        }

        //----------sajid-----------//
        public List<Member> Membersearch(string searchText)
        {
            return lobj.Membersearch(searchText);
        }

        public List<Magzine> Magzinebind()
        {
            return lobj.Magzinebind();
        }

        public List<Magzine> Magzinefrequency(string mzid)
        {
            return lobj.Magzinefrequency(mzid);
        }

        public List<Price> Magzinetenure(string mzid)
        {
            return lobj.Magzinetenure(mzid);
        }

        public List<Magzine> Relmagzinebind()
        {
            return lobj.Relmagzinebind();
        }

        public List<Magzine> ReleasedMagazine(string mzid)
        {
            return lobj.ReleasedMagazine(mzid);
        }

        public List<Area> Display_AreaList(int cid, int sid, int cityid)
        {
            return lobj.Display_AreaList(cid, sid, cityid);
        }

        public int AvailStock(int mrid)
        {
            return lobj.AvailStock(mrid);
        }

        public string Generatepartycode()
        {
            return lobj.Generatepartycode();
        }

        public List<Member> bindvendor()
        {
            return lobj.bindvendor();
        }

        public List<Price> Magazinerate(string mzid)
        {
            return lobj.Magazinerate(mzid);
        }

        public List<Price> Magazineavlstock(string mzid, string mrid)
        {
            return lobj.Magazineavlstock(mzid,mrid);
        }

        public string Getvno(string vtype)
        {
            return lobj.Getvno(vtype);
        }
    }
}
