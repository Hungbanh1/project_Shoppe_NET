using BaiTapLonWeb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BaiTapLonWeb.Controllers
{
    public class ShopeeCartController : Controller
    {
        ShopeeEntities db = new ShopeeEntities();
        // GET: ShopeeCart


        public CartModel GetCart()
        {
            CartModel cart = Session["CartModel"] as CartModel;
            if (cart == null || Session["CartModel"] == null)
            {
                cart = new CartModel();
                Session["CartModel"] = cart;

            }
            return cart;
        }
        //Add Item vao Gio hang
        public ActionResult AddToCart(int id)
        {
            var pro = db.Products.SingleOrDefault(s => s.ProductID == id);
            if (pro != null)
            {
                GetCart().Add(pro);
                
            }
            if (pro == null)
            {
                return RedirectToAction("ShowToCart", "ShopeeCart");
            }
            return RedirectToAction("ShowToCart", "ShopeeCart");

        }
        public ActionResult AddToCart1(int id)
        {
            var pro = db.Products.SingleOrDefault(s => s.ProductID == id);
            if (pro != null)
            {
                GetCart().Add(pro);
            }
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công";
            return RedirectToAction("Details", "Products");
            
        }


        //Hien thi san pham trong gio hang
        public ActionResult ShowToCart()
        {

            CartModel cart = Session["CartModel"] as CartModel;

            if (cart == null)
            {
                // Giỏ hàng rỗng, bạn có thể thực hiện các xử lý tương ứng, ví dụ:
                ViewBag.CartEmpty = true;
                return View();
            }

            ViewBag.CartEmpty = false;
            return View(cart);


        }
        public ActionResult UpdateAmount(FormCollection form)
        {
            CartModel cart = Session["CartModel"] as CartModel;
            int id_product = int.Parse(form["ID_Product"]);
            int amount = int.Parse(form["Amount"]);
            cart.Update_Amount(id_product, amount);
            return RedirectToAction("ShowToCart", "ShopeeCart");
        }
        public ActionResult RemoveItem(int id)
        {
            CartModel cart = Session["CartModel"] as CartModel;
            cart.Remove_Item(id);
           
            return RedirectToAction("ShowToCart", "ShopeeCart");
            
        }
        public PartialViewResult BagCart()
        {
            int total_item = 0;
            CartModel cart = Session["CartModel"] as CartModel;
            if (cart != null)
            {


                total_item = cart.Total_Amount();
                ViewBag.AmountCart = total_item;

            }
          /*  else
            {
                ViewBag.AmountCart = "Rỗ";

            }*/
            return PartialView("BagCart");

        }
        public ActionResult Payment()
        {
            var userId = Session["UserID"] as int?;
            var UserById = db.Users.Find(userId);

          

            if (ModelState.IsValid)
            {
                CartModel cart = (CartModel)Session["CartModel"];
                if (cart != null)
                {
                    if (UserById.Address == null || UserById.PhoneNumber == null)
                    {
                        TempData["AddressorPhoneNull"] = "Bạn cần cập nhật địa chỉ và số điện thoại trước khi đặt hàng";
                        return RedirectToAction("ShowToCart", "ShopeeCart");
                    }
                    if (Session["UserID"] == null)
                    {
                        TempData["FailMessage"] = "Bạn Cần Đăng Nhập Trước khi Đặt Hàng";
                        return RedirectToAction("ShowToCart", "ShopeeCart");
                    }
                    else { 
                        int id = (int)Session["UserID"];
                        Order order = new Order();
                        string prefix = "SP-19";
                        Random random = new Random();
                        string randomDigits = random.Next(10000000, 99999999).ToString("D8"); // Đảm bảo có 8 chữ số
                        //order.CodeOrder = $"{prefix}{randomDigits}";

                        order.UserID = id;
                        order.TotalMoney = cart.Items.Sum(x => (x.Amount * x._products.ProductPrice));
                        order.DateOrder = DateTime.Now;
                        //order.DateOrder = Convert.ToDateTime(DateTime.Now).ToString("yyy-MM--dd");
                        
                        order.StatusID = 1;
                        //order.CodeOrder = $"{prefix}{randomDigits}";
                        order.CodeOrder = prefix + randomDigits.ToString();
                        var CodeOrder = order.CodeOrder;
                        //order.CodeOrder = $"{prefix}{randomDigits}";
                        db.Orders.Add(order);
                        db.SaveChanges();
                        cart.Items.ForEach(x =>
                        {
                            var orderDetail = new OrderDetail
                            {
                                ProductID = x._products.ProductID,
                                Amount = x.Amount,
                                Price = x._products.ProductPrice,
                                CodeOrder = CodeOrder // Lấy `CodeOrder` từ `order`
                            };

                            order.OrderDetails.Add(orderDetail); // Thêm `orderDetail` vào `OrderDetails` của `order`
                        });

                        // Lưu toàn bộ `OrderDetail` vào cơ sở dữ liệu
                        //try
                        //{
                        //    db.SaveChanges();
                        //}
                        //catch (DbEntityValidationException e)
                        //{
                        //    foreach (var eve in e.EntityValidationErrors)
                        //    {
                        //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        //        foreach (var ve in eve.ValidationErrors)
                        //        {
                        //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //                ve.PropertyName, ve.ErrorMessage);
                        //        }
                        //    }
                        //    throw;
                        //}
                        db.SaveChanges();

                        cart.ClearCart();
                        TempData["SuccessMessage"] = "Đặt Hàng Thành Công";

                    }
                }
                 
                

            }
            return RedirectToAction("ShowToCart", "ShopeeCart");

        }
        public ActionResult DeleteAll()
        {
            CartModel cart = (CartModel)Session["CartModel"];
            cart.ClearCart();
            return RedirectToAction("ShowToCart", "ShopeeCart");
        }
    }
}