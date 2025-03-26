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
    public class CompanyController : ApiController
    {
        ResponseModel objResponse = new ResponseModel();
        
        CompanyBs objbll = new CompanyBs();        
        [HttpGet] // GET api/cardetails
        public HttpResponseMessage DisplayComapnyList()
        {

            List<Company> userList = new List<Company>();
            userList = objbll.AllList();
            objResponse.Data = userList;
            objResponse.Status = userList.Count > 0 ? true : false;
            objResponse.Message = userList.Count > 0 ? "Data Found !" : "Data Not Found !";
            string Json = JsonConvert.SerializeObject(objResponse);           
            var response = Request.CreateResponse(userList.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;
        }
    }
}