using BOL_Business_Objects_Layer_;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult DisplayArticleListPagewise(JqueryDatatableParam param,string mrid)
        {
            string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
            var List = new List<MagzineArticle>();
            var displayResult = new List<MagzineArticle>();
            using (var client = new HttpClient())
            {
                string callurl = baseurl + "/api/Magzine/DisplayArticleListPagewise?mrid=" + mrid;
                var stringContent = new StringContent(JsonConvert.SerializeObject(param), UnicodeEncoding.UTF8, "application/json");
                var check = stringContent.ReadAsStringAsync().GetAwaiter().GetResult();
                var responseTask = client.PostAsync(callurl, stringContent);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    //Deserialize object
                    var my_obj = JsonConvert.DeserializeObject<JObject>(readTask);
                    var data = (JArray)my_obj["Data"];
                    List = JsonConvert.DeserializeObject<List<MagzineArticle>>(data.ToString());
                }
            }
            if (param.iDisplayLength > 0)
            {
                displayResult = List.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            }
            else
            {
                displayResult = List.ToList();
            }
            var totalRecords = List.Count();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }



    }
}