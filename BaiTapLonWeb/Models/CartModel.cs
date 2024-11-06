using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BaiTapLonWeb.Models
{
    public class Cart_Item
    {
        public Product _products { get; set; }
        public int Amount { get; set; }
    }
    //Gio Hang
    public class CartModel
    {
        List<Cart_Item> items =new List<Cart_Item>();
        public IEnumerable<Cart_Item> Items
        {
            get { return items; }
        }
        public void Add(Product _pro,int amount=1 )
        {
            var item = items.FirstOrDefault(x => x._products.ProductID == _pro.ProductID);
            if(item == null)
            {
                items.Add(new Cart_Item
                {
                    _products = _pro,
                    Amount = amount
                });
            }
            else
            {
                item.Amount+=amount;
            }   
        }
        public void Update_Amount(int id,int amount)
        {
            var item = items.Find(s => s._products.ProductID == id);
            if(item != null)
            {
                item.Amount =amount; 
            }
        }
        public double Total_Money()
        {
            var total = items.Sum(s => s._products.ProductPrice * s.Amount);
            return (double)total;
        }
        public void Remove_Item(int id)
        {
            items.RemoveAll(s=>s._products.ProductID == id);
        }
        public int Total_Amount()
        {
            return items.Sum(s => s.Amount);
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}