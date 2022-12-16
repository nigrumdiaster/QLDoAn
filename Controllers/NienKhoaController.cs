using PagedList;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class NienKhoaController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        // GET: NienKhoa
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_NienKhoa.OrderByDescending(x => x.NienKhoa).ToPagedList(page, pageSize);
            return View(model);
        }


        //Xóa 
        public JsonResult Delete(long ID)
        {

            try
            {
                var nienkhoa = db.Tbl_NienKhoa.Find(ID);
                db.Tbl_NienKhoa.Remove(nienkhoa);
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
        public ActionResult addNienKhoa(Tbl_NienKhoa entity)
        {

            db.Tbl_NienKhoa.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Thêm niên khóa thành công";
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult editNienKhoa(Tbl_NienKhoa entity)
        {
                var prv = db.Tbl_NienKhoa.Find(entity.ID);
                prv.NienKhoa = entity.NienKhoa;
                db.SaveChanges();
                TempData["add_success"] = "Sửa niên khóa thành công";
                return RedirectToAction("Index");
        }

        public JsonResult GetNienKhoaByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var nienKhoa = db.Tbl_NienKhoa.Find(ID);
            return Json(nienKhoa, JsonRequestBehavior.AllowGet);
        }

    }
}