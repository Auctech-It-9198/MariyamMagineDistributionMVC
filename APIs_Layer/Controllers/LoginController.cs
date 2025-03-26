using BLL_Business_Logic_Layer_;
using BOL_Business_Objects_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIs_Layer.Controllers
{
    public class LoginController : ApiController
    {
        UsersBs objbll = new UsersBs();
        //public Login getLogin(string username, string password)
        //{
        //    List<Login> userList = new List<Login>();
        //    userList = objbll.UserLogin(username, password);
        //    return userList.FirstOrDefault();
        //}

        public Login getLogin(string username, string password)
        {
            // Call the UserLogin method to get a list of matching users
            List<Login> userList = objbll.UserLogin(username, password);

            // Return the first user, or null if no users are found
            return userList?.FirstOrDefault();
        }

       
    }
}