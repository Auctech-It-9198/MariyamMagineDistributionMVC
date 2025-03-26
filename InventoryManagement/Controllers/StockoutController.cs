using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class StockoutController : Controller
    {
        // GET: Stockout
        public ActionResult Index()
        {
            return View();
        }
    }
}