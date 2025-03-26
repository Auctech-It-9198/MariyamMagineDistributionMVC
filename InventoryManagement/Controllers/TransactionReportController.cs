using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class TransactionReportController : Controller
    {
        // GET: TransactionReport
        public ActionResult saleregister()
        {
            return View();
        }

        public ActionResult Damage()
        {
            return View();
        }

        public ActionResult Stockinout()
        {
            return View();
        }
    }
}