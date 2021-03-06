﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        //public JsonResult Index()
        //{
        //    Models.CodeService customerservice = new Models.CodeService();
        //     String empData = JsonConvert.SerializeObject(LoadSelectListItem("Emp", 0));
        //    String shipperData = JsonConvert.SerializeObject(LoadSelectListItem("Shipper", 0));
        //    var result = new { empData = empData, shipperData = shipperData };
        //    return Json(result);
        //}
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllOrder()
        {
            Models.OrderService orderservice = new Models.OrderService();
            List<Order> order = orderservice.GetAllOrder();
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchOrder()
        {
            return View();
        }
        [HttpPost()]
        public JsonResult SearchOrder(String jsonobj)
        {
            Models.Order searchCondition = JsonConvert.DeserializeObject<Models.Order>(jsonobj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Models.OrderService orderservice = new Models.OrderService();
            List<Order> order = orderservice.GetOrderByCondition(searchCondition);
            return Json(order);
        }

        public ActionResult InsertOrder()
        {
            Models.CodeService customerservice = new Models.CodeService();
            List<SelectListItem> custData = new List<SelectListItem>();
            List<SelectListItem> empData = new List<SelectListItem>();
            List<SelectListItem> shipperData = new List<SelectListItem>();
            List<SelectListItem> productData = new List<SelectListItem>();

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
            productData.Add(new SelectListItem(){Text = "",Value = ""});

            foreach (var dd in customerservice.GetProductList())
            {
                productData.Add(new SelectListItem()
                {
                    Text = dd.ProductName,
                    Value = dd.ProductId.ToString()
                });
            }
            ViewBag.CustId = custData;
            ViewBag.EmpId = empData;
            ViewBag.ShipperId = shipperData;
            ViewBag.ProductId = productData;
            ViewBag.ProductData = customerservice.GetProductList();

            return View();
        }
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order,List<int> ProductId,List<double> UnitPrice,List<int> Qty)
        {
            Models.OrderService orderservice = new Models.OrderService();
            orderservice.InsertOrder(order, ProductId, UnitPrice, Qty);
            return RedirectToAction("Index");
        }

        [HttpPost()]
        public JsonResult DeleteOrder(String jsonobj)
        {
            Models.Order order = JsonConvert.DeserializeObject< Models.Order>(jsonobj);
            Models.OrderService orderservice = new Models.OrderService();
            int effectData = orderservice.DeleteOrder(order.OrderId.ToString());
            return Json(effectData);
        }

        [HttpPost()]
        public String DeleteOrderDetail(int OrderId , int ProductId)
        {
            Models.OrderService orderservice = new Models.OrderService();
            orderservice.DeleteOrderDetail(OrderId,  ProductId);
            return ProductId.ToString();
        }

        [HttpGet]
        public ActionResult UpdateOrder(int id)
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
            custData = JsonConvert.DeserializeObject<List<SelectListItem>>(LoadSelectListItem("Cust", defaultCustId).ToString());
            empData = JsonConvert.DeserializeObject<List<SelectListItem>>(LoadSelectListItem("Emp", defaultEmpId).ToString());
            shipperData = JsonConvert.DeserializeObject<List<SelectListItem>>(LoadSelectListItem("Shipper", defaultShipperId).ToString());
            
            ViewBag.CustId = custData;
            ViewBag.EmpId = empData;
            ViewBag.ShipperId = shipperData;
            ViewBag.OrderDetail = orderdetail;
            return View(order);
        }

        [HttpPost()]
        public JsonResult UpdateOrder(String jsonobj)
        {
            Models.Order order = JsonConvert.DeserializeObject<Models.Order>(jsonobj);
            Models.OrderService orderservice = new Models.OrderService();
            int effectData = orderservice.UpdateOrder(order);
            return Json(effectData);
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
        [HttpPost]
        public JsonResult LoadSelectListItem(String listName,int defaultId)
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
            return Json(resultList);
        }
    }
}