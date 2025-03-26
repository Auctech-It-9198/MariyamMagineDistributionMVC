using BOL_Business_Objects_Layer_;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace InventoryManagement.Controllers
{
    public class UserController : Controller
    {
        // GET: User
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
        public ActionResult ViewDetails(string ID)
        {
            
            string url = "";
            if (Request.Cookies["inventroyV1"] != null)
            {
                string val = Request.Cookies["inventroyV1"]["userid"].ToString().Trim();               
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(ID);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string UserId = new String(decoded_char);
                Users UserList = new Users();
                using (HttpClient client = new HttpClient())
                {
                    string urls = baseurl + "/api/User/GetUserDetails/?UserId=" + UserId;
                    Uri uri = new Uri(urls);
                    Task<HttpResponseMessage> result = client.GetAsync(uri);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (result.Result.IsSuccessStatusCode)
                    {
                        Task<string> response = result.Result.Content.ReadAsStringAsync();
                        string results = response.Result;
                        var my_obj = JsonConvert.DeserializeObject<JObject>(results);
                        var data = my_obj["Data"];
                        UserList = JsonConvert.DeserializeObject<Users>(data.ToString());
                        msg.Message = "Data Found ! ";
                        msg.Result = "True";
                    }
                    else
                    {
                        msg.Message = "No Records Found !";
                        msg.Result = "False";
                    }
                } 
                return View(UserList);
            }
            else
            {
                url = "Login";
            }
            return View(url);

        }
        public ActionResult Add()
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
        public ActionResult Update()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCompanyList()
        {
            List<Company> ComapnyList = new List<Company>();
            using (HttpClient client = new HttpClient())
            {
                string url = baseurl + "/api/Company/DisplayComapnyList";
                Uri uri = new Uri(url);
                Task<HttpResponseMessage> result = client.GetAsync(uri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));               
                if (result.Result.IsSuccessStatusCode)
                {
                    Task<string> response = result.Result.Content.ReadAsStringAsync();
                    string results = response.Result;
                    var my_obj = JsonConvert.DeserializeObject<JObject>(results);
                    var data = (JArray)my_obj["Data"];
                    ComapnyList = JsonConvert.DeserializeObject<List<Company>>(data.ToString());
                    msg.Message = "Data Found ! ";
                    msg.Result = "True";
                }
                else
                {
                    msg.Message = "No Records Found !";
                    msg.Result = "False";
                }
            }
            return Json(ComapnyList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserPermission()
        {
            return View();
        }
    }
}