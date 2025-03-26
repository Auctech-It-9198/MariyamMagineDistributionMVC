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
    public class SubscriberController : Controller
    {
        // GET: Subscriber
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ADD()
        {
            return View();
        }



        public ActionResult Issue()
        {
            return View();
        }
        public ActionResult DisplayListPagewise(JqueryDatatableParam param, string cid, string sid, string cityid, string deactive, string forworder,string mtype,string gender, string ptype)
        {
            string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
            var List = new List<Subscriber>();
            var displayResult = new List<Subscriber>();
            using (var client = new HttpClient())
            {
                string callurl = baseurl + "/api/SubscriberApi/DisplayListPagewise?cid=" + cid + "&sid=" + sid + "&cityid=" + cityid + "&deactive=" + deactive + "&forworder=" + forworder + "&mtype=" + mtype + "&gender=" + gender + "&ptype=" + ptype;
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
                    List = JsonConvert.DeserializeObject<List<Subscriber>>(data.ToString());
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