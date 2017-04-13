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
        public int InsertOrder(Models.Order order)
        {
            string sql = @"INSERT INTO [Sales].[Orders]
           ([CustomerID],[EmployeeID],[OrderDate],[RequiredDate],[ShippedDate],[ShipperID]
           ,[Freight],[ShipName],[ShipAddress],[ShipCity],[ShipRegion],[ShipPostalCode],[ShipCountry])
     VALUES
           (
            @custid,@empid,@orderdate,@requireddate,@shippeddate,@shipperid
            @freight,@shipname,@shipaddress,@shipcity,@shipregion,@shippostalcode,@shipcountry
            )
            select scope_identity()
            ";
            int orderId=11078;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@empid", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@requireddate", order.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@shippeddate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));

                //orderId = (int)cmd.ExecuteScalar();

                conn.Close();

            }
            return orderId;
        }
        public void DeleteOrder()
        {

        }
        public void UpdateOrder()
        {

        }

        public List<Models.Order> GetOrderById(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT TOP (100) O.OrderID, O.CustomerID as CustId, C.CompanyName as CustName, E.EmployeeID as EmpId,
E.FirstName+E.LastName as EmpName, O.OrderDate, O.RequiredDate, O.ShippedDate, S.ShipperID
, S.CompanyName as ShipperName, O.Freight, O.ShipName ,O.ShipAddress, O.ShipCity
,O.ShipPostalCode , O.ShipCountry
  FROM [Sales].[Orders] as O 
  JOIN [HR].[Employees] as E ON O.EmployeeID=E.EmployeeID
  JOIN [Sales].[Customers] as C ON O.CustomerID=C.CustomerID
  JOIN [Sales].[Shippers] as S ON O.ShipperID=S.ShipperID
WHERE O.OrderID=@OrderId";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();

            }
            return this.MapOrderDataToList(dt);
        
        }
        public List<Models.Order> GetAllOrder()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT TOP (100) O.OrderID, O.CustomerID as CustId, C.CompanyName as CustName, E.EmployeeID as EmpId,
E.FirstName+E.LastName as EmpName, O.OrderDate, O.RequiredDate, O.ShippedDate, S.ShipperID
, S.CompanyName as ShipperName, O.Freight, O.ShipName ,O.ShipAddress, O.ShipCity
,O.ShipPostalCode , O.ShipCountry
  FROM [Sales].[Orders] as O 
  JOIN [HR].[Employees] as E ON O.EmployeeID=E.EmployeeID
  JOIN [Sales].[Customers] as C ON O.CustomerID=C.CustomerID
  JOIN [Sales].[Shippers] as S ON O.ShipperID=S.ShipperID";

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
                    CustId = (int)row["CustId"],
                    CustName = row["CustName"].ToString(),
                    EmpId = (int)row["EmpId"],
                    EmpName = row["EmpName"].ToString(),
                 // Freight = (double)row["Freight"],
                    Orderdate = row["Orderdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Orderdate"],
                    OrderId = (int)row["OrderId"],
                    ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"],
                    RequiredDate = row["RequiredDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequiredDate"]

                });
            }
            return result;
        }
    }
}