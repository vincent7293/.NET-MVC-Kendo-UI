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
            return RedirectToAction("Index");
        }

        [HttpGet()]
        public ActionResult DeleteOrder(int id)
        {
            Models.OrderService orderservice = new Models.OrderService();
            ViewBag.effectData = orderservice.DeleteOrder(id.ToString());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateOrder(String id)
        {
            Models.CodeService customerservice = new Models.CodeService();
            Models.OrderService orderservice = new Models.OrderService();
            Order order = new Order();
            List<SelectListItem> custData = new List<SelectListItem>();
            List<SelectListItem> empData = new List<SelectListItem>();
            List<SelectListItem> shipperData = new List<SelectListItem>();
            int defaultCustId=1, defaultEmpId=1, defaultShipperId=1;
            foreach (var oo in orderservice.GetOrderById(id))
            {
                order.OrderId = oo.OrderId;
                order.Orderdate = oo.Orderdate;
                order.RequiredDate = oo.RequiredDate;
                order.ShippedDate = oo.ShippedDate;
                order.Freight = oo.Freight;
                order.ShipName = oo.ShipName;
                order.ShipAddress = oo.ShipAddress;
                order.ShipCity = oo.ShipCity;
                order.ShipRegion = oo.ShipRegion;
                order.ShipPostalCode = oo.ShipPostalCode;
                order.ShipCountry = oo.ShipCountry;
                defaultCustId = oo.CustId;
                defaultEmpId = oo.EmpId;
                defaultShipperId = oo.ShipperId;
            }
            foreach (var aa in customerservice.GetCustomerList())
            {
                custData.Add(new SelectListItem()
                {
                    Text = aa.CustName,
                    Value = aa.CustId.ToString(),
                    Selected = (aa.CustId == defaultCustId ? true : false)
                });
            }
            foreach (var bb in customerservice.GetEmpList())
            {
                empData.Add(new SelectListItem()
                {
                    Text = bb.EmpName,
                    Value = bb.EmpId.ToString(),
                    Selected = (bb.EmpId == defaultEmpId ? true : false)
                });
            }
            foreach (var cc in customerservice.GetShipperList())
            {
                shipperData.Add(new SelectListItem()
                {
                    Text = cc.ShipperName,
                    Value = cc.ShipperId.ToString(),
                    Selected = (cc.ShipperId == defaultShipperId ? true : false)
                });
            }
            ViewBag.CustId = custData;
            ViewBag.EmpId = empData;
            ViewBag.ShipperId = shipperData;
            return View(order);
        }
        [HttpPost()]
        public ActionResult UpdateOrder(Models.Order order)
        {
            Models.OrderService orderservice = new Models.OrderService();
            orderservice.UpdateOrder(order);
            return RedirectToAction("Index");
        }
    }
}