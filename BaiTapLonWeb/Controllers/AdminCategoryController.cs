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
    public class AdminCategoryController : Controller
    {
        private ShopeeEntities db = new ShopeeEntities();

       

        // GET: AdminProduct
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10,int CateID =0)
        {
            // Truy vấn trực tiếp bảng Categories
            var query = db.Categories.AsQueryable();
            ViewBag.KeyWord = searchString;

            // Tìm kiếm theo tên danh mục nếu có từ khóa
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.CategoryName.Contains(searchString));
            }

            // Phân trang và sắp xếp
            var categories = query.OrderByDescending(c => c.CategoryID).ToList();
            var model = categories.ToPagedList(page, pageSize);

            return View(model); 
        }




        // GET: AdminProduct/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: AdminProduct/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(new Category());
        }

        // POST: AdminCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,CategoryImage")] Category category, HttpPostedFileBase CategoryImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (CategoryImage.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(CategoryImage.FileName);
                        string _path = Path.Combine(Server.MapPath("~/public/images"), _FileName);
                        CategoryImage.SaveAs(_path);
                        category.CategoryImage = _FileName;
                   
                    }
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminCategory");
                }
                catch
                {
                    ViewBag.Message = "Không Thành Công!";
                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", category.CategoryID);
            return View(category);
        }

        // GET: AdminProduct/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", category.CategoryID);
            return View(category);
        }

        // POST: AdminProduct/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,CategoryImage")] Category category, HttpPostedFileBase CategoryImage, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (CategoryImage != null)
                    {
                        string _FileName = Path.GetFileName(CategoryImage.FileName);
                        string _path = Path.Combine(Server.MapPath("~/public/images"), _FileName);
                        CategoryImage.SaveAs(_path);
                        category.CategoryImage = _FileName;
                        //get path of old images for delete it
                        _path = Path.Combine(Server.MapPath("~/public/images"), form["oldimage"]);
                        if (System.IO.File.Exists(_path))
                            System.IO.File.Delete(_path);
                    }
                    else
                    {
                        category.CategoryImage = form["oldimage"];
                        db.Entry(category).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "AdminCategory");
                    }
                }
                catch
                {
                    ViewBag.Message = "Không thành công";
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", category.CategoryID);
            return View(category);
        }

        // GET: AdminProduct/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: AdminProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
