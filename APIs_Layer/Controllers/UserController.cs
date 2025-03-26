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
    public class UserController : ApiController
    {
        ResponseModel objResponse = new ResponseModel();
        MessageShow msg = new MessageShow();
        UsersBs objbll = new UsersBs();

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Add(Users objuser)
        {
            try
            {                
                if (objuser == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in User Request ");
                }
                objuser.Registered = System.DateTime.Now.ToString();
                objuser.LastActivity = System.DateTime.Now.ToString();
                objbll.Add(objuser);
                msg.Message = objbll.Message.ToString();
                msg.Result = objbll.Result.ToString();
                //string Json = JsonConvert.SerializeObject(msg);
                var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
                //response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
                //var res = response.StatusCode;
                ////return response;
                return Request.CreateResponse(response.StatusCode, msg);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Display_UserList()
        {

            List<Users> userList = new List<Users>();
            userList = objbll.AllList();
            objResponse.Data = userList;
            objResponse.Status = userList.Count > 0 ? true : false;
            objResponse.Message = userList.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(userList.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        
        public HttpResponseMessage GetUserDetails(int UserId)
        {
            //Users userList = new Users();
            var userList = objbll.AllList().Find(x => x.UserId.Equals(UserId.ToString()));
            objResponse.Data = userList;
            objResponse.Status = userList != null ? true : false;
            objResponse.Message = userList != null ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);
            var response = Request.CreateResponse(userList != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Update(Users objuser)
        {
            try
            {
                if (objuser == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in User Request ");
                }
                objuser.Registered = System.DateTime.Now.ToString();
                objuser.LastActivity = System.DateTime.Now.ToString();
                objbll.Update(objuser);
                msg.Message = objbll.Message.ToString();
                msg.Result = objbll.Result.ToString();
                //string Json = JsonConvert.SerializeObject(msg);
                var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
                //response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
                //var res = response.StatusCode;
                ////return response;
                return Request.CreateResponse(response.StatusCode, msg);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage AddPermission(Menu objmenu)
        {
            try
            {
                if (objmenu == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not Present in User Request ");
                }                
                objbll.AddPermission(objmenu);
                msg.Message = objbll.Message.ToString();
                msg.Result = objbll.Result.ToString();
                //string Json = JsonConvert.SerializeObject(msg);
                var response = Request.CreateResponse(objbll.Result == true ? HttpStatusCode.OK : HttpStatusCode.Ambiguous);
                //response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
                //var res = response.StatusCode;
                ////return response;
                return Request.CreateResponse(response.StatusCode, msg);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}