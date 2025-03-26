using BLL_Business_Logic_Layer_;
using BOL_Business_Objects_Layer_;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace APIs_Layer.Controllers
{
    public class MagzineController : ApiController
    {
        ResponseModel objResponse = new ResponseModel();
        MessageShow msg = new MessageShow();
        MagzineBs objbll = new MagzineBs();

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Add(Magzine objuser)
        {
            try
            {
                if (objuser == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in the Request ");
                }

                objbll.Add(objuser);

                msg.Message = objbll.Message.ToString();
                msg.Result = objbll.Result.ToString();
                var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
                return Request.CreateResponse(response.StatusCode, msg);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage DisplayListPagewise(JqueryDatatableParam param)
        {
            List<Magzine> Lists = new List<Magzine>();
            Lists = objbll.AllListPagewise(param);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetMagzineDetails(string MagzineId)
        {
          
            var Lists = objbll.AllList(MagzineId);
            objResponse.Data = Lists;
            objResponse.Status = Lists != null ? true : false;
            objResponse.Message = Lists != null ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;

        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Update(Magzine obj)
        {
            try
            {
                if (obj == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in the Request ");
                }

                objbll.Update(obj);
                msg.Message = objbll.Message.ToString();
                msg.Result = objbll.Result.ToString();
                var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
                return Request.CreateResponse(response.StatusCode, msg);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Delete(int MagzineId)
        {
            objbll.Delete(MagzineId);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(response.StatusCode, msg);
        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeletePriceaster(int MagzineId, int priceid)
        {
            objbll.DeletePriceaster(MagzineId, priceid);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
            return Request.CreateResponse(response.StatusCode , msg);
        }


        //magazine release 
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage ReleaseMagzine()
        {
            //Create the Directory.
            string data = HttpContext.Current.Request["data"];
            string coverfolder = HttpContext.Current.Server.MapPath("/Upload/MagzineCover/");
            string thumbfolder = HttpContext.Current.Server.MapPath("/Upload/MagzineThumb/");
            string pdffolder = HttpContext.Current.Server.MapPath("/Upload/PDf/");
            MagzineRelease obj = JsonConvert.DeserializeObject<MagzineRelease>(data);
            if (!Directory.Exists(coverfolder))
            {
                Directory.CreateDirectory(coverfolder);
            }
            if (!Directory.Exists(thumbfolder))
            {
                Directory.CreateDirectory(thumbfolder);
            }
            if (!Directory.Exists(pdffolder))
            {
                Directory.CreateDirectory(pdffolder);
            }
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                string _imgname = string.Empty;
                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    //Fetch the File.
                    var postedFile = HttpContext.Current.Request.Files[i];

                    //Fetch the File Name.
                    //string fileName = Path.GetFileName(postedFile.FileName);
                    var fileName = Path.GetFileName(postedFile.FileName);
                    var _ext = Path.GetExtension(postedFile.FileName);
                    var path = "";
                    if (i == 0)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/MagzineCover/") + _imgname + _ext;
                        _imgname = "/Upload/MagzineCover/" + _imgname + _ext;
                        path = _comPath;
                        obj.CoverImageUrl = _imgname;
                    }
                    else if (i == 1)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/MagzineThumb/") + _imgname + _ext;
                        _imgname = "/Upload/MagzineThumb/" + _imgname + _ext;
                        path = _comPath;
                        obj.ThumbnailUrl = _imgname;
                    }
                    else
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/PDf/") + _imgname + _ext;
                        _imgname = "/Upload/PDf/" + _imgname + _ext;
                        path = _comPath;
                        obj.PdfUrl = _imgname;
                    }
                    // Saving Image in Original Mode
                    postedFile.SaveAs(path);
                }
            }
            MagzineReleaseBs objbll = new MagzineReleaseBs();
            objbll.Add(obj);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
            return Request.CreateResponse(response.StatusCode, msg);
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage DisplayReleaseListPagewise(JqueryDatatableParam param)
        {
            List<MagzineRelease> Lists = new List<MagzineRelease>();
            Lists = objbll.AllReleaseListPagewise(param);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetMagzineReleaseDetails(string MRId)
        {

            var Lists = objbll.GetMagzineReleaseDetails(MRId);
            objResponse.Data = Lists;
            objResponse.Status = Lists != null ? true : false;
            objResponse.Message = Lists != null ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage UpdateReleaseMagzine()
        {
            //Create the Directory.
            string data = HttpContext.Current.Request["data"];
            string coverfolder = HttpContext.Current.Server.MapPath("/Upload/MagzineCover/");
            string thumbfolder = HttpContext.Current.Server.MapPath("/Upload/MagzineThumb/");
            string pdffolder = HttpContext.Current.Server.MapPath("/Upload/PDf/");
            MagzineRelease obj = JsonConvert.DeserializeObject<MagzineRelease>(data);
            if (!Directory.Exists(coverfolder))
            {
                Directory.CreateDirectory(coverfolder);
            }
            if (!Directory.Exists(thumbfolder))
            {
                Directory.CreateDirectory(thumbfolder);
            }
            if (!Directory.Exists(pdffolder))
            {
                Directory.CreateDirectory(pdffolder);
            }
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                string _imgname = string.Empty;
                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    //Fetch the File.
                    var postedFile = HttpContext.Current.Request.Files[i];

                    //Fetch the File Name.
                    //string fileName = Path.GetFileName(postedFile.FileName);
                    var fileName = Path.GetFileName(postedFile.FileName);
                    var _ext = Path.GetExtension(postedFile.FileName);
                    var path = "";
                    if (i == 0)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/MagzineCover/") + _imgname + _ext;
                        _imgname = "/Upload/MagzineCover/" + _imgname + _ext;
                        path = _comPath;
                        obj.CoverImageUrl = _imgname;
                    }
                    else if (i == 1)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/MagzineThumb/") + _imgname + _ext;
                        _imgname = "/Upload/MagzineThumb/" + _imgname + _ext;
                        path = _comPath;
                        obj.ThumbnailUrl = _imgname;
                    }
                    else
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/PDf/") + _imgname + _ext;
                        _imgname = "/Upload/PDf/" + _imgname + _ext;
                        path = _comPath;
                        obj.PdfUrl = _imgname;
                    }
                    // Saving Image in Original Mode
                    postedFile.SaveAs(path);
                }
            }
            MagzineReleaseBs objbll = new MagzineReleaseBs();
            objbll.UpdateReleaseMagzine(obj);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
            return Request.CreateResponse(response.StatusCode, msg);
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage DeleteReleaseMagzine(int MRId)
        {
            objbll.DeleteReleaseMagzine(MRId);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse( HttpStatusCode.OK );
            return Request.CreateResponse(response.StatusCode, msg);
        }


        //Article
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage ArticleAdd()
        {
            //Create the Directory.
            string data = HttpContext.Current.Request["data"];
            string coverfolder = HttpContext.Current.Server.MapPath("/Upload/ArticleCover/");
            string thumbfolder = HttpContext.Current.Server.MapPath("/Upload/ArticleThumb/");

            MagzineArticle obj = JsonConvert.DeserializeObject<MagzineArticle>(data);
            if (!Directory.Exists(coverfolder))
            {
                Directory.CreateDirectory(coverfolder);
            }
            if (!Directory.Exists(thumbfolder))
            {
                Directory.CreateDirectory(thumbfolder);
            }
           
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                string _imgname = string.Empty;
                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    //Fetch the File.
                    var postedFile = HttpContext.Current.Request.Files[i];

                    //Fetch the File Name.
                    //string fileName = Path.GetFileName(postedFile.FileName);
                    var fileName = Path.GetFileName(postedFile.FileName);
                    var _ext = Path.GetExtension(postedFile.FileName);
                    var path = "";
                    if (i == 0)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/ArticleCover/") + _imgname + _ext;
                        _imgname = "/Upload/ArticleCover/" + _imgname + _ext;
                        path = _comPath;
                        obj.CoverImageUrl = _imgname;
                    }
                    else if (i == 1)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/ArticleThumb/") + _imgname + _ext;
                        _imgname = "/Upload/ArticleThumb/" + _imgname + _ext;
                        path = _comPath;
                        obj.ThumbnailUrl = _imgname;
                    }
                   
                    // Saving Image in Original Mode
                    postedFile.SaveAs(path);
                }
            }
            MagzineArticleBs objbll = new MagzineArticleBs();

            objbll.Add(obj);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
            return Request.CreateResponse(response.StatusCode, msg);
        }



        [System.Web.Http.HttpPost]
        public HttpResponseMessage DisplayArticleListPagewise(JqueryDatatableParam param, string mrid)
        {
            List<MagzineArticle> Lists = new List<MagzineArticle>();
            Lists = objbll.AllArticleListPagewise(param, mrid);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }


        [System.Web.Http.HttpGet]
        public HttpResponseMessage DeleteArticle(int arid)
        {
            objbll.DeleteArticle(arid);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(response.StatusCode, msg);
        }


        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetMagzineArticleDetails(string arid)
        {

            var Lists = objbll.GetMagzineArticleDetails(arid);
            objResponse.Data = Lists;
            objResponse.Status = Lists != null ? true : false;
            objResponse.Message = Lists != null ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage UpdateMagzineArticle()
        {
            //Create the Directory.
            string data = HttpContext.Current.Request["data"];
            string coverfolder = HttpContext.Current.Server.MapPath("/Upload/ArticleCover/");
            string thumbfolder = HttpContext.Current.Server.MapPath("/Upload/ArticleThumb/");

            MagzineArticle obj = JsonConvert.DeserializeObject<MagzineArticle>(data);
            if (!Directory.Exists(coverfolder))
            {
                Directory.CreateDirectory(coverfolder);
            }
            if (!Directory.Exists(thumbfolder))
            {
                Directory.CreateDirectory(thumbfolder);
            }

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                string _imgname = string.Empty;
                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    //Fetch the File.
                    var postedFile = HttpContext.Current.Request.Files[i];

                    //Fetch the File Name.
                    //string fileName = Path.GetFileName(postedFile.FileName);
                    var fileName = Path.GetFileName(postedFile.FileName);
                    var _ext = Path.GetExtension(postedFile.FileName);
                    var path = "";
                    if (i == 0)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/ArticleCover/") + _imgname + _ext;
                        _imgname = "/Upload/ArticleCover/" + _imgname + _ext;
                        path = _comPath;
                        obj.CoverImageUrl = _imgname;
                    }
                    else if (i == 1)
                    {
                        //Save the File.
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = HttpContext.Current.Server.MapPath("/Upload/ArticleThumb/") + _imgname + _ext;
                        _imgname = "/Upload/ArticleThumb/" + _imgname + _ext;
                        path = _comPath;
                        obj.ThumbnailUrl = _imgname;
                    }

                    // Saving Image in Original Mode
                    postedFile.SaveAs(path);
                }
            }
            MagzineArticleBs objbll = new MagzineArticleBs();

            objbll.UpdateMagzineArticle(obj);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
            return Request.CreateResponse(response.StatusCode, msg);

        }


    }
}