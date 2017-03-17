using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.Employee.Controllers
{
    public class Default1Controller : Controller
    {
        // GET: Employee/Default1
        public ActionResult Index()
        {
            ViewBag.ooo = "ddd";
            return View();
        }
    }
}