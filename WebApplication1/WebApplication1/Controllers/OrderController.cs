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
            List<Order> order= orderservice.GetAllOrder();

            return View(order);
        }

        public ActionResult SearchOrder()
        {
            return View();
        }
        [HttpPost()]
        public ActionResult SearchOrder(String OrderId)
        {
            Models.OrderService orderservice = new Models.OrderService();
            List<Order> order = orderservice.GetOrderById(OrderId);

            return View(order);
        }


        public ActionResult InsertOrder()
        {
            Models.CodeService customerservice = new Models.CodeService();
            List<SelectListItem> custData = new List<SelectListItem>();
            List<SelectListItem> empData = new List<SelectListItem>();
            List<SelectListItem> shipperData = new List<SelectListItem>();

            foreach (var aa in customerservice.GetCustomerList())
            {
                custData.Add(new SelectListItem()
                {
                    Text = aa.CustName,
                    Value = aa.CustId.ToString()
                });
            }
            foreach (var bb in customerservice.GetEmpList())
            {
                empData.Add(new SelectListItem()
                {
                    Text = bb.EmpName,
                    Value = bb.EmpId.ToString()
                });
            }
            foreach (var cc in customerservice.GetShipperList())
            {
                shipperData.Add(new SelectListItem()
                {
                    Text = cc.ShipperName,
                    Value = cc.ShipperId.ToString()
                });
            }
            ViewBag.CustId = custData;
            ViewBag.EmpId = empData;
            ViewBag.ShipperId = shipperData;
            return View();
        }
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order)
        {
            Models.OrderService orderservice = new Models.OrderService();
            orderservice.InsertOrder(order);
            return View("InsertOrder");
        }
    }
}