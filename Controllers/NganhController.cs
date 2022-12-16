using PagedList;
using QLDoAn.Models.DTO;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class NganhController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_ChuyenNganh.OrderByDescending(x => x.ChuyenNganh).ToPagedList(page, pageSize);
            ViewBag.Khoa = db.Tbl_Khoa.ToList();
            return View(model);
        }


        //Xóa 
        public JsonResult Delete(long ID)
        {

            try
            {
                var nganh = db.Tbl_ChuyenNganh.Find(ID);
                db.Tbl_ChuyenNganh.Remove(nganh);
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
        public ActionResult Add(Tbl_ChuyenNganh entity)
        {

            var dem = db.Tbl_ChuyenNganh.Count(x => x.ChuyenNganh == entity.ChuyenNganh);
            if (dem > 0)
            {
                TempData["add_success1"] = " Thêm thất bại ! chuyên ngành này đã tồn tại!";
                return RedirectToAction("Index");

            }
            else
            {
                db.Tbl_ChuyenNganh.Add(entity);
                db.SaveChanges();
                TempData["add_success"] = "Thêm chuyên ngành thành công";
                return RedirectToAction("Index");

            }
         

        }

    
        [HttpPost]
        public ActionResult Edit(Tbl_ChuyenNganh entity)
        {
            var prv = db.Tbl_ChuyenNganh.Find(entity.ID);
            prv.ChuyenNganh = entity.ChuyenNganh;
            prv.MaChuyenNganh = entity.MaChuyenNganh;
            prv.Khoa_ID = entity.Khoa_ID;

            var dem = db.Tbl_ChuyenNganh.Count(x => x.ChuyenNganh == prv.ChuyenNganh);
            if (dem > 0)
            {
                TempData["add_success1"] = " Sửa thất bại ! chuyên ngành này đã tồn tại!";
                return RedirectToAction("Index");

            }

            else
            {
                db.SaveChanges();
                TempData["add_success"] = "Sửa chuyên ngành thành công";
                return RedirectToAction("Index");

            }


          
        }

        public JsonResult GetNganhByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = from nganh in db.Tbl_ChuyenNganh
                        join khoa in db.Tbl_Khoa on nganh.Khoa_ID equals khoa.ID
                        where nganh.ID == ID
                        select new NganhDTO()
                        {
                            ID = nganh.ID,
                            ChuyenNganh = nganh.ChuyenNganh,
                            MaChuyenNganh = nganh.MaChuyenNganh,
                            Khoa_ID = nganh.Khoa_ID,
                            TenKhoa = khoa.TenKhoa
                        };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}