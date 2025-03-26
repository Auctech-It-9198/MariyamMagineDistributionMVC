using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BOL_Business_Objects_Layer_;
using System.IO;
using System.Net;
using BLL_Business_Logic_Layer_;

namespace InventoryManagement.Controllers
{
    public class MagzineController : Controller
    {
        MessageShow msg = new MessageShow();
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
        // GET: Magzine
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Issue()
        {
            return View();
        }
        public ActionResult ReleaseList()
        {
            return View();
        }
        public ActionResult DisplayListPagewise(JqueryDatatableParam param)
        {
            string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
            var jobworkerList = new List<Magzine>();
            var displayResult = new List<Magzine>();
            using (var client = new HttpClient())
            {
                string callurl = baseurl + "/api/Magzine/DisplayListPagewise";
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
                    jobworkerList = JsonConvert.DeserializeObject<List<Magzine>>(data.ToString());
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

        public ActionResult ReleaseMagzine(HttpPostedFileBase[] Coverfile, HttpPostedFileBase[] thumbfile, string data)
        {
            //Create the Directory.
            string coverfolder = Server.MapPath("/Upload/MagzineCover/");
            string thumbfolder = Server.MapPath("/Upload/MagzineThumb/");
            MagzineRelease obj = JsonConvert.DeserializeObject<MagzineRelease>(data);
            if (!Directory.Exists(coverfolder))
            {
                Directory.CreateDirectory(coverfolder);
            }
            if (!Directory.Exists(thumbfolder))
            {
                Directory.CreateDirectory(thumbfolder);
            }
            if (Request.Files.Count > 0)
            {
                string _imgname = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    //Fetch the File.
                    var postedFile = Request.Files[i];
                    
                    //Fetch the File Name.
                    //string fileName = Path.GetFileName(postedFile.FileName);
                    var fileName = Path.GetFileName(postedFile.FileName);
                    var _ext = Path.GetExtension(postedFile.FileName);
                    var path = "";
                    if (i == 0)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = Server.MapPath("/Upload/MagzineCover/") + _imgname + _ext;
                        _imgname = "/Upload/MagzineCover/" + _imgname + _ext;
                        path = _comPath;
                        obj.CoverImageUrl = _imgname;
                    }
                    else
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = Server.MapPath("/Upload/MagzineThumb/") + _imgname + _ext;
                        _imgname = "/Upload/MagzineThumb/" + _imgname + _ext;
                        path = _comPath;
                        obj.ThumbnailUrl = _imgname;
                    }
                    // Saving Image in Original Mode
                    postedFile.SaveAs(path);
                }
            }
            MagzineReleaseBs objbll = new MagzineReleaseBs();
            objbll.Add(obj);
            msg.Result = objbll.Result.ToString();
            msg.Message = objbll.Message.ToString();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayReleaseListPagewise(JqueryDatatableParam param)
        {
            string baseurl = System.Configuration.ConfigurationManager.AppSettings["Url"];
            var List = new List<MagzineRelease>();
            var displayResult = new List<MagzineRelease>();
            using (var client = new HttpClient())
            {
                string callurl = baseurl + "/api/Magzine/DisplayReleaseListPagewise";
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
                    List = JsonConvert.DeserializeObject<List<MagzineRelease>>(data.ToString());
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