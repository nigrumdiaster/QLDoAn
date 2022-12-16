using PagedList;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class HeDaoTaoController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        // GET: NienKhoa
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_HeDaoTao.OrderByDescending(x => x.TenHe).ToPagedList(page, pageSize);
            return View(model);
        }


        //Xóa 
        public JsonResult Delete(long ID)
        {

            try
            {
                var hedt = db.Tbl_HeDaoTao.Find(ID);
                db.Tbl_HeDaoTao.Remove(hedt);
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
        public ActionResult Add(Tbl_HeDaoTao entity)
        {

            var dem = db.Tbl_HeDaoTao.Count(x => x.TenHe.Trim() == entity.TenHe.Trim() );
            if (dem > 0)
            {
                TempData["add_success1"] = " Thêm thất bại ! hệ đào tạo này này đã tồn tại!";
                return RedirectToAction("Index");

            }
            else
            {
                 db.Tbl_HeDaoTao.Add(entity);
                db.SaveChanges();
                TempData["add_success"] = "Thêm hệ đào tạo thành công.";
                return RedirectToAction("Index");

            }


           

        }

        [HttpPost]
        public ActionResult Edit(Tbl_HeDaoTao entity)
        {
            var prv = db.Tbl_HeDaoTao.Find(entity.ID);
            prv.TenHe = entity.TenHe;
            prv.MaHe = entity.MaHe;
            var dem = db.Tbl_HeDaoTao.Count(x => x.TenHe == prv.TenHe);
            if (dem > 0)
            {
                TempData["add_success1"] = " Sửa thất bại ! hệ đào tạo này đã tồn tại!";
                return RedirectToAction("Index");

            }

            else
            {
                db.SaveChanges();
                TempData["add_success"] = "Sửa hệ đào tạo thành công";
                return RedirectToAction("Index");
            }

           
        }

        public JsonResult GetByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var hedt = db.Tbl_HeDaoTao.Find(ID);
            return Json(hedt, JsonRequestBehavior.AllowGet);
        }
    }
}