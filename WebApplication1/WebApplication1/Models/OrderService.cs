using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OrderService
    {
        public void InsertOrder(Models.Order order)
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
            result.CustId = id;
            result.CustName = id;
            return result;
        }
    }
}