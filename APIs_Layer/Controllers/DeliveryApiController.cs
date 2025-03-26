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
    public class DeliveryApiController : ApiController
    {
        ResponseModel objResponse = new ResponseModel();
        MessageShow msg = new MessageShow();
        DeliveryBs objbll = new DeliveryBs();



        [HttpGet]
        public HttpResponseMessage DisplayListPagewise(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            List<Delivery> Lists = new List<Delivery>();
            Lists = objbll.AllListPagewise( search,cid, sid,cityid, areaid, deactive, mtype, gender, mzid,mrid,gperiods,cstatus);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }


        [HttpGet]
        public HttpResponseMessage DisplayListPagewisemasterlist(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            List<Delivery> Lists = new List<Delivery>();
            Lists = objbll.AllListPagewiselist(search, cid, sid, cityid, areaid, deactive, mtype, gender, mzid, mrid, gperiods, cstatus);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }




        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Add(List<Delivery> Delivery)
        {
            try
            {
                if (Delivery == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in User Request ");
                }
                objbll.Add(Delivery);
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
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Update(List<Delivery> Delivery)
        {
            try
            {
                if (Delivery == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in User Request ");
                }
                objbll.Update(Delivery);
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
        public HttpResponseMessage Delete(int vno, string vtype)
        {
            objbll.Delete(vno, vtype);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
            return Request.CreateResponse(response.StatusCode, msg);
        }



        [HttpGet]
        public HttpResponseMessage DisplayListforprint(string vno, string vtype, string isfarwarder)
        {
            List<Delivery> Lists = new List<Delivery>();
            Lists = objbll.DisplayListforprint(vno, vtype,isfarwarder);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        [HttpGet]
        public HttpResponseMessage DisplayListPagewiseforprint(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            List<Delivery> Lists = new List<Delivery>();
            Lists = objbll.AllListPagewiseforprintlables(search, cid, sid, cityid, areaid, deactive, mtype, gender, mzid, mrid, gperiods, cstatus);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }



        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage AddPrint(List<Delivery> Delivery)
        {
            try
            {
                {
                    if (Delivery == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in User Request ");
                    }
                    objbll.AddPrint(Delivery);
                    msg.Message = objbll.Message.ToString();
                    msg.Result = objbll.Result.ToString();
                    var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
                    return Request.CreateResponse(response.StatusCode, msg);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }



        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage DispatchAdd(List<Delivery> Delivery)
        {
            try
            {
                if (Delivery == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in User Request ");
                }
                objbll.DispatchAdd(Delivery);
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
        //----
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DisplayPrinlist(JqueryDatatableParam param, string mzid,string mrid)
        {
            List<Delivery> Lists = new List<Delivery>();
            Lists = objbll.DisplayPrinlist(param, mzid, mrid);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }
        //

        [System.Web.Http.HttpGet]
        public HttpResponseMessage DeletePrintRecord(int vno, int pvno)
        {
            objbll.DeletePrintRecord(vno,pvno);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
            return Request.CreateResponse(response.StatusCode, msg);
        }
        //---------------


        [HttpGet]
        public HttpResponseMessage DisplayListforDispatch(string vno)
        {
            List<Delivery> Lists = new List<Delivery>();
            Lists = objbll.DisplayListforDispatch(vno);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Dispatchpostadd(List<Delivery> Delivery)
        {
            try
            {
                {
                    if (Delivery == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in User Request ");
                    }
                    objbll.Dispatchpostadd(Delivery);
                    msg.Message = objbll.Message.ToString();
                    msg.Result = objbll.Result.ToString();
                    var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
                    return Request.CreateResponse(response.StatusCode, msg);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }



        [System.Web.Http.HttpPost]
        public HttpResponseMessage DisplayListPagewiseforsale(JqueryDatatableParam param,  string vtype)
        {
            List<Delivery> Lists = new List<Delivery>();
            Lists = objbll.DisplayListPagewiseforsale(param, vtype);
            objResponse.Data = Lists;
            objResponse.Status = Lists.Count > 0 ? true : false;
            objResponse.Message = Lists.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage DeleteSale(int vno, string vtype)
        {
            objbll.DeleteSale(vno, vtype);
            msg.Message = objbll.Message.ToString();
            msg.Result = objbll.Result.ToString();
            var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
            return Request.CreateResponse(response.StatusCode, msg);
        }



        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetSaleEntryRecords(string vno, string vtype)
        {
            List<Delivery> Lists = new List<Delivery>();
            Lists = objbll.GetSaleEntryRecordss(vno,vtype);
            objResponse.Data = Lists;
            objResponse.Status = Lists != null ? true : false;
            objResponse.Message = Lists != null ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(Lists != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        //------------------------

    }
}