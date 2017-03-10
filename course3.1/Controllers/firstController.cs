using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace course3.Controllers
{
    public class firstController : Controller
    {
        public ActionResult Index()
        {
			ViewBag.Label = "gg";
            return View ();
        }
    }
}
