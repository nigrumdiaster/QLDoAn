using PagedList;
using QLDoAn.Models.DTO;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_GiaoVien.OrderByDescending(x => x.MaGiaoVien).ToPagedList(page, pageSize);
            return View(model);
        }

        public ActionResult Add()
        {
            ViewBag.lstNganh = db.Tbl_ChuyenNganh.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddGiaoVien(Tbl_GiaoVien entity, HttpPostedFileBase HinhAnh)
        {
            var gv = new Tbl_GiaoVien();
            gv.TenGiaoVien = entity.TenGiaoVien;
            gv.MaGiaoVien = entity.MaGiaoVien;
            gv.Email = entity.Email;
            gv.SoDienThoai = entity.SoDienThoai;
            gv.QueQuan = entity.QueQuan;
            gv.GioiTinh = entity.GioiTinh;
            gv.NgaySinh = entity.NgaySinh;
            gv.ChuyenNganh_ID = entity.ChuyenNganh_ID;
            ViewBag.Thongbao =0;

            if (db.Tbl_GiaoVien.Count(x => x.MaGiaoVien == entity.MaGiaoVien) > 0)
            {
                TempData["add_success"] = "Thêm giáo viên thất bại, mã giáo viên bị trùng, vui lòng nhập lại.";
                return Redirect("/Teacher/Add");
            }

            if (HinhAnh == null)
            {
                //TempData["add_success"] = "Bạn chưa chọn ảnh đại diện cho giáo viên.";
                //return Redirect("/Teacher/Add");
                gv.HinhAnh = "default-avatar.png";
            }
            else
            {
                //Thêm hình ảnh
                var path = Path.Combine(Server.MapPath("~/Assets/img/teacher"), HinhAnh.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(HinhAnh.FileName);
                    string filename = HinhAnh.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("~/Assets/img/teacher/"), filename);
                    HinhAnh.SaveAs(path);
                    gv.HinhAnh = filename;
                }
                else
                {
                    HinhAnh.SaveAs(path);
                    gv.HinhAnh = HinhAnh.FileName;
                }
            }

            db.Tbl_GiaoVien.Add(gv);
            db.SaveChanges();
            TempData["add_success"] = "Thêm giáo viên thành công.";
            return Redirect("/Teacher");
        }

        public ActionResult Edit(long ID)
        {
            ViewBag.GiaoVien = db.Tbl_GiaoVien.Find(ID);
            ViewBag.lstNganh = db.Tbl_ChuyenNganh.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EditGiaoVien(Tbl_GiaoVien entity, HttpPostedFileBase HinhAnh)
        {

            var gv = db.Tbl_GiaoVien.Find(entity.ID);
            gv.TenGiaoVien = entity.TenGiaoVien;
            gv.MaGiaoVien = entity.MaGiaoVien;
            gv.Email = entity.Email;
            gv.SoDienThoai = entity.SoDienThoai;
            gv.QueQuan = entity.QueQuan;
            gv.GioiTinh = entity.GioiTinh;
            gv.NgaySinh = entity.NgaySinh;
            gv.ChuyenNganh_ID = entity.ChuyenNganh_ID;

            if (HinhAnh != null && gv.HinhAnh != HinhAnh.FileName)
            {
                //Xóa file cũ
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/img/teacher"), gv.HinhAnh));
                //Thêm hình ảnh
                var path = Path.Combine(Server.MapPath("~/Assets/img/teacher/"), HinhAnh.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(HinhAnh.FileName);
                    string filename = HinhAnh.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("~/Assets/img/teacher/"), filename);
                    HinhAnh.SaveAs(path);
                    gv.HinhAnh = filename;
                }
                else
                {
                    HinhAnh.SaveAs(path);
                    gv.HinhAnh = HinhAnh.FileName;
                }
            }
          
            db.SaveChanges();
            TempData["add_success"] = "Sửa giáo viên thành công.";
            return Redirect("/Teacher");
        }


        public JsonResult Delete(long ID)
        {
            try
            {
                var gv = db.Tbl_GiaoVien.Find(ID);
                db.Tbl_GiaoVien.Remove(gv);
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
       // tìm kiếm giáo viên
        public ActionResult Search(string tengiaovien = "", int page = 1, int pageSize = 5)
        {
            if(tengiaovien!="")
            {
                var model = db.Tbl_GiaoVien.Where(x => x.TenGiaoVien.Contains(tengiaovien)).ToList();
                ViewBag.TenGiaoVien = tengiaovien;
                return View(model.OrderBy(x => x.MaGiaoVien).ToPagedList(page, pageSize));

            }
            else
            {
                   return Redirect("/Teacher");
            }
           
           





        }

    }
}