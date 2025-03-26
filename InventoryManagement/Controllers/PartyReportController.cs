using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class PartyReportController : Controller
    {
        // GET: PartyReport
        public ActionResult partyledger()
        {
            return View();
        }

        public ActionResult partylist()
        {
            return View();
        }

        public ActionResult memberledger()
        {
            return View();
        }

        public ActionResult memberlist()
        {
            return View();
        }
    }
}