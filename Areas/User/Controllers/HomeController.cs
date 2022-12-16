using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        // GET: Home
        public ActionResult Index()
        {
            //Đếm số lượng đồ án
            ViewBag.DoAn = db.Tbl_DoAn.Count();

            //Số lương sinh viên tham gia đồ án
            ViewBag.SinhVien = db.Tbl_SinhVien.Count();

            //Số lượng giáo viên
            ViewBag.GiaoVien = db.Tbl_GiaoVien.Count();

            //Số lượng chuyên ngành
            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.Count();
            return View();
        }
    }
}