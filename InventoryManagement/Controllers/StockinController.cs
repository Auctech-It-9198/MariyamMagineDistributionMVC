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
    public class StockinController : Controller
    {
        // GET: Sale
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }


        public ActionResult DisplayListPagewise(JqueryDatatableParam param, string vtype)
        {
            string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
            var jobworkerList = new List<Delivery>();
            var displayResult = new List<Delivery>();
            using (var client = new HttpClient())
            {
                string callurl = baseurl + "/api/DeliveryApi/DisplayListPagewiseforsale?vtype=" + vtype;
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
                    jobworkerList = JsonConvert.DeserializeObject<List<Delivery>>(data.ToString());
                }
            }
            if (param.iDisplayLength > 0)
            {
                displayResult = jobworkerList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            }
            else
            {
                displayResult = jobworkerList.ToList();
            }
            var totalRecords = jobworkerList.Count();
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