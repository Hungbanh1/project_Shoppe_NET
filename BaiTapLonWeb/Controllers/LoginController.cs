using BaiTapLonWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BaiTapLonWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        ShopeeEntities db = new ShopeeEntities();
      
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult LoginAdmin()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AuthenAdmin(User user)
        {
            var check = db.Users.Where(s => s.Email.Equals(user.Email) && s.PassWord.Equals(user.PassWord)).FirstOrDefault();
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PassWord))
            {

                return View("LoginAdmin", user);
            }
            else if (check.Role == false || check == null)
            {
                ModelState.AddModelError("PassWord", "Tài khoản và mật khẩu không hợp lệ vui lòng liên hệ với admin");
                return View("LoginAdmin", user);
            }
            else
            {
                Session["UserID"] = check.UserID;
                Session["Email"] = user.Email;
                Session["UserName"] = user.UserName;
                return RedirectToAction("Dashboard", "AdminOrder");
            }
            //else
            //{
            //    Session["UserID"] = check.UserID;
            //    Session["Email"] = user.Email;
            //    Session["UserName"] = user.UserName;
            //    return RedirectToAction("Index", "Products");

            //}

        }
        //Method Login - login user
        [HttpPost]
        public ActionResult Authen(User user)
        {
            //Console.WriteLine("Dasdasda");
            //Console.WriteLine("Giá trị của user.Role: " + (user.Role.HasValue ? user.Role.Value.ToString() : "null"));
            //Console.ReadLine();
            //return View(user);
            var check = db.Users.Where(s => s.Email.Equals(user.Email) && s.PassWord.Equals(user.PassWord)).FirstOrDefault();

            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PassWord))
            {

                return View("Index", user);
                //return View();
            }
            else if (check.Role == true || check == null)
            {
                ModelState.AddModelError("PassWord", "Tài khoản hoặc mật khẩu không hợp lệ");
                return View("Index", user);
            }
            else
            {
                Session["UserID"] = check.UserID;
                Session["Email"] = user.Email;
                Session["Role"] = check.Role;
                TempData["ShowAlert"] = true;
                TempData["Message"] = "Từ ngày 1/1/2024, bạn cần cập nhật thông tin tài khoản của mình nếu còn thiếu để đảm bảo không bị gián đoạn dịch vụ. Nếu đã cập nhật vui lòng bỏ qua thông báo này.";
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        //method xử lý model
        [HttpPost]
        public ActionResult Register(User users)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email == users.Email);
                if (check != null)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại ! Vui lòng sử dụng 1 email khác");
                    return View();
                }
                else
                {
                    users.Role = false;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(users);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Login");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        public ActionResult LogoutAdmin()
        {
            Session.Abandon();
            return RedirectToAction("LoginAdmin", "Login");
        }

        public ActionResult Edit(int ?userId)
        {
             userId = (int)Session["UserID"];

            User user = db.Users.Find(userId);
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

     
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
                  
                   // Cập nhật thông tin người dùng
                    existingUser.UserName = users.UserName;
                    existingUser.Email = users.Email;
                    existingUser.Address = users.Address;
                    existingUser.PhoneNumber = users.PhoneNumber; 
                    var check = db.Users.FirstOrDefault(s => s.Email == users.Email && s.UserID != users.UserID);
                    if (check != null)
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại ! Vui lòng sử dụng 1 email khác");
                        TempData["FailMessage"] = "Thêm sản phẩm thành công";
                        users.ImageUser = form["oldimage"];
                        return View(users);
                    }

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
                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công";
                    return RedirectToAction("Edit", "Login");
                    
                }
                catch
                {
                    ViewBag.Message = "Không Thành Công!";
                }
            }

            return View(users);
        }
        public ActionResult EditAdmin(int? userId)
        {
            userId = (int)Session["UserID"];

            User user = db.Users.Find(userId);
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditAdmin([Bind(Include = "UserID,UserName,Email,PassWord,Address,PhoneNumber,ImageUser,Role")] User users, HttpPostedFileBase ImageUser, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    User existingUser = db.Users.Find(users.UserID);

                    // Cập nhật thông tin người dùng
                    existingUser.UserName = users.UserName;
                    existingUser.Email = users.Email;
                    existingUser.Address = users.Address;
                    existingUser.PhoneNumber = users.PhoneNumber;
                    var check = db.Users.FirstOrDefault(s => s.Email == users.Email && s.UserID != users.UserID);
                    if (check != null)
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại ! Vui lòng sử dụng 1 email khác");
                        TempData["FailMessage"] = "Thêm sản phẩm thành công";
                        users.ImageUser = form["oldimage"];
                        return View(users);
                    }

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
                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công";
                    return RedirectToAction("Edit", "Login");

                }
                catch
                {
                    ViewBag.Message = "Không Thành Công!";
                }
            }

            return View(users);
        }
    }
}