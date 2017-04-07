using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OrderService
    {
        private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        public void InsertOrder(Models.Order order)
        {

        }
        public void DeleteOrder()
        {

        }
        public void UpdateOrder()
        {

        }
        public Models.Order GetOrderById(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.Orders";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();

            }
            return this.MapOrderDataToList(dt).FirstOrDefault();
        }
        public List<Models.Order> GetOrder()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.Orders";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();

            }
            return this.MapOrderDataToList(dt);//.FirstOrDefault();
        }

        private List<Models.Order> MapOrderDataToList(DataTable orderData)
        {
            List<Models.Order> result = new List<Order>();
            
            foreach(DataRow row in orderData.Rows)
            {
                result.Add(new Order()
                {
                    OrderId = (int)row["OrderId"]
                });
            }
            return result;
        }
    }
}