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

        public void DeleteOrderDetail(int OrderId, int ProductId)
        {
            string sql = @"DELETE FROM [Sales].[OrderDetails]
      WHERE [OrderID]=@OrderId AND [ProductId]=@ProductId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                cmd.Parameters.Add(new SqlParameter("@ProductId", ProductId));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
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
                cmd.Parameters.Add(new SqlParameter("@freight", order.Freight.ToString()=="0" ? string.Empty : order.Freight.ToString()));
                cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipName == null ? string.Empty : order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress == null ? string.Empty : order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity == null ? string.Empty : order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion == null ? string.Empty : order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode == null ? string.Empty : order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry == null ? string.Empty : order.ShipCountry));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void UpdateOrderDetail(Models.OrderDetail orderdetail)
        {
            string sql = @"UPDATE [Sales].[OrderDetails]
   SET [UnitPrice] = @unitprice
      ,[Qty] = @qty
      
 WHERE [OrderID] = @orderid AND [ProductID] = @productid
            ";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@orderid", orderdetail.OrderId));
                cmd.Parameters.Add(new SqlParameter("@productid", orderdetail.ProductId));
                cmd.Parameters.Add(new SqlParameter("@unitprice", orderdetail.UnitPrice));
                cmd.Parameters.Add(new SqlParameter("@qty", orderdetail.Qty));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<Models.Order> GetOrderByCondition(Models.Order searchCondition)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  O.OrderID, O.CustomerID as CustId, C.CompanyName as CustName, E.EmployeeID as EmpId
,E.FirstName+E.LastName as EmpName,  O.OrderDate as OrderDate
,O.RequiredDate as RequiredDate,O.ShippedDate as ShippedDate
, S.ShipperID
, S.CompanyName as ShipperName,convert(float, O.Freight, 1) as Freight, O.ShipName ,O.ShipAddress, O.ShipCity, O.ShipRegion
,O.ShipPostalCode , O.ShipCountry
  FROM [Sales].[Orders] as O 
  JOIN [HR].[Employees] as E ON O.EmployeeID=E.EmployeeID
  JOIN [Sales].[Customers] as C ON O.CustomerID=C.CustomerID
  JOIN [Sales].[Shippers] as S ON O.ShipperID=S.ShipperID
WHERE (O.OrderID Like '%'+@OrderId+'%' OR @OrderId = '')
AND  (C.CompanyName Like '%'+@CustName+'%')
AND  (E.EmployeeID = @EmpId OR @EmpId = '')
AND  (S.ShipperID = @ShipperId OR @ShipperId = '')
AND  (O.OrderDate = @Orderdate OR @Orderdate = '')
AND  (O.ShippedDate = @ShippedDate OR @ShippedDate = '')
AND  (O.RequiredDate = @RequiredDate OR @RequiredDate = '')
";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", searchCondition.OrderId.ToString()=="0" ? string.Empty : searchCondition.OrderId.ToString()));
                cmd.Parameters.Add(new SqlParameter("@CustName", searchCondition.CustName==null ? string.Empty : searchCondition.CustName));
                cmd.Parameters.Add(new SqlParameter("@EmpId", searchCondition.EmpId.ToString() == "0" ? string.Empty : searchCondition.EmpId.ToString()));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", searchCondition.ShipperId.ToString() == "0" ? string.Empty : searchCondition.ShipperId.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", searchCondition.Orderdate.ToString("yyyy-MM-dd") == "0001-01-01" ? string.Empty : searchCondition.Orderdate.ToString("yyyy-MM-dd")));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", searchCondition.ShippedDate.ToString("yyyy-MM-dd") == "0001-01-01" ? string.Empty : searchCondition.ShippedDate.ToString("yyyy-MM-dd")));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", searchCondition.RequiredDate.ToString("yyyy-MM-dd") == "0001-01-01" ? string.Empty : searchCondition.RequiredDate.ToString("yyyy-MM-dd")));



                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();

            }
            return this.MapOrderDataToList(dt);
        
        }

        public List<Models.Order> GetOrderById(String id)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  O.OrderID, O.CustomerID as CustId, C.CompanyName as CustName, E.EmployeeID as EmpId
,E.FirstName+E.LastName as EmpName,  O.OrderDate as OrderDate
,O.RequiredDate as RequiredDate,O.ShippedDate as ShippedDate
, S.ShipperID
, S.CompanyName as ShipperName,convert(float, O.Freight, 1) as Freight, O.ShipName ,O.ShipAddress, O.ShipCity, O.ShipRegion
,O.ShipPostalCode , O.ShipCountry
  FROM [Sales].[Orders] as O 
  JOIN [HR].[Employees] as E ON O.EmployeeID=E.EmployeeID
  JOIN [Sales].[Customers] as C ON O.CustomerID=C.CustomerID
  JOIN [Sales].[Shippers] as S ON O.ShipperID=S.ShipperID
WHERE O.OrderID = @OrderId";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", id));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();

            }
            return this.MapOrderDataToList(dt);

        }

        public List<Models.Order> GetAllOrder()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT TOP (50) O.OrderID, O.CustomerID as CustId, C.CompanyName as CustName, E.EmployeeID as EmpId
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

        public List<Models.OrderDetail> GetOrderDetailById(String orderid)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  OrderId, P.ProductId, ProductName, convert(float, OD.UnitPrice, 1) as UnitPrice, Qty
FROM [Sales].[OrderDetails] OD JOIN [Production].[Products] as P ON OD.ProductId = P.ProductId
WHERE OrderID = @OrderId";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderid));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDetailDataToList(dt);
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

        private List<Models.OrderDetail> MapOrderDetailDataToList(DataTable orderData)
        {
            List<Models.OrderDetail> result = new List<OrderDetail>();
            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new OrderDetail()
                {
                    OrderId = (int)row["OrderId"],
                    ProductId = (int)row["ProductId"],
                    ProductName = row["ProductName"].ToString(),
                    UnitPrice = (double)(row["UnitPrice"]),
                    Qty = (Int16)row["Qty"],
                });
            }
            return result;
        }
    }
}