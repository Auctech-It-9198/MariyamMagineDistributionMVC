using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace APIs_Layer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }       
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Note---- Both method are work
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            if (Request.Headers.AllKeys.Contains("Origin", StringComparer.OrdinalIgnoreCase) && Request.HttpMethod == "OPTIONS")
            {
                Response.Flush();
            }
            //if ((Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS"))
            //{
            //    Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //    Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With, Session");
            //    Response.Flush();
            //}
        }
    }
}
