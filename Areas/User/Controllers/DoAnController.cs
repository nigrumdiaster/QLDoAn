using PagedList;
using QLDoAn.Models.DTO;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDoAn.Areas.User.Controllers
{
    public class DoAnController : Controller
    {
        // GET: User/DoAn
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_DoAn.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
            ViewBag.lstLop = db.Tbl_Lop.ToList();
            ViewBag.listNamBaoVe = db.Tbl_NamBaoVe.ToList();
            return View(model);
        }


        public ActionResult GetPDF(long ID)
        {
            var doc = db.Tbl_DoAn.Find(ID);
            string filePath = "~/Assets/document/" + doc.LinkDocument;
            Response.AddHeader("Content-Disposition", "inline; filename=DataBindEvalFormat.pdf");

            return File(filePath, "application/pdf");
        }
        public ActionResult Detail(long ID)
        {
            ViewBag.DoAn = db.Tbl_DoAn.Find(ID);

            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
            ViewBag.Khoa = db.Tbl_Khoa.ToList();


            ViewBag.ThamGia = db.Tbl_ThamGia.Where(x => x.DoAn_ID == ID).ToList();
            var model = (from gvhd in db.Tbl_GVHD
                         join gv in db.Tbl_GiaoVien on gvhd.GiaoVien_ID equals gv.ID
                         join da in db.Tbl_DoAn on gvhd.DoAn_ID equals da.ID
                         where gvhd.DoAn_ID == ID
                         select new GiaoVienDTO()
                         {
                             TenGiaoVien = gv.TenGiaoVien,
                             ChuyenNganh = gv.Tbl_ChuyenNganh.ChuyenNganh,
                             ID = gv.ID
                         }).Distinct();
            ViewBag.GVHD = model.ToList();
            ViewBag.HuongNC = db.Tbl_GVHD.Where(x => x.DoAn_ID == ID).ToList();
            var query = from hd in db.Tbl_HoiDong
                        join hdc in db.Tbl_HDCham on hd.HDCham_ID equals hdc.ID
                        join doan in db.Tbl_DoAn on hdc.ID equals doan.HDCham_ID
                        where doan.ID == ID
                        select new GiaoVienDTO()
                        {
                            TenGiaoVien = hd.Tbl_GiaoVien.TenGiaoVien,
                            ChuyenNganh = hd.Tbl_GiaoVien.Tbl_ChuyenNganh.ChuyenNganh
                        };
            ViewBag.HDC = query.ToList();
            return View();
        }

        //Download source code
        public ActionResult DownloadSource(long ID)
        {
            var doan = db.Tbl_DoAn.Find(ID);
            string fullName = Server.MapPath("~/Assets/source/" + doan.LinkSource);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, doan.LinkSource);
        }

        //Download báo cáo
        public ActionResult DownloadDocument(long ID)
        {
            var doan = db.Tbl_DoAn.Find(ID);
            string fullName = Server.MapPath("~/Assets/document/" + doan.LinkDocument);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, doan.LinkDocument);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        public ActionResult Search(string TenDoAn = "", long NamBaoVe_ID = 0, string MaSinhVien = "", int page = 1, int pageSize = 1)
        {

            ViewBag.lstLop = db.Tbl_Lop.ToList();
            ViewBag.lstSV = db.Tbl_SinhVien.ToList();
            ViewBag.listNamBaoVe = db.Tbl_NamBaoVe.ToList();
            ViewBag.lstGV = db.Tbl_GiaoVien.ToList();
            ViewBag.lstHDC = db.Tbl_HDCham.ToList();
            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();




            //rule 1

            if (TenDoAn.Trim() != "" && NamBaoVe_ID != 0 && MaSinhVien.Trim() != "")
            {
                var model = from da in db.Tbl_DoAn
                            join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                            join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID

                            where tg.NamBaoVe_ID == NamBaoVe_ID && da.TenDoAn.Contains(TenDoAn) && sv.MaSinhVien.Contains(MaSinhVien.Trim())
                            select new DoAnDTO()
                            {
                                ID = da.ID,
                                TenDoAn = da.TenDoAn,
                                MaDoAn = da.MaDoAn,
                                LinkDocument = da.LinkDocument,
                                LinkSource = da.LinkSource,
                                NgayTao = da.NgayTao,
                                ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
                            };
                ViewBag.TenDoAn = TenDoAn;
                ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
                ViewBag.MaSinhVien = MaSinhVien.Trim();

                return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            }



            //rule 3

            else if (MaSinhVien.Trim() != "" && NamBaoVe_ID == 0 && TenDoAn.Trim() == "")

            {
                var model = from da in db.Tbl_DoAn
                            join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                            join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID

                            where sv.MaSinhVien.Contains(MaSinhVien.Trim())
                            select new DoAnDTO()
                            {
                                ID = da.ID,
                                TenDoAn = da.TenDoAn,
                                MaDoAn = da.MaDoAn,
                                LinkDocument = da.LinkDocument,
                                LinkSource = da.LinkSource,
                                NgayTao = da.NgayTao,
                                ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
                            };
                ViewBag.TenDoAn = TenDoAn;
                ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
                ViewBag.MaSinhVien = MaSinhVien.Trim();

                return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            }
            //rule 4

            else if (TenDoAn.Trim() != "" && NamBaoVe_ID != 0 && MaSinhVien.Trim() == "")
            {
                var model = from da in db.Tbl_DoAn
                            join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                            join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID

                            where tg.NamBaoVe_ID == NamBaoVe_ID && da.TenDoAn.Contains(TenDoAn)
                            select new DoAnDTO()
                            {
                                ID = da.ID,
                                TenDoAn = da.TenDoAn,
                                MaDoAn = da.MaDoAn,
                                LinkDocument = da.LinkDocument,
                                LinkSource = da.LinkSource,
                                NgayTao = da.NgayTao,
                                ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
                            };
                ViewBag.TenDoAn = TenDoAn;
                ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
                ViewBag.MaSinhVien = MaSinhVien.Trim();

                return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            }


            //rule 5
            else if (TenDoAn.Trim() == "" && NamBaoVe_ID != 0 && MaSinhVien.Trim() == "")
            {
                var model = from da in db.Tbl_DoAn
                            join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
                            join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID
                            where tg.NamBaoVe_ID == NamBaoVe_ID
                            select new DoAnDTO()
                            {
                                ID = da.ID,
                                TenDoAn = da.TenDoAn,
                                MaDoAn = da.MaDoAn,
                                LinkDocument = da.LinkDocument,
                                LinkSource = da.LinkSource,
                                NgayTao = da.NgayTao,
                                ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
                            };
                ViewBag.TenDoAn = TenDoAn;
                ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
                ViewBag.MaSinhVien = MaSinhVien.Trim();

                return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            }


            //rule 1

            else if (TenDoAn.Trim() != "" && NamBaoVe_ID == 0 && MaSinhVien.Trim() != "")
            {
                var model = from da in db.Tbl_DoAn
                            join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID


                            where da.TenDoAn.Contains(TenDoAn) && sv.MaSinhVien.Contains(MaSinhVien.Trim())
                            select new DoAnDTO()
                            {
                                ID = da.ID,
                                TenDoAn = da.TenDoAn,
                                MaDoAn = da.MaDoAn,
                                LinkDocument = da.LinkDocument,
                                LinkSource = da.LinkSource,
                                NgayTao = da.NgayTao,
                                ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
                            };
                ViewBag.TenDoAn = TenDoAn;
                ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
                ViewBag.MaSinhVien = MaSinhVien.Trim();

                return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            }


            //rule 6

            else if (TenDoAn.Trim() != "" && NamBaoVe_ID == 0 && MaSinhVien.Trim() == "")
            {
                var model = from da in db.Tbl_DoAn
                            join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID

                            where da.TenDoAn.Contains(TenDoAn.Trim())
                            select new DoAnDTO()
                            {
                                ID = da.ID,
                                TenDoAn = da.TenDoAn,
                                MaDoAn = da.MaDoAn,
                                LinkDocument = da.LinkDocument,
                                LinkSource = da.LinkSource,
                                NgayTao = da.NgayTao,
                                ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
                            };
                ViewBag.TenDoAn = TenDoAn;
                ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
                ViewBag.MaSinhVien = MaSinhVien.Trim();

                return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            }

            //rule 2
            else if (TenDoAn.Trim() != "" && NamBaoVe_ID != 0 && MaSinhVien.Trim() == "")
            {
                var model = from da in db.Tbl_DoAn
                            join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
                            join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                            join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID

                            where da.TenDoAn.Contains(TenDoAn.Trim()) && tg.NamBaoVe_ID == NamBaoVe_ID
                            select new DoAnDTO()
                            {
                                ID = da.ID,
                                TenDoAn = da.TenDoAn,
                                MaDoAn = da.MaDoAn,
                                LinkDocument = da.LinkDocument,
                                LinkSource = da.LinkSource,
                                NgayTao = da.NgayTao,
                                ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
                            };
                ViewBag.TenDoAn = TenDoAn;
                ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
                ViewBag.MaSinhVien = MaSinhVien.Trim();

                return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            }
            //rule 8

            else
            {
                return RedirectToAction("Index", "DoAn");
            }






















            //if (TenDoAn.Trim() != "" && NamBaoVe_ID != 0)
            //{
            //    var model = from da in db.Tbl_DoAn
            //                join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
            //                join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
            //                //join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID

            //                where tg.NamBaoVe_ID == NamBaoVe_ID && da.TenDoAn.Contains(TenDoAn) /*&& sv.MaSinhVien.Contains(MaSinhVien.Trim())*/
            //                select new DoAnDTO()
            //                {
            //                    ID = da.ID,
            //                    TenDoAn = da.TenDoAn,
            //                    MaDoAn = da.MaDoAn,
            //                    LinkDocument = da.LinkDocument,
            //                    LinkSource = da.LinkSource,
            //                    NgayTao = da.NgayTao,
            //                    ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
            //                };
            //    ViewBag.TenDoAn = TenDoAn;
            //    ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);
            //    //ViewBag.MaSinhVien = MaSinhVien.Trim();

            //    return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            //}    
            //else if(TenDoAn.Trim() !="")
            //{
            //    var model = from da in db.Tbl_DoAn


            //                where  da.TenDoAn.Contains(TenDoAn)
            //                select new DoAnDTO()
            //                {
            //                    ID = da.ID,
            //                    TenDoAn = da.TenDoAn,

            //                    MaDoAn = da.MaDoAn,
            //                    LinkDocument = da.LinkDocument,
            //                    LinkSource = da.LinkSource,
            //                    NgayTao = da.NgayTao,
            //                    ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
            //                };
            //    ViewBag.TenDoAn = TenDoAn;


            //    return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));

            //}
            //else if(NamBaoVe_ID != 0)
            //{
            //    var model = from da in db.Tbl_DoAn
            //                join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
            //                join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID

            //                where tg.NamBaoVe_ID == NamBaoVe_ID 
            //                select new DoAnDTO()
            //                {
            //                    ID = da.ID,
            //                    TenDoAn = da.TenDoAn,

            //                    MaDoAn = da.MaDoAn,
            //                    LinkDocument = da.LinkDocument,
            //                    LinkSource = da.LinkSource,
            //                    NgayTao = da.NgayTao,
            //                    ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
            //                };
            //    ViewBag.TenDoAn = TenDoAn;
            //    ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);

            //    return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
            //}
            //else
            //{
            //  return  RedirectToAction("Index", "DoAn");
            //}    




        }

        //Kết quả tìm kiếm đồ án và tên lớp
        //public ActionResult Search(string TenDoAn = null, long Lop_ID = 0, int page = 1, int pageSize = 5)
        //{
        //    ViewBag.lstLop = db.Tbl_Lop.ToList();
        //    ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
        //    if (TenDoAn != null && Lop_ID != 0)
        //    {
        //        var model = from cn in db.Tbl_ChuyenNganh
        //                    join doan in db.Tbl_DoAn on cn.ID equals doan.ChuyenNganh_ID
        //                    join lop in db.Tbl_Lop on cn.ID equals lop.ChuyenNganh_ID
        //                    where doan.TenDoAn.Contains(TenDoAn) && lop.ID == Lop_ID
        //                    select new DoAnDTO()
        //                    {
        //                        ID = doan.ID,
        //                        TenDoAn = doan.TenDoAn,
        //                        MaDoAn = doan.MaDoAn,
        //                        LinkDocument = doan.LinkDocument,
        //                        LinkSource = doan.LinkSource,
        //                        NgayTao = doan.NgayTao,
        //                        ChuyenNganh = doan.Tbl_ChuyenNganh.ChuyenNganh
        //                    };
        //        ViewBag.TenDoAn = TenDoAn;
        //        ViewBag.TenLop = db.Tbl_Lop.Find(Lop_ID);
        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
        //    }
        //    else if (TenDoAn != null)
        //    {
        //        var model = from cn in db.Tbl_ChuyenNganh
        //                    join doan in db.Tbl_DoAn on cn.ID equals doan.ChuyenNganh_ID
        //                    where doan.TenDoAn.Contains(TenDoAn)
        //                    select new DoAnDTO()
        //                    {
        //                        ID = doan.ID,
        //                        TenDoAn = doan.TenDoAn,
        //                        MaDoAn = doan.MaDoAn,
        //                        LinkDocument = doan.LinkDocument,
        //                        LinkSource = doan.LinkSource,
        //                        NgayTao = doan.NgayTao,
        //                        ChuyenNganh = doan.Tbl_ChuyenNganh.ChuyenNganh
        //                    };
        //        ViewBag.TenDoAn = TenDoAn;
        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
        //    }
        //    else
        //    {
        //        var model = from cn in db.Tbl_ChuyenNganh
        //                    join doan in db.Tbl_DoAn on cn.ID equals doan.ChuyenNganh_ID
        //                    join lop in db.Tbl_Lop on cn.ID equals lop.ChuyenNganh_ID
        //                    where lop.ID == Lop_ID
        //                    select new DoAnDTO()
        //                    {
        //                        ID = doan.ID,
        //                        TenDoAn = doan.TenDoAn,
        //                        MaDoAn = doan.MaDoAn,
        //                        LinkDocument = doan.LinkDocument,
        //                        LinkSource = doan.LinkSource,
        //                        NgayTao = doan.NgayTao,
        //                        ChuyenNganh = doan.Tbl_ChuyenNganh.ChuyenNganh
        //                    };
        //        ViewBag.TenLop = db.Tbl_Lop.Find(Lop_ID);
        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
        //    }

        //}
        //public ActionResult Search(string TenDoAn = null, long NamBaoVe_ID = 0, int page = 1, int pageSize = 5)
        //{
        //    ViewBag.lstLop = db.Tbl_Lop.ToList();
        //    ViewBag.listNamBaoVe = db.Tbl_NamBaoVe.ToList();

        //    ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
        //    if (TenDoAn != null && NamBaoVe_ID != 0)
        //    {
        //        var model = from da in db.Tbl_DoAn
        //                    join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
        //                    join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID

        //                    where tg.NamBaoVe_ID == NamBaoVe_ID && da.TenDoAn.Contains(TenDoAn)
        //                    select new DoAnDTO()
        //                    {
        //                        ID = da.ID,
        //                        TenDoAn = da.TenDoAn,

        //                        MaDoAn = da.MaDoAn,
        //                        LinkDocument = da.LinkDocument,
        //                        LinkSource = da.LinkSource,
        //                        NgayTao = da.NgayTao,
        //                        ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
        //                    };
        //        ViewBag.TenDoAn = TenDoAn;
        //        ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);

        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
        //    }
        //    else if (TenDoAn != null)
        //    {
        //        var model = from da in db.Tbl_DoAn


        //                    where da.TenDoAn.Contains(TenDoAn)
        //                    select new DoAnDTO()
        //                    {
        //                        ID = da.ID,
        //                        TenDoAn = da.TenDoAn,

        //                        MaDoAn = da.MaDoAn,
        //                        LinkDocument = da.LinkDocument,
        //                        LinkSource = da.LinkSource,
        //                        NgayTao = da.NgayTao,
        //                        ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
        //                    };
        //        ViewBag.TenDoAn = TenDoAn;


        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));


        //    }
        //    else
        //    {
        //        var model = from da in db.Tbl_DoAn
        //                    join tg in db.Tbl_ThamGia on da.ID equals tg.DoAn_ID
        //                    join nam in db.Tbl_NamBaoVe on tg.NamBaoVe_ID equals nam.ID

        //                    where tg.NamBaoVe_ID == NamBaoVe_ID
        //                    select new DoAnDTO()
        //                    {
        //                        ID = da.ID,
        //                        TenDoAn = da.TenDoAn,

        //                        MaDoAn = da.MaDoAn,
        //                        LinkDocument = da.LinkDocument,
        //                        LinkSource = da.LinkSource,
        //                        NgayTao = da.NgayTao,
        //                        ChuyenNganh = da.Tbl_ChuyenNganh.ChuyenNganh
        //                    };
        //        ViewBag.TenDoAn = TenDoAn;
        //        ViewBag.NamBaoVe = db.Tbl_NamBaoVe.Find(NamBaoVe_ID);

        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
        //    }



    }
}
