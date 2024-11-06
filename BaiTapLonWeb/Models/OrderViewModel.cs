using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTapLonWeb.Models
{
    public class OrderViewModel
    {
        //public List<Order> Orders { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public int ProcessingCount { get; set; }
        public int DeliveringCount { get; set; }
        public int SuccessCount { get; set; }
        public int CancelledCount { get; set; }
        public int TotalCount { get; set; }
    }
}