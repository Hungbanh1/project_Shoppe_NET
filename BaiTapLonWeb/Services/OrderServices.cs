using BaiTapLonWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTapLonWeb.Services
{
    public class OrderServices 
    {
        public List<ExcelModel> GetOrderDetailForExcel(int orderId)
        {
            using (var db = new ShopeeEntities())
            {
                var lsDetail = db.OrderDetails
                    .Join(db.Products,
                        orderDetail => orderDetail.ProductID,
                        product => product.ProductID,
                        (orderDetail, product) => new { OrderDetail = orderDetail, Product = product })
                    .Join(db.Orders,
                        combined => combined.OrderDetail.OrderID,
                        order => order.OrderID,
                        (combined, order) => new { combined.OrderDetail, combined.Product, Order = order })
                    .Where(x => x.OrderDetail.OrderID == orderId)
                    .Select(x => new ExcelModel
                    {
                        OrderId = (int)x.OrderDetail.OrderID,
                        CodeOrder = x.OrderDetail.CodeOrder,
                        UserName = x.Order.User.UserName,
                        ProductName = x.Product.ProductName,
                        Amount = (int)x.OrderDetail.Amount,
                        Price = (int)x.OrderDetail.Price,
                        TotalMoney = (int)x.Order.TotalMoney,
                        DateOrder = (DateTime)x.Order.DateOrder // Lấy DateOrder từ bảng Orders
                    }).ToList();

                return lsDetail;
            }
        }
        public List<ExcelModel> GetAllOrderForExcel()
        {
            using (var db = new ShopeeEntities())
            {
                var lsDetail = db.OrderDetails
                    .Join(db.Products,
                        orderDetail => orderDetail.ProductID,
                        product => product.ProductID,
                        (orderDetail, product) => new { OrderDetail = orderDetail, Product = product })
                     .Join(db.Orders,
                        orderDetail => orderDetail.OrderDetail.OrderID,
                        order => order.OrderID,
                        (orderDetail, order) => new { orderDetail.OrderDetail, orderDetail.Product, Order = order })
                     .Where(x => x.OrderDetail.Order.User.Role == false)
                     .Select(x => new ExcelModel
                            {
                            OrderId = (int)x.OrderDetail.OrderID,
                            CodeOrder = x.OrderDetail.CodeOrder,
                            UserName = x.Order.User.UserName,
                            ProductName = x.Product.ProductName,
                            Amount = (int)x.OrderDetail.Amount,
                            Price = (int)x.OrderDetail.Price,
                            TotalMoney = (int)x.Order.TotalMoney,
                            DateOrder = (DateTime)x.Order.DateOrder // Lấy DateOrder từ bảng Orders


                      }).ToList();
                return lsDetail;

            }
        }

    }
}