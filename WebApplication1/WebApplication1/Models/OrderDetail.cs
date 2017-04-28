using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OrderDetail
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        ///
        [Display(Name = "訂單編號")]
        public int OrderId { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        [Display(Name = "產品編號")]
        public int ProductId { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        [Display(Name = "產品名稱")]
        public String ProductName { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        [Display(Name = "單價")]
        public Double UnitPrice { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        [Display(Name = "數量")]
        public Int16 Qty { get; set; }
    }
}