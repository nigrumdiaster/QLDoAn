using QLDoAn.Models.DTO;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Areas.User.Controllers
{
    public class SearchController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        //tìm kiếm đồ án
        public JsonResult SearchDoAn(string q)
        {
            var query = db.Tbl_DoAn.Where(x => x.TenDoAn.Contains(q)).Select(x => x.TenDoAn);
            return Json(new
            {
                data = query,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //tìm kiếm giảng viên hướng dẫn
        public JsonResult SearchGVHD(string q)
        {
            var query = db.Tbl_GiaoVien.Where(x => x.TenGiaoVien.Contains(q)).Select(x => x.TenGiaoVien);
            return Json(new
            {
                data = query,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //tìm kiếm sinh viên
        public JsonResult SearchSinhVien(string q)
        {
            var query = db.Tbl_SinhVien.Where(x => x.TenSinhVien.Contains(q)).Select(x => x.TenSinhVien);
            return Json(new
            {
                data = query,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Kết quả tìm kiếm đồ án và gvhd
        public JsonResult Result_DoAnGVHD(string tendoan = null, string giaovien = null)
        {
            db.Configuration.ProxyCreationEnabled = false;

            if (tendoan != null && giaovien != null)
            {
                var model = (from gvhd in db.Tbl_GVHD
                             join doan in db.Tbl_DoAn on gvhd.DoAn_ID equals doan.ID
                             join gv in db.Tbl_GiaoVien on gvhd.GiaoVien_ID equals gv.ID
                             where doan.TenDoAn.Contains(tendoan) && gv.TenGiaoVien.Contains(giaovien)
                             select new DoAnDTO()
                             {
                                 ID = doan.ID,
                                 TenDoAn = doan.TenDoAn,
                                 MaDoAn = doan.MaDoAn,
                                 GVHD = gv.TenGiaoVien,
                                 ChuyenNganh = gv.Tbl_ChuyenNganh.ChuyenNganh
                             }).Distinct();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else if (tendoan != null)
            {
                var model = (from gvhd in db.Tbl_GVHD
                             join doan in db.Tbl_DoAn on gvhd.DoAn_ID equals doan.ID
                             join gv in db.Tbl_GiaoVien on gvhd.GiaoVien_ID equals gv.ID
                             where doan.TenDoAn.Contains(tendoan)
                             select new DoAnDTO()
                             {
                                 ID = doan.ID,
                                 TenDoAn = doan.TenDoAn,
                                 MaDoAn = doan.MaDoAn,
                                 GVHD = gv.TenGiaoVien,
                                 ChuyenNganh = gv.Tbl_ChuyenNganh.ChuyenNganh
                             }).Distinct();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = (from gvhd in db.Tbl_GVHD
                             join doan in db.Tbl_DoAn on gvhd.DoAn_ID equals doan.ID
                             join gv in db.Tbl_GiaoVien on gvhd.GiaoVien_ID equals gv.ID
                             where gv.TenGiaoVien.Contains(giaovien)
                             select new DoAnDTO()
                             {
                                 ID = doan.ID,
                                 TenDoAn = doan.TenDoAn,
                                 MaDoAn = doan.MaDoAn,
                                 GVHD = gv.TenGiaoVien,
                                 ChuyenNganh = gv.Tbl_ChuyenNganh.ChuyenNganh
                             }).Distinct();
                return Json(model, JsonRequestBehavior.AllowGet);
            }


        }

        //Kết quả tìm kiếm giảng viên
        public JsonResult Result_GVHD(string giaovien)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var model = from gvhd in db.Tbl_GVHD
                        join doan in db.Tbl_DoAn on gvhd.DoAn_ID equals doan.ID
                        join gv in db.Tbl_GiaoVien on gvhd.GiaoVien_ID equals gv.ID
                        where gv.TenGiaoVien.Contains(giaovien)
                        select new GiaoVienDTO()
                        {
                            TenGiaoVien = gv.TenGiaoVien,
                            TenDoAn = doan.TenDoAn,
                            ChuyenNganh = gv.Tbl_ChuyenNganh.ChuyenNganh,
                            HuongNghienCuu = gvhd.HuongNghienCuu
                        };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //Kết quả tìm kiếm sinh viên
        public JsonResult Result_SinhVien(string tensinhvien)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var model = from thamgia in db.Tbl_ThamGia
                        join doan in db.Tbl_DoAn on thamgia.DoAn_ID equals doan.ID
                        join sv in db.Tbl_SinhVien on thamgia.SinhVien_ID equals sv.ID
                        where sv.TenSinhVien.Contains(tensinhvien)
                        select new ThamGiaDTO()
                        {
                            TenSinhVien = sv.TenSinhVien,
                            TenDoAn = doan.TenDoAn,
                            ChuyenNganh = sv.Tbl_ChuyenNganh.ChuyenNganh
                        };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}