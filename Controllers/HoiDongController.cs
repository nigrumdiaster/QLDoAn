using PagedList;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class HoiDongController : Controller
    {
        // GET: HoiDong
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_HDCham.OrderBy(x => x.ID).ToPagedList(page, pageSize);
            return View(model);
        }


        //Xóa 
        public JsonResult Delete(long ID)
        {

            try
            {
                var hdc = db.Tbl_HDCham.Find(ID);
                foreach(var item in db.Tbl_HoiDong.Where(x => x.HDCham_ID == hdc.ID))
                {
                    db.Tbl_HoiDong.Remove(item);
                }
                db.Tbl_HDCham.Remove(hdc);
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

        public ActionResult Add()
        {
            ViewBag.Khoa = db.Tbl_Khoa.ToList();
            return View();
        }


        [HttpPost]
        public ActionResult addHoiDong(Tbl_HDCham entity)
        {

            db.Tbl_HDCham.Add(entity);
            db.SaveChanges();
            var gv = Session["add_gv"] as List<Tbl_GiaoVien>;
            foreach(var item in gv)
            {
                var hd = new Tbl_HoiDong();
                hd.GiaoVien_ID = item.ID;
                hd.HDCham_ID = db.Tbl_HDCham.Max(x => x.ID);
                db.Tbl_HoiDong.Add(hd);
                db.SaveChanges();
            }
            Session["add_gv"] = null;
            TempData["add_success"] = "Thêm hội đồng chấm thành công";
            return RedirectToAction("Index");

        }


        public JsonResult addGV(long ID)
        {
            var cart = Session["add_gv"];
            if (cart != null)//Nếu giỏ đã chứa
            {
                var list = (List<Tbl_GiaoVien>)cart;
                if (!list.Exists(x => x.ID == ID))
                {
                    //Tạo đối tượng mới
                    var item = db.Tbl_GiaoVien.Find(ID);
                    list.Add(item);
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else//nếu giỏ hàng trống
            {
                var item = db.Tbl_GiaoVien.Find(ID);
                var list = new List<Tbl_GiaoVien>();
                list.Add(item);

                Session["add_gv"] = list;
            }
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Delete_GV(long ID)
        {
            var cartSec = (List<Tbl_GiaoVien>)Session["add_gv"];
            cartSec.RemoveAll(x => x.ID == ID);
            Session["add_gv"] = cartSec;
            return Json(new
            {
                status = true
            });
        }

        //Xóa giáo viên trong hội đồng khi sửa
        public JsonResult DeleteHD(long ID)
        {

            try
            {
                var hd = db.Tbl_HoiDong.Find(ID);
                db.Tbl_HoiDong.Remove(hd);
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

        //Thêm giáo viên vào hội đồng khi sửa
        public JsonResult Add_GVHD(long GiaoVien_ID, long HDCham_ID)
        {

            try
            {
                var hd = new Tbl_HoiDong();
                hd.GiaoVien_ID = GiaoVien_ID;
                hd.HDCham_ID = HDCham_ID;
                db.Tbl_HoiDong.Add(hd);
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

        public ActionResult Edit(long ID)
        {
            ViewBag.HDC = db.Tbl_HDCham.Find(ID);
            ViewBag.Khoa = db.Tbl_Khoa.ToList();
            ViewBag.HD = db.Tbl_HoiDong.Where(x => x.HDCham_ID == ID).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult editHoiDong(Tbl_HDCham entity)
        {
            var prv = db.Tbl_HDCham.Find(entity.ID);
            prv.TenHoiDong = entity.TenHoiDong;
            db.SaveChanges();
            
            TempData["add_success"] = "Sửa hội đồng chấm thành công";
            return RedirectToAction("Index");
        }

        public JsonResult GethdByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var hd = db.Tbl_HoiDong.Find(ID);
            return Json(hd, JsonRequestBehavior.AllowGet);
        }
    }
}