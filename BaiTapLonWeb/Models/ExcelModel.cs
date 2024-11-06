using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTapLonWeb.Models
{
    public class ExcelModel
    {
        public int OrderId { get; set; }

        public string CodeOrder {  get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }

        public int Amount { get; set; }
        public decimal Price { get; set; }

        public DateTime DateOrder { get; set; }


    }
}