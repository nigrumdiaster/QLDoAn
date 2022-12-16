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
    public class LopController : Controller
    {
        
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_Lop.OrderByDescending(x => x.TenLop).ToPagedList(page, pageSize);
            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
            ViewBag.HeDT = db.Tbl_HeDaoTao.ToList();
       
            return View(model);
        }


        //Xóa 
        public JsonResult Delete(long ID)
        {

            try
            {
                var lop = db.Tbl_Lop.Find(ID);
                db.Tbl_Lop.Remove(lop);
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
        public ActionResult Add(Tbl_Lop entity)
        {
            var dem = db.Tbl_Lop.Count(x => x.TenLop == entity.TenLop);

            if (dem > 0)
            {
                TempData["add_success1"] = " Thêm thất bại ! lớp này đã tồn tại!";
                return RedirectToAction("Index");

            }
            else
            {
                db.Tbl_Lop.Add(entity);
                db.SaveChanges();
                TempData["add_success"] = "Thêm lớp thành công";
                return RedirectToAction("Index");

            }
          

        }

        //public int idEdit = 0;
        

        [HttpPost]
        public ActionResult Edit(Tbl_Lop entity)
        {
            var prv = db.Tbl_Lop.Find(entity.ID);
            //idEdit = (int)prv.ID;
            prv.TenLop = entity.TenLop;
            prv.MaLop = entity.MaLop;
            prv.ChuyenNganh_ID = entity.ChuyenNganh_ID;
            prv.HeDaoTao_ID = entity.HeDaoTao_ID;
            db.SaveChanges();
            TempData["add_success"] = "Sửa lớp thành công";
            return RedirectToAction("Index");
        }

        public JsonResult GetByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = from lop in db.Tbl_Lop
                        join cn in db.Tbl_ChuyenNganh on lop.ChuyenNganh_ID equals cn.ID
                        join hedt in db.Tbl_HeDaoTao on lop.HeDaoTao_ID equals hedt.ID
                        where lop.ID == ID
                        select new LopDTO()
                        {
                            ID = lop.ID,
                            TenLop = lop.TenLop,
                            MaLop = lop.MaLop,
                            ChuyenNganh_ID = lop.ChuyenNganh_ID,
                            ChuyenNganh = cn.ChuyenNganh,
                            HeDaoTao_ID = lop.HeDaoTao_ID,
                            TenHe = hedt.TenHe
                        };
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //tìm kiếm lớp
        public JsonResult SearchLop(string q)
        {
            var query = db.Tbl_Lop.Where(x => x.TenLop.Contains(q)).Select(x => x.TenLop);
            return Json(new
            {
                data = query,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Kết quả tìm kiếm lớp
        public ActionResult Search(string TenLop = null, int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_Lop.Where(x => x.TenLop.Contains(TenLop)).OrderByDescending(x => x.TenLop).ToPagedList(page, pageSize);
            ViewBag.TenLop = TenLop;
            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
            ViewBag.HeDT = db.Tbl_HeDaoTao.ToList();
            return View(model);

        }
    }
}