using BOL_Business_Objects_Layer_;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        MessageShow msg = new MessageShow();
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
        public ActionResult Index()
        {
            string url = "";
            if (Request.Cookies["inventroyV1"] != null)
            {
                string val = Request.Cookies["inventroyV1"]["userid"].ToString().Trim();
            }
            else
            {
                url = "Login";
            }
            return View(url);            
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Menu()
        {           
            List<Menu> MenuList = new List<Menu>();
            if (Request.Cookies["inventroyV1"] != null)
            {
                string val = Request.Cookies["inventroyV1"]["userid"].ToString().Trim();
                string compcode = Request.Cookies["inventroyV1"]["compcode"].ToString().Trim();
                using (HttpClient client = new HttpClient())
                {
                    string callurl = baseurl + "/api/Comman/GetMenuListByUserId?UserId=" + val+ "&Compcode="+ compcode;
                    Uri uri = new Uri(callurl);
                    Task<HttpResponseMessage> result = client.GetAsync(uri);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (result.Result.IsSuccessStatusCode)
                    {
                        Task<string> response = result.Result.Content.ReadAsStringAsync();
                        string results = response.Result;
                        var my_obj = JsonConvert.DeserializeObject<JObject>(results);
                        var data = (JArray)my_obj["Data"];
                        MenuList = JsonConvert.DeserializeObject<List<Menu>>(data.ToString());
                        ViewData["menues"] = MenuList;                       
                    }                   
                }               
            }            
            return PartialView("Menu", MenuList);
        }

        //[HttpPost]
        //public JsonResult logindetails(string username, string password)
        //{
        //    string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
        //    Login userList = new Login();
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string url = baseurl + "/api/Login/getLogin?username=" + username + "&password=" + password + "";
        //        Uri uri = new Uri(url);
        //        Task<HttpResponseMessage> result = client.GetAsync(uri);
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        //var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
        //        //var response = await client.PostAsync(url, content);
        //        if (result.Result.IsSuccessStatusCode)
        //        {
        //            Task<string> response = result.Result.Content.ReadAsStringAsync();
        //            string results = response.Result;
        //            userList = JsonConvert.DeserializeObject<Login>(results);
        //            if (userList.RES != "")
        //            {
        //                msg.Message = userList.RES;
        //                msg.Result = "False";
        //            }
        //            else
        //            {
        //                msg.Message = "Login Success ! ";
        //                msg.Result = "True";
        //                HttpCookie cookie = new HttpCookie("inventroyV1");
        //                cookie.Values.Add("userid", userList.UserId.ToString());
        //                cookie.Values.Add("compcode", userList.CompCode.ToString());
        //                cookie.Values.Add("username", userList.UserName.ToString());
        //                cookie.Values.Add("logintype", userList.UserType.ToString());
        //                //expire cookies data   
        //                cookie.Expires = DateTime.Now.AddDays(1);
        //                Response.Cookies.Add(cookie);

        //            }
        //        }
        //        else
        //        {
        //            msg.Message = result.Result.RequestMessage.ToString();
        //            msg.Result = "False";
        //        }
        //    }
        //    return Json(msg, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]

        public async Task<JsonResult> logindetails(string username, string password)
        {
            string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    // Encode the username and password in case they contain special characters
                    string encodedUsername = HttpUtility.UrlEncode(username);
                    string encodedPassword = HttpUtility.UrlEncode(password);

                    //string url = baseurl + "/api/Login/getLogin?username=" + username + "&password=" + password + "";
                    string url = baseurl + "/api/Login/getLogin?username=" + HttpUtility.UrlEncode(username) + "&password=" + HttpUtility.UrlEncode(password);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage result = await client.GetAsync(url);

                    if (result.IsSuccessStatusCode)
                    {
                        string responseString = await result.Content.ReadAsStringAsync();
                        Login userList = JsonConvert.DeserializeObject<Login>(responseString);

                        if (!string.IsNullOrEmpty(userList.RES))
                        {
                            return Json(new { Message = userList.RES, Result = "False" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Message = "Login Success!", Result = "True" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                       
                        return Json(new { Message = "Failed to connect. Status Code: " + result.StatusCode, Result = "False" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                return Json(new { Message = "HTTP Error: " + httpEx.Message, Result = "False" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = "General Error: " + ex.Message, Result = "False" }, JsonRequestBehavior.AllowGet);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]

        public JsonResult LogOut()
        {
            Session.Abandon();
            Response.Cookies.Clear();
            //Delete the authentication ticket and sign out.
            //System.Web.Security.FormsAuthentication.SignOut();
            // Clearauthentication cookie.
            //HttpCookie cookie = new HttpCookie("inventroyV1");
            //cookie.Expires = DateTime.Now.AddYears(-1);
            //HttpContext.Response.Cookies.Add(cookie);           
            if (Request.Cookies["inventroyV1"] != null)
            {
                HttpCookie myCookie = new HttpCookie("inventroyV1");              
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }            
            return Json(new { RedirectUrl = "/Login" });
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}