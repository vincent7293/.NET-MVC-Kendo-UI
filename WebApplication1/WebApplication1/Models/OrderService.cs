using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OrderService
    {
        public void InsertOrder()
        {

        }
        public void DeleteOrder()
        {

        }
        public void UpdateOrder()
        {

        }
        public Models.Order GetOrderById(string id)
        {
            Models.Order result = new Order();
            result.CustId = "111";
            result.CustName = "Vincent";
            return result;
        }
    }
}