using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class StockReportController : Controller
    {
        // GET: StockReport
        public ActionResult stockledger()
        {
            return View();
        }

        public ActionResult stockstatement()
        {
            return View();
        }
    }
}