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
    public class SubscriptionsApiController : ApiController
    {
        ResponseModel objResponse = new ResponseModel();
        MessageShow msg = new MessageShow();
        SubscriptionsBs objbll = new SubscriptionsBs();

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Add(Subscriptions objuser)
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
        public HttpResponseMessage DisplayListPagewise(JqueryDatatableParam param, int cid,int sid,int cityid, string deactive, string forworder, string mtype, string gender, string mzid, string tenure,string fromdate, string todate, string expmonth, string expyear)
        {
            List<Subscriptions> Lists = new List<Subscriptions>();
            Lists = objbll.AllListPagewise(param, cid, sid, cityid, deactive, forworder, mtype, gender,mzid,tenure,fromdate,todate,expmonth,expyear);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }


        [System.Web.Http.HttpGet]
        public HttpResponseMessage getRecordsbysbid(string sbid, string mzid, string msid)
        {
            var Lists = objbll.getRecordsbysbid(sbid,mzid,msid);
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
        public HttpResponseMessage Renew(Subscriptions objuser)
        {
            try
            {
                if (objuser == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in the Request ");
                }

                objbll.Renew(objuser);

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


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Grace(Subscriptions objuser)
        {
            try
            {
                if (objuser == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in the Request ");
                }

                objbll.Grace(objuser);

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


        //-------------
    }
}