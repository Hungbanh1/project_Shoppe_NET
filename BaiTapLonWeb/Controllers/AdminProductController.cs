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
using System.IO;
using PagedList;

namespace BaiTapLonWeb.Controllers
{
    public class AdminProductController : Controller
    {
        private ShopeeEntities db = new ShopeeEntities();

        // GET: AdminProduct


        // GET: AdminProduct
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10,int CateID =0)
        {
            var query = db.Products.Include(p => p.Category);
            ViewBag.KeyWord = searchString;
            if (!string.IsNullOrEmpty(searchString))
            
                query = query.Where(p => p.ProductName.Contains(searchString) || p.Category.CategoryName.Contains(searchString));
            
            if (CateID != 0)
           query=query.Where(c=>c.CategoryID==CateID);

            var products = query.OrderByDescending(x => x.CategoryID).ToList();
            var model = products.ToPagedList(page, pageSize);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(model);
        }




        // GET: AdminProduct/Details/5
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

        // GET: AdminProduct/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: AdminProduct/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,ProductDescription,ProductPrice,ProductImage,ProductQuantity,ProductSold,CategoryID")] Product products, HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ProductImage.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(ProductImage.FileName);
                        string _path = Path.Combine(Server.MapPath("~/public/images"), _FileName);
                        ProductImage.SaveAs(_path);
                        products.ProductImage = _FileName;
                        products.ProductSold = 0;
                        products.ProductQuantity = 0;
                    }
                    db.Products.Add(products);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminProduct");
                }
                catch
                {
                    ViewBag.Message = "Không Thành Công!";
                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            return View(products);
        }

        // GET: AdminProduct/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            return View(products);
        }

        // POST: AdminProduct/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProductDescription,ProductPrice,ProductImage,ProductQuantity,ProductSold,CategoryID")] Product products, HttpPostedFileBase ProductImage, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ProductImage != null)
                    {
                        string _FileName = Path.GetFileName(ProductImage.FileName);
                        string _path = Path.Combine(Server.MapPath("~/public/images"), _FileName);
                        ProductImage.SaveAs(_path);
                        products.ProductImage = _FileName;
                        //get path of old images for delete it
                        _path = Path.Combine(Server.MapPath("~/public/images"), form["oldimage"]);
                        if (System.IO.File.Exists(_path))
                            System.IO.File.Delete(_path);
                    }
                    else
                    {
                        products.ProductImage = form["oldimage"];
                        db.Entry(products).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "AdminProduct");
                    }
                }
                catch
                {
                    ViewBag.Message = "Không thành công";
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            return View(products);
        }

        // GET: AdminProduct/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: AdminProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
