using PagedList;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class NamBaoVeController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        // GET: NamBaoVe
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_NamBaoVe.OrderByDescending(x => x.NamBaoVe).ToPagedList(page, pageSize);
            return View(model);
           
        }
        public JsonResult Delete(long ID)
        {

            try
            {
                var NamBV = db.Tbl_NamBaoVe.Find(ID);
                db.Tbl_NamBaoVe.Remove(NamBV);
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
        public ActionResult Add(Tbl_NamBaoVe entity)
        {
            var dem = db.Tbl_NamBaoVe.Count(x => x.NamBaoVe == entity.NamBaoVe && x.DotBaoVe==entity.DotBaoVe);
            if (dem > 0)
            {
                TempData["add_success1"] = " Thêm thất bại ! đợt bảo vệ này đã tồn tại!";
                return Redirect("/NamBaoVe");

            }


            db.Tbl_NamBaoVe.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Thêm đợt bảo vệ thành công.";
            return Redirect("/NamBaoVe");





        }

        [HttpPost]
        public ActionResult Edit(Tbl_NamBaoVe entity)
        {
            var prv = db.Tbl_NamBaoVe.Find(entity.ID);
            prv.NamBaoVe = entity.NamBaoVe;
            prv.DotBaoVe = entity.DotBaoVe;

            var dem = db.Tbl_NamBaoVe.Count(x => x.NamBaoVe == prv.NamBaoVe && x.DotBaoVe == prv.DotBaoVe);
            if (dem > 0)
            {
                TempData["add_success1"] = " Sửa thất bại ! đợt bảo vệ này đã tồn tại!";
                return RedirectToAction("Index");

            }
            else
            {
                db.SaveChanges();
                TempData["add_success"] = "Sửa đợt bảo vệ thành công";
                return RedirectToAction("Index");

            }



            
        }

        public JsonResult GetByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var hedt = db.Tbl_NamBaoVe.Find(ID);
            return Json(hedt, JsonRequestBehavior.AllowGet);
        }
    }
}