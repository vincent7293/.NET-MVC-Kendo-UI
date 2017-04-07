using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            Models.OrderService orderservice = new Models.OrderService();
            List<Order> order= orderservice.GetOrder();

            return View(order);
        }
        public ActionResult InsertOrder()
        {
            return View();
        }
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order)
        {
            Models.OrderService orderservice = new Models.OrderService();
            orderservice.InsertOrder(order);
            return View("Index");
        }
        
    }
}