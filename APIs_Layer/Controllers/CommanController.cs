using BLL_Business_Logic_Layer_;
using BOL_Business_Objects_Layer_;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace APIs_Layer.Controllers
{
    public class CommanController : ApiController
    {
        CommanBs objbll = new CommanBs();
        ResponseModel objResponse = new ResponseModel();
        MessageShow msg = new MessageShow();

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayComapnyList()
        {

            List<Company> ComapnyList = new List<Company>();
            ComapnyList = objbll.Display_CompanyList();
            objResponse.Data = ComapnyList;
            objResponse.Status = ComapnyList.Count > 0 ? true : false;
            objResponse.Message = ComapnyList.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(ComapnyList.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage GetComapnyList(int UserId)
        {

            List<Company> ComapnyList = new List<Company>();
            ComapnyList = objbll.Display_CompanyList().FindAll(x => x.UserId.Equals(UserId)).ToList();
            objResponse.Data = ComapnyList;
            objResponse.Status = ComapnyList.Count > 0 ? true : false;
            objResponse.Message = ComapnyList.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(ComapnyList.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayUserList()
        {

            List<Users> userList = new List<Users>();
            userList = objbll.Display_UserList();
            objResponse.Data = userList;
            objResponse.Status = userList.Count > 0 ? true : false;
            objResponse.Message = userList.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(userList.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }


        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayMenuList(int UserId,int Compcode)
        {
            List<Menu> menuList = new List<Menu>();
            menuList = objbll.Display_MenuList(UserId, Compcode);
            objResponse.Data = menuList;
            objResponse.Status = menuList.Count > 0 ? true : false;
            objResponse.Message = menuList.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(menuList.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage GetMenuListByUserId(int UserId, int Compcode)
        {
            List<Menu> menuList = new List<Menu>();
            menuList = objbll.GetMenuListByUserId(UserId, Compcode);
            objResponse.Data = menuList;
            objResponse.Status = menuList.Count > 0 ? true : false;
            objResponse.Message = menuList.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(menuList.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        [HttpGet] // GET api/cardetails
        public HttpResponseMessage GetMenuList_for_Permission(int UserId, int Compcode, int MenuId)
        {
            List<Menu> menuList = new List<Menu>();
            menuList = objbll.GetMenuList_for_Permission(UserId, Compcode, MenuId);
            objResponse.Data = menuList;
            objResponse.Status = menuList.Count > 0 ? true : false;
            objResponse.Message = menuList.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(menuList.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayMagzineList()
        {

            List<Magzine> List = new List<Magzine>();
            List = objbll.DisplayMagzineList().ToList();
            objResponse.Data = List;
            objResponse.Status = List.Count > 0 ? true : false;
            objResponse.Message = List.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(List.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        [HttpGet] // GET api/cardetails
        public HttpResponseMessage getreleasedcode(string SearchText)
        {

                List<MagzineArticleReleased> Getmagreleadesd = new List<MagzineArticleReleased>();
                Getmagreleadesd = objbll.Getmagreleadesd( SearchText);
                objResponse.Data = Getmagreleadesd;
                objResponse.Status = Getmagreleadesd.Count > 0 ? true : false;
                objResponse.Message = Getmagreleadesd.Count > 0 ? "Data Found !" : "Data Not Found !";
                string Json = JsonConvert.SerializeObject(objResponse);
                var response = Request.CreateResponse(Getmagreleadesd.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
                response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
                return response;
            
        }
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayMemberTypeList()
        {

            List<MemberType> List = new List<MemberType>();
            List = objbll.Display_MemberTypeList();
            objResponse.Data = List;
            objResponse.Status = List.Count > 0 ? true : false;
            objResponse.Message = List.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(List.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayCountryList()
        {

            List<Country> List = new List<Country>();
            List = objbll.Display_CountryList();
            objResponse.Data = List;
            objResponse.Status = List.Count > 0 ? true : false;
            objResponse.Message = List.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(List.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

       


        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayStateList(int cid)
        {

            List<State> List = new List<State>();
            List = objbll.Display_StateList(cid);
            objResponse.Data = List;
            objResponse.Status = List.Count > 0 ? true : false;
            objResponse.Message = List.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(List.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayCityList(int cid,int sid)
        {

            List<City> List = new List<City>();
            List = objbll.Display_CityList(cid, sid);
            objResponse.Data = List;
            objResponse.Status = List.Count > 0 ? true : false;
            objResponse.Message = List.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(List.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayAreaList(int cid, int sid,int cityid)
        {

            List<Area> List = new List<Area>();
            List = objbll.Display_AreaList(cid, sid,cityid);
            objResponse.Data = List;
            objResponse.Status = List.Count > 0 ? true : false;
            objResponse.Message = List.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(List.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GenerateMsCode()
        {
            string Mscode = objbll.GenerateMsCode();
            objResponse.Data = Mscode;
            objResponse.Status = Mscode != "" ? true : false;
            objResponse.Message = Mscode != "" ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Mscode != "" ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage Generatepartycode()
        {
            string Mscode = objbll.Generatepartycode();
            objResponse.Data = Mscode;
            objResponse.Status = Mscode != "" ? true : false;
            objResponse.Message = Mscode != "" ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Mscode != "" ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage SearchCountry(string SearchText)
        {

            List<Country> list = new List<Country>();
            list = objbll.SearchCountry(SearchText);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        [HttpGet] // GET api/cardetails
        public HttpResponseMessage SearchState(string SearchText, string cid)
        {

            List<State> list = new List<State>();
            list = objbll.SearchState(SearchText, cid);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage SearchCity(string SearchText, string cid, string sid)
        {

            List<City> list = new List<City>();
            list = objbll.SearchCity(SearchText, cid, sid);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage SearchArea(string SearchText, string cid, string sid,string cityid)
        {

            List<Area> list = new List<Area>();
            list = objbll.SearchArea(SearchText, cid, sid, cityid);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Searchparent(string SearchText)
        {

            List<Subscriber> list = new List<Subscriber>();
            //list = objbll.Searchparent(SearchText).Where(x => x.ParentId.Equals(String.Empty)).ToList();
            list = objbll.Searchparent(SearchText);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        // Member Autocompletbox
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Membersearch(string SearchText)
        {

            List<Member> list = new List<Member>();
            list = objbll.Membersearch(SearchText);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        // Member Autocompletbox
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Magzinebind()
        {

            List<Magzine> list = new List<Magzine>();
            list = objbll.Magzinebind();
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Magzinefrequency(string mzid)
        {

            List<Magzine> list = new List<Magzine>();
            list = objbll.Magzinefrequency(mzid);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Magzinetenure(string mzid)
        {

            List<Price> list = new List<Price>();
            list = objbll.Magzinetenure(mzid);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }




        // Member Autocompletbox
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Relmagzinebind()
        {

            List<Magzine> list = new List<Magzine>();
            list = objbll.Relmagzinebind();
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }


        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Releasedmagazine(string mzid)
        {

            List<Magzine> list = new List<Magzine>();
            list = objbll.ReleasedMagazine(mzid);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Availstock(string mrid)
        {
            int Stocks = objbll.AvailStock(Convert.ToInt32(mrid));
            objResponse.Data = Stocks;
            objResponse.Status = Stocks.ToString() != "" ? true : false;
            objResponse.Message = Stocks.ToString() != "" ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Stocks.ToString() != "" ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        // Member Autocompletbox
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage bindvendor()
        {

            List<Member> list = new List<Member>();
            list = objbll.bindvendor();
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(list.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }


        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Magazinerate(string mzid)
        {

            List<Price> list = new List<Price>();
            list = objbll.Magazinerate(mzid);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet] // GET api/cardetails
        public HttpResponseMessage Magazineavlstock(string mzid, string mrid)
        {

            List<Price> list = new List<Price>();
            list = objbll.Magazineavlstock(mzid,mrid);
            objResponse.Data = list;
            objResponse.Status = list.Count > 0 ? true : false;
            objResponse.Message = list.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

         [System.Web.Http.HttpPost]
        public HttpResponseMessage getvno(string vtype)
        {
            string vno = objbll.Getvno(vtype);
            objResponse.Data = vno;
            objResponse.Status = vno != "" ? true : false;
            objResponse.Message = vno != "" ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(vno != "" ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

    }
}