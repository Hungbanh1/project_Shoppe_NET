using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTapLonWeb.Models
{
    public class HomeModel
    {
        public IEnumerable<Category> CategoriesView { get; set; }
        public IEnumerable<Product> ProductsView { get; set; }

        public IEnumerable<User> ListUser { get; set; }

        public User UserById { get; set; }
    }
}