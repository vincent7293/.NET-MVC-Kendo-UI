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
            string sql = @"INSERT INTO [Sales].[Orders]
           ([CustomerID],[EmployeeID],[OrderDate],[RequiredDate],[ShippedDate],[ShipperID]
           ,[Freight],[ShipName],[ShipAddress],[ShipCity],[ShipRegion],[ShipPostalCode],[ShipCountry])
     VALUES
           (
            @custid,@empid,@orderdate,@requireddate,@shippeddate,@shipperid,
            @freight,@shipname,@shipaddress,@shipcity,@shipregion,@shippostalcode,@shipcountry
            )
            select scope_identity()
            ";
            string format1 = order.Orderdate.ToString("yyyy-MM-dd");
            string format2 = order.RequiredDate.ToString("yyyy-MM-dd");
            string format3 = order.ShippedDate.ToString("yyyy-MM-dd");

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@empid", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@orderdate", format1));
                cmd.Parameters.Add(new SqlParameter("@requireddate", format2));
                cmd.Parameters.Add(new SqlParameter("@shippeddate", format3));
                cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));

                //orderId = (int)cmd.ExecuteScalar();
                cmd.ExecuteNonQuery();
                conn.Close();

            }
        }

        public int DeleteOrder(string orderId)
        {
            int effectData = 0;
            string sql = @"DELETE FROM [Sales].[OrderDetails]
      WHERE [OrderID]=@OrderId

DELETE FROM [Sales].[Orders]
      WHERE [OrderID]=@OrderId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                effectData = cmd.ExecuteNonQuery();
         
                conn.Close();
            }
            return effectData;
        }

        public void UpdateOrder(Models.Order order)
        {
            string sql = @"UPDATE [Sales].[Orders]
   SET [CustomerID] = @custid
      ,[EmployeeID] = @empid
      ,[OrderDate] = @orderdate
      ,[RequiredDate] = @requireddate
      ,[ShippedDate] = @shippeddate
      ,[ShipperID] = @shipperid
      ,[Freight] = @freight
      ,[ShipName] = @shipname
      ,[ShipAddress] = @shipaddress
      ,[ShipCity] = @shipcity
      ,[ShipRegion] = @shipregion
      ,[ShipPostalCode] = @shippostalcode
      ,[ShipCountry] = @shipcountry
 WHERE [OrderID] = @orderid
            ";
            string format1 = order.Orderdate.ToString("yyyy-MM-dd");
            string format2 = order.RequiredDate.ToString("yyyy-MM-dd");
            string format3 = order.ShippedDate.ToString("yyyy-MM-dd");

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@orderid", order.OrderId));
                cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@empid", order.EmpId));

                cmd.Parameters.Add(new SqlParameter("@orderdate", format1));
                cmd.Parameters.Add(new SqlParameter("@requireddate", format2));
                cmd.Parameters.Add(new SqlParameter("@shippeddate", format3));
                cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<Models.Order> GetOrderById(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT TOP (100) O.OrderID, O.CustomerID as CustId, C.CompanyName as CustName, E.EmployeeID as EmpId
,E.FirstName+E.LastName as EmpName,  O.OrderDate as OrderDate
,O.RequiredDate as RequiredDate,O.ShippedDate as ShippedDate
, S.ShipperID
, S.CompanyName as ShipperName,convert(float, O.Freight, 1) as Freight, O.ShipName ,O.ShipAddress, O.ShipCity, O.ShipRegion
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
            string sql = @"SELECT TOP (100) O.OrderID, O.CustomerID as CustId, C.CompanyName as CustName, E.EmployeeID as EmpId
,E.FirstName+E.LastName as EmpName,  O.OrderDate as OrderDate
,O.RequiredDate as RequiredDate,O.ShippedDate as ShippedDate
, S.ShipperID
, S.CompanyName as ShipperName, convert(float, O.Freight, 1) as Freight, O.ShipName ,O.ShipAddress, O.ShipCity, O.ShipRegion
,O.ShipPostalCode , O.ShipCountry
  FROM [Sales].[Orders] as O 
  JOIN [HR].[Employees] as E ON O.EmployeeID=E.EmployeeID
  JOIN [Sales].[Customers] as C ON O.CustomerID=C.CustomerID
  JOIN [Sales].[Shippers] as S ON O.ShipperID=S.ShipperID
ORDER BY O.OrderID DESC";

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
                    OrderId = (int)row["OrderId"],
                    CustId = (int)row["CustId"],
                    CustName = row["CustName"].ToString(),
                    EmpId = (int)row["EmpId"],
                    EmpName = row["EmpName"].ToString(),
                    Orderdate = (DateTime)row["OrderDate"],
                    RequiredDate = (DateTime)row["RequiredDate"],
                    ShippedDate = (DateTime)row["ShippedDate"],
                    ShipperId = (int)row["ShipperId"],
                    ShipperName = row["ShipperName"].ToString(),
                    Freight = (Double)row["Freight"],
                    ShipName = row["ShipName"].ToString(),
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString(),
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString(),



                });
            }
            return result;
        }
    }
}