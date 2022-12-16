using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class LoginController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(Tbl_User entity)
        {
            if(db.Tbl_User.Count(x => x.Account == entity.Account && x.Password == entity.Password) > 0)
            {
                var user = db.Tbl_User.FirstOrDefault(x => x.Account == entity.Account && x.Password == entity.Password);
                if(user.Type == 0)//admin
                {
                    Session["admin"] = user;
                    return Redirect("/");
                }
                else if(user.Type == 1)
                {
                    Session["user"] = user;
                    return Redirect("/User/Home");
                }
                else if (user.Type == 2)
                {
                    Session["student"] = user;
                    return Redirect("/Student/Home");
                }
                else if (user.Type == 3)
                {
                    Session["teacher"] = user;
                    return Redirect("/Teacher/Home");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                TempData["add_success"] = "Tài khoản hoặc mật khẩu không đúng";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegister(Tbl_User entity)
        {
            var user = new Tbl_User();
            user.Account = entity.Account;
            user.Password = entity.Password;
            user.Fullname = entity.Fullname;
            user.Type = 2;
            user.Status = true;
            db.Tbl_User.Add(user);
            db.SaveChanges();
            TempData["add_success"] = "Đăng ký tài khoản thành công. Vui lòng đăng nhập để truy cập hệ thống.";
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["admin"] = null;
            Session["teacher"] = null;
            Session["student"] = null;

            return Redirect("/Login");
        }
    }
}