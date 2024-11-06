using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BaiTapLonWeb.Models;
using System.EnterpriseServices.CompensatingResourceManager;


namespace BaiTapLonWeb.Controllers
{
    public class ProductsController : Controller
    {
        private ShopeeEntities db = new ShopeeEntities();
        public Category cate = new Category();
        public Product pro = new Product();
        // GET: Products
        public ActionResult Index()
        {
            var userId = Session["UserID"] as int?;
            var products = db.Products.Include(p => p.Category);
            var categories = db.Categories.Include(p => p.Products);
            var UserById = db.Users.Find(userId);

            if (UserById == null)
            {
                // Xử lý khi không tìm thấy người dùng, ví dụ gán giá trị mặc định hoặc thông báo lỗi
                return HttpNotFound("User không tồn tại");
            }

            HomeModel home = new HomeModel
            {
                UserById = UserById,
                ProductsView = products,
                CategoriesView = categories
            };

            return View(home);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        //: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult CategoryDetails(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cate1 = db.Categories.Find(id);
            if (cate1 == null)
            {
                return HttpNotFound();
            }
            //Lấy tên cate hiển thị trong Viewbag
            ViewBag.CategoryName = cate1.CategoryName;
            //Giống như diều kiện if so sánh và lấy giá trị thuộc danh mục
            var products = db.Products.Where(p => p.CategoryID == id).ToList();

            return View(products.ToList());

        }
        public ActionResult Search(string searchTerm)
        {
            var products = db.Products.Include(p => p.Category);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm) );
            }
            ViewBag.SearchTerm = searchTerm;    
            return View(products.ToList());
        }

    }
}
