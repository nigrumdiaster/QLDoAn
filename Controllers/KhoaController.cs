using PagedList;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class KhoaController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_Khoa.OrderByDescending(x => x.TenKhoa).ToPagedList(page, pageSize);
            return View(model);
        }


        //Xóa 
        public JsonResult Delete(long ID)
        {

            try
            {
                var khoa = db.Tbl_Khoa.Find(ID);
                db.Tbl_Khoa.Remove(khoa);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }

        }


        [HttpPost]
        public ActionResult Add(Tbl_Khoa entity)
        {

          

            var dem = db.Tbl_Khoa.Count(x => x.TenKhoa == entity.TenKhoa);
            if (dem > 0)
            {
                TempData["add_success1"] = " Thêm thất bại ! khoa này đã tồn tại!";
                return RedirectToAction("Index");

            }
            else
            {
                db.Tbl_Khoa.Add(entity);
                db.SaveChanges();
                TempData["add_success"] = "Thêm khoa thành công";
                return RedirectToAction("Index");

            }
          

        }

        [HttpPost]
        public ActionResult Edit(Tbl_Khoa entity)
        {



            var prv = db.Tbl_Khoa.Find(entity.ID);
            prv.TenKhoa = entity.TenKhoa;
            prv.MaKhoa = entity.MaKhoa;
            var dem = db.Tbl_Khoa.Count(x => x.TenKhoa == prv.TenKhoa );
            if (dem > 0)
            {
                TempData["add_success1"] = " Sửa thất bại ! khoa này đã tồn tại!";
                return RedirectToAction("Index");

            }



            db.SaveChanges();
            TempData["add_success"] = "Sửa khoa thành công";
            return RedirectToAction("Index");
        }

        public JsonResult GetByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var khoa = db.Tbl_Khoa.Find(ID);
            return Json(khoa, JsonRequestBehavior.AllowGet);
        }
    }
}