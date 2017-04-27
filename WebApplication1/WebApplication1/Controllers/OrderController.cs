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
            Models.CodeService customerservice = new Models.CodeService();
            List<SelectListItem> empData = new List<SelectListItem>();
            List<SelectListItem> shipperData = new List<SelectListItem>();
            empData = LoadSelectListItem("Emp", 0);
            shipperData = LoadSelectListItem("Shipper", 0);
            ViewBag.EmpId = empData;
            ViewBag.ShipperId = shipperData;
            return View(order);
        }

        public ActionResult SearchOrder()
        {
            return View();
        }
        [HttpPost()]
        public ActionResult SearchOrder(Models.Order searchCondition)
        {
            Models.OrderService orderservice = new Models.OrderService();
            List<Order> order = orderservice.GetOrderByCondition(searchCondition);
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
            List<OrderDetail> orderdetail = new List<OrderDetail>();
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
            foreach(var od in orderservice.GetOrderDetailById(id))
            {
                orderdetail.Add(new OrderDetail()
                {
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    ProductName = od.ProductName,
                    UnitPrice = od.UnitPrice,
                    Qty = od.Qty
                });
                
            }
            custData = LoadSelectListItem("Cust", defaultCustId);
            empData = LoadSelectListItem("Emp", defaultEmpId);
            shipperData = LoadSelectListItem("Shipper", defaultShipperId);
            
            ViewBag.CustId = custData;
            ViewBag.EmpId = empData;
            ViewBag.ShipperId = shipperData;
            ViewBag.OrderDetail = orderdetail;
            return View(order);
        }
        [HttpPost()]
        public ActionResult UpdateOrder(Models.Order order)
        {
            Models.OrderService orderservice = new Models.OrderService();
            orderservice.UpdateOrder(order);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public String UpdateOrderDetail(List<OrderDetail> odList)
        {
            Models.OrderService orderservice = new Models.OrderService();
            foreach(OrderDetail od in odList)
            {
                orderservice.UpdateOrderDetail(od);
            }
            return "已更新";
        }


        /// <summary>
        /// 取得下拉式選單內容
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="defaultId"></param>
        /// <returns></returns>
        public List<SelectListItem> LoadSelectListItem(String listName,int defaultId)
        {
            Models.CodeService customerservice = new Models.CodeService();
            List<SelectListItem> resultList = new List<SelectListItem>();
            resultList.Add(new SelectListItem()
            {
                Text = "",
                Value = "",
                Selected = ( 0 == defaultId ? true : false)
            });
            switch (listName)
            {
                case "Cust":
                    foreach (var aa in customerservice.GetCustomerList())
                    {
                        resultList.Add(new SelectListItem()
                        {
                            Text = aa.CustName,
                            Value = aa.CustId.ToString(),
                            Selected = (aa.CustId == defaultId ? true : false)
                        });
                    }
                    break;
                case "Emp":
                    foreach (var bb in customerservice.GetEmpList())
                    {
                        resultList.Add(new SelectListItem()
                        {
                            Text = bb.EmpName,
                            Value = bb.EmpId.ToString(),
                            Selected = (bb.EmpId == defaultId ? true : false)
                        });
                    }
                    break;
                case "Shipper":
                    foreach (var cc in customerservice.GetShipperList())
                    {
                        resultList.Add(new SelectListItem()
                        {
                            Text = cc.ShipperName,
                            Value = cc.ShipperId.ToString(),
                            Selected = (cc.ShipperId == defaultId ? true : false)
                        });
                    }
                    break;
            }
            return resultList;
        }
    }
}