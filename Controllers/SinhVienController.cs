using ClosedXML.Excel;
using PagedList;
using QLDoAn.Models.DTO;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Controllers
{
    public class SinhVienController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities(); 
        // GET: SinhVien
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_SinhVien.OrderByDescending(x => x.MaSinhVien).ToPagedList(page, pageSize);
            ViewBag.lstLop = db.Tbl_Lop.ToList();
            return View(model);
        }

        public ActionResult Add()
        {
            ViewBag.lstLop = db.Tbl_Lop.ToList();
            ViewBag.lstNganh = db.Tbl_ChuyenNganh.ToList();
            ViewBag.lstHeDT = db.Tbl_HeDaoTao.ToList();
            ViewBag.lstNienKhoa = db.Tbl_NienKhoa.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddSinhVien(Tbl_SinhVien entity)
        {
            var dem = db.Tbl_SinhVien.Count(x => x.MaSinhVien == entity.MaSinhVien);
            if (dem > 0)
            {
                TempData["add_success"] = " Thêm thất bại ! Mã sinh viên đã tồn tại!";
                return Redirect("/SinhVien/Add");

            }
           

            db.Tbl_SinhVien.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Thêm sinh viên thành công.";
            return Redirect("/SinhVien");



        }

        public ActionResult Edit(long ID)
        {
            ViewBag.sinhvien = db.Tbl_SinhVien.Find(ID);
            ViewBag.lstLop = db.Tbl_Lop.ToList();
            ViewBag.lstNganh = db.Tbl_ChuyenNganh.ToList();
            ViewBag.lstHeDT = db.Tbl_HeDaoTao.ToList();
            ViewBag.lstNienKhoa = db.Tbl_NienKhoa.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EditSinhVien(Tbl_SinhVien entity)
        {
            var sv = db.Tbl_SinhVien.Find(entity.ID);
            sv.TenSinhVien = entity.TenSinhVien;
            sv.MaSinhVien = entity.MaSinhVien;
            sv.Email = entity.Email;
            sv.SoDienThoai = entity.SoDienThoai;
            sv.QueQuan = entity.QueQuan;
            sv.GioiTinh = entity.GioiTinh;
            sv.NgaySinh = entity.NgaySinh;
            sv.Lop_ID = entity.Lop_ID;
            sv.ChuyenNganh_ID = entity.ChuyenNganh_ID;
            sv.HeDaoTao_ID = entity.HeDaoTao_ID;
            sv.NienKhoa_ID = entity.NienKhoa_ID;
            db.SaveChanges();
            TempData["add_success"] = "Sửa sinh viên thành công.";
            return Redirect("/SinhVien");
        }


        public JsonResult Delete(long ID)
        {
            try
            {
                var sv = db.Tbl_SinhVien.Find(ID);
                db.Tbl_SinhVien.Remove(sv);
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

        public JsonResult GetChuyenNganhByLop(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = from lop in db.Tbl_Lop
                        join cn in db.Tbl_ChuyenNganh on lop.ChuyenNganh_ID equals cn.ID
                        join he in db.Tbl_HeDaoTao on lop.HeDaoTao_ID equals he.ID
                        select new SinhVienDTO()
                        {
                            HeDaoTao_ID = lop.HeDaoTao_ID,
                            TenHe = he.TenHe,
                            ChuyenNganh_ID = lop.ChuyenNganh_ID,
                            ChuyenNganh = cn.ChuyenNganh
                        };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //Kết quả tìm kiếm sinh viên và tên lớp
        public ActionResult Search(string MaSinhVien = "", long Lop_ID =0 , int page = 1, int pageSize = 5)
        {
            ViewBag.lstLop = db.Tbl_Lop.ToList();
            if (MaSinhVien.Trim() !="" && Lop_ID > 0)
            {
                var model = from sv in db.Tbl_SinhVien
                            join lop in db.Tbl_Lop on sv.Lop_ID equals lop.ID
                            where sv.MaSinhVien.Contains(MaSinhVien.TrimEnd()) && lop.ID == Lop_ID
                            select new SinhVienDTO()
                            {
                                ID = sv.ID,
                                TenSinhVien = sv.TenSinhVien,
                                MaSinhVien = sv.MaSinhVien,
                                NgaySinh = sv.NgaySinh,
                                Email = sv.Email,
                                QueQuan = sv.QueQuan,
                                SoDienThoai = sv.SoDienThoai,
                                GioiTinh = sv.GioiTinh,
                                ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh,
                                TenHe = sv.Tbl_HeDaoTao.TenHe,
                                TenLop = sv.Tbl_Lop.TenLop,
                                NienKhoa = sv.Tbl_NienKhoa.NienKhoa,
                            };
                ViewBag.MaSinhVien = MaSinhVien.Trim();
                ViewBag.TenLop = db.Tbl_Lop.Find(Lop_ID);
                return View(model.OrderByDescending(x => x.TenSinhVien).ToPagedList(page, pageSize));
            }
            else if (Lop_ID > 0 && MaSinhVien.Trim() == "")
            {
                var model = from sv in db.Tbl_SinhVien
                            join lop in db.Tbl_Lop on sv.Lop_ID equals lop.ID
                            where lop.ID == Lop_ID
                            select new SinhVienDTO()
                            {
                                ID = sv.ID,
                                TenSinhVien = sv.TenSinhVien,
                                MaSinhVien = sv.MaSinhVien,
                                NgaySinh = sv.NgaySinh,
                                Email = sv.Email,
                                QueQuan = sv.QueQuan,
                                SoDienThoai = sv.SoDienThoai,
                                GioiTinh = sv.GioiTinh,
                                ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh,
                                TenHe = sv.Tbl_HeDaoTao.TenHe,
                                TenLop = sv.Tbl_Lop.TenLop,
                                NienKhoa = sv.Tbl_NienKhoa.NienKhoa,
                            };
                ViewBag.TenLop = db.Tbl_Lop.Find(Lop_ID);
                return View(model.OrderByDescending(x => x.TenSinhVien).ToPagedList(page, pageSize));



              
            }

             else if(MaSinhVien.Trim() != "" && Lop_ID == 0)
            {
                var model = from sv in db.Tbl_SinhVien
                            join lop in db.Tbl_Lop on sv.Lop_ID equals lop.ID
                            where sv.MaSinhVien.Contains(MaSinhVien.TrimEnd())
                            select new SinhVienDTO()
                            {
                                ID = sv.ID,
                                TenSinhVien = sv.TenSinhVien,
                                MaSinhVien = sv.MaSinhVien,
                                NgaySinh = sv.NgaySinh,
                                Email = sv.Email,
                                QueQuan = sv.QueQuan,
                                SoDienThoai = sv.SoDienThoai,
                                GioiTinh = sv.GioiTinh,
                                ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh,
                                TenHe = sv.Tbl_HeDaoTao.TenHe,
                                TenLop = sv.Tbl_Lop.TenLop,
                                NienKhoa = sv.Tbl_NienKhoa.NienKhoa,
                            };
                ViewBag.MaSinhVien = MaSinhVien;
                return View(model.OrderByDescending(x => x.TenSinhVien).ToPagedList(page, pageSize));
               
            }
            else
            

            
            {
                return RedirectToAction("Index", "SinhVien");


            }
         

        }

        //Kết quả tìm kiếm sinh viên và tên lớp
        public ActionResult Filter_Diem(long NamBaoVe_ID = 0, int page = 1, int pageSize = 10)
        {
         
            ViewBag.NamBaoVe = db.Tbl_NamBaoVe.ToList();
            if (NamBaoVe_ID != 0)
            {
                var model = from tg in db.Tbl_ThamGia
                            join doan in db.Tbl_DoAn on tg.DoAn_ID equals doan.ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                            where tg.NamBaoVe_ID == NamBaoVe_ID
                            select new ThamGiaDTO()
                            {
                                ID = tg.ID,
                                TenSinhVien = sv.TenSinhVien,
                                TenDoAn = doan.TenDoAn,
                                Diem = tg.Diem,
                                ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh
                            };
                ViewBag.NamBV = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
                return View(model.OrderByDescending(x => x.TenSinhVien).ToPagedList(page, pageSize));
            }
            else
            {
                var model = from tg in db.Tbl_ThamGia
                            join doan in db.Tbl_DoAn on tg.DoAn_ID equals doan.ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                            select new ThamGiaDTO()
                            {
                                ID = tg.ID,
                                TenSinhVien = sv.TenSinhVien,
                                TenDoAn = doan.TenDoAn,
                                Diem = tg.Diem,
                                ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh
                            };
                return View(model.OrderByDescending(x => x.TenSinhVien).ToPagedList(page, pageSize));
            }

        }


        public ActionResult Diem(int page = 1, int pageSize = 10)
        {
            var model = (from tg in db.Tbl_ThamGia
                         join doan in db.Tbl_DoAn on tg.DoAn_ID equals doan.ID
                         join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                         select new ThamGiaDTO()
                         {
                             ID = tg.ID,
                             TenSinhVien = sv.TenSinhVien,
                             TenDoAn = doan.TenDoAn,
                             Diem = tg.Diem,
                             ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh
                         }).ToList();
            //ViewBag.lstLop = db.Tbl_Lop.ToList();
            //ViewBag.lstNienKhoa = db.Tbl_NienKhoa.ToList();
            ViewBag.NamBaoVe = db.Tbl_NamBaoVe.ToList();
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public FileResult Export(long NamBaoVe_ID = 0)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("STT"),
                                            new DataColumn("Tên sinh viên"),
                                            new DataColumn("Tên đồ án"),
                                            new DataColumn("Điểm"),
                                            new DataColumn("Chuyên ngành") });

            IQueryable<ThamGiaDTO> model;

            if (NamBaoVe_ID != 0)
            {
                model = from tg in db.Tbl_ThamGia
                            join doan in db.Tbl_DoAn on tg.DoAn_ID equals doan.ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                            where tg.NamBaoVe_ID == NamBaoVe_ID
                            select new ThamGiaDTO()
                            {
                                ID = tg.ID,
                                TenSinhVien = sv.TenSinhVien,
                                TenDoAn = doan.TenDoAn,
                                Diem = tg.Diem,
                                ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh
                            };
            }
            else
            {
                model = from tg in db.Tbl_ThamGia
                            join doan in db.Tbl_DoAn on tg.DoAn_ID equals doan.ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                            select new ThamGiaDTO()
                            {
                                ID = tg.ID,
                                TenSinhVien = sv.TenSinhVien,
                                TenDoAn = doan.TenDoAn,
                                Diem = tg.Diem,
                                ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh
                            };
            }

            int dem = 1;
            foreach (var item in model)
            {
                dt.Rows.Add(dem, item.TenSinhVien, item.TenDoAn, item.Diem, item.ChuyenNganh);
                dem++;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Diem-sinh-vien.xlsx");
                }
            }
        }

}
}