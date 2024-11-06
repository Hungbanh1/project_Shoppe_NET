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
using PagedList;
using System.IO;

namespace BaiTapLonWeb.Controllers
{
    public class AdminUserController : Controller
    {
        private ShopeeEntities db = new ShopeeEntities();

        // GET: AdminUser

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: AdminUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: AdminUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "UserID,UserName,Email,PassWord,Address,PhoneNumber,ImageUser,Role")] User users, HttpPostedFileBase ImageUser)
        {
            if (ModelState.IsValid)
            {
                try
                { 

                  
                
                    var check = db.Users.FirstOrDefault(s => s.Email == users.Email && s.UserID != users.UserID);
                    if (check != null)
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại ! Vui lòng sử dụng 1 email khác");
                        return View(users);
                    }
                    if (ImageUser != null && ImageUser.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(ImageUser.FileName);
                        string _path = Path.Combine(Server.MapPath("~/public/images"), _FileName);
                        ImageUser.SaveAs(_path);
                        users.ImageUser = _FileName;
                    }
                    db.Users.Add(users);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminUser");
                }
                catch
                {
                    ViewBag.Message = "Không Thành Công!";
                }
            }

                return View(users);
        }

        // GET: AdminUser/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: AdminUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Email,PassWord,Address,PhoneNumber,ImageUser,Role")] User users, HttpPostedFileBase ImageUser, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User existingUser = db.Users.Find(users.UserID);
                    users.Role = bool.Parse(Request.Form["Role"]);
                    // Cập nhật thông tin người dùng
                    existingUser.UserName = users.UserName;
                    existingUser.Email = users.Email;
                    existingUser.Address = users.Address;
                    existingUser.PhoneNumber = users.PhoneNumber;

                    bool? role = bool.Parse(Request.Form["Role"]);
                    existingUser.Role = role;
                    if (ImageUser != null)
                    {
                        string _FileName = Path.GetFileName(ImageUser.FileName);
                        string _path = Path.Combine(Server.MapPath("~/public/images"), _FileName);
                        ImageUser.SaveAs(_path);
                        existingUser.ImageUser = _FileName;
                        _path = Path.Combine(Server.MapPath("~/public/images"), form["oldimage"]);
                        if (System.IO.File.Exists(_path))
                            System.IO.File.Delete(_path);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(users.PassWord))
                        {
                            existingUser.PassWord = users.PassWord;
                        }
                        existingUser.ImageUser = form["oldimage"];
                    }

                    db.Entry(existingUser).State = EntityState.Modified;
                   
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminUser");
                }
                catch
                {
                    ViewBag.Message = "Không Thành Công!";
                }
            }

            return View(users);
        }


        // GET: AdminUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: AdminUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User users = db.Users.Find(id);
            db.Users.Remove(users);
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
