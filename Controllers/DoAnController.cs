using PagedList;
using QLDoAn.Models.DTO;
using QLDoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace QLDoAn.Controllers
{
    public class DoAnController : Controller
    {
        private QuanLyDoAnEntities db = new QuanLyDoAnEntities();
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Tbl_DoAn.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
            ViewBag.lstLop = db.Tbl_Lop.ToList();
            ViewBag.lstGV = db.Tbl_GiaoVien.ToList();
            ViewBag.lstHDC = db.Tbl_HDCham.ToList();
            ViewBag.NamBV = db.Tbl_NamBaoVe.ToList();
            ViewBag.listNamBaoVe = db.Tbl_NamBaoVe.ToList();
            ViewBag.sinhvien = db.Tbl_SinhVien.ToList();


            return View(model);
        }


        //Xóa 
        public JsonResult Delete(long ID, HttpPostedFileBase LinkSource, HttpPostedFileBase LinkDocument)
        {

            try
            {
                var doan = db.Tbl_DoAn.Find(ID);
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/source"), doan.LinkSource));
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/document"), doan.LinkDocument));

                var thamgia = db.Tbl_ThamGia.SingleOrDefault(x => x.DoAn_ID == ID);
                if(thamgia!=null)
                {
                    db.Tbl_ThamGia.Remove(thamgia);

                }    
               

                //var thamgia = db.Tbl_ThamGia.FirstOrDefault(x => x.DoAn_ID == ID);
                //var gvhd = db.Tbl_GVHD.FirstOrDefault(x => x.DoAn_ID == ID);
                //var hd = db.Tbl_HoiDong.FirstOrDefault(x => x.DoAn_ID == ID);
                db.Tbl_DoAn.Remove(doan);
                //db.Tbl_ThamGia.Remove(thamgia);
                //db.Tbl_GVHD.Remove(gvhd);
                //db.Tbl_HoiDong.Remove(hd);
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
        public ActionResult Add(Tbl_DoAn entity, HttpPostedFileBase LinkSource, HttpPostedFileBase LinkDocument, long sinhVien_ID=0, long namBaoVe_ID=0 ,int Diem=0)
        {
            var doan = new Tbl_DoAn();
            doan.TenDoAn = entity.TenDoAn;

            doan.MaDoAn = "";

            doan.NgayTao = DateTime.Now;
            doan.ChuyenNganh_ID = entity.ChuyenNganh_ID;

            doan.GiaoVien_ID = entity.GiaoVien_ID;
            doan.ChuyenNganh_ID = entity.ChuyenNganh_ID;

            doan.HDCham_ID = entity.HDCham_ID;

            if (LinkSource != null)
            {
                var path_sou = Path.Combine(Server.MapPath("~/Assets/source"), LinkSource.FileName);
                if (System.IO.File.Exists(path_sou))
                {
                    string extensionName_sou = Path.GetExtension(LinkSource.FileName);
                    string filename_sou = LinkSource.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName_sou;
                    path_sou = Path.Combine(Server.MapPath("~/Assets/source"), filename_sou);
                    LinkSource.SaveAs(path_sou);
                    doan.LinkSource = filename_sou;
                }
                else
                {
                    LinkSource.SaveAs(path_sou);
                    doan.LinkSource = LinkSource.FileName;
                }
            }

            if (LinkDocument != null)
            {
                var path_doc = Path.Combine(Server.MapPath("~/Assets/document"), LinkDocument.FileName);
                if (System.IO.File.Exists(path_doc))
                {
                    string extensionName_doc = Path.GetExtension(LinkDocument.FileName);
                    string filename_doc = LinkDocument.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName_doc;
                    path_doc = Path.Combine(Server.MapPath("~/Assets/document"), filename_doc);
                    LinkDocument.SaveAs(path_doc);
                    doan.LinkDocument = filename_doc;
                }
                else
                {
                    LinkDocument.SaveAs(path_doc);
                    doan.LinkDocument = LinkDocument.FileName;
                }
            }


            db.Tbl_DoAn.Add(doan);
            db.SaveChanges();



            //ViewBag.DoAn = db.Tbl_DoAn.Find(ID);

            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
            ViewBag.Khoa = db.Tbl_Khoa.ToList();
            ViewBag.NamBV = db.Tbl_NamBaoVe.ToList();
            if(sinhVien_ID!=0)
            {
                Console.WriteLine(sinhVien_ID);
                    
                var thamgia = new Tbl_ThamGia();
                thamgia.DoAn_ID = doan.ID;
                thamgia.SinhVien_ID = sinhVien_ID;
                thamgia.NamBaoVe_ID = namBaoVe_ID;
                thamgia.Diem = 0;
                if (Diem>0)
                {
                    thamgia.Diem = Diem;

                }
                
              
                db.Tbl_ThamGia.Add(thamgia);
                db.SaveChanges();

            }



            TempData["add_success"] = "Thêm đồ án thành công";
            return RedirectToAction("Index");





            //ViewBag.GVPB = db.Tbl_HoiDong.Where(x => x.DoAn_ID == ID && x.Loai == 2).ToList();









        }



      

        [HttpPost]
        public ActionResult Edit(Tbl_DoAn entity, HttpPostedFileBase LinkSource, HttpPostedFileBase LinkDocument)
        {
            var prv = db.Tbl_DoAn.Find(entity.ID);


            
            prv.TenDoAn = entity.TenDoAn.Trim();
            prv.MaDoAn = "";
            prv.ChuyenNganh_ID = entity.ChuyenNganh_ID;
            prv.GiaoVien_ID = entity.GiaoVien_ID;
            prv.HDCham_ID = entity.HDCham_ID;

            if (LinkSource != null && prv.LinkSource != LinkSource.FileName)
            {
                if(prv.LinkSource != null)
                {
                    //Xóa file cũ
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/source"), prv.LinkSource));
                }

                //Thêm file
                var path = Path.Combine(Server.MapPath("~/Assets/source"), LinkSource.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName_sou = Path.GetExtension(LinkSource.FileName);
                    string filename_sou = LinkSource.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName_sou;
                    path = Path.Combine(Server.MapPath("~/Assets/source"), filename_sou);
                    LinkSource.SaveAs(path);
                    prv.LinkSource = filename_sou;
                }
                else
                {
                    LinkSource.SaveAs(path);
                    prv.LinkSource = LinkSource.FileName;
                }
            }

            if (LinkDocument != null && prv.LinkDocument != LinkDocument.FileName)
            {
                if (prv.LinkDocument != null)
                {
                    //Xóa file cũ
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/document"), prv.LinkDocument));
                }

                //Thêm file
                var path = Path.Combine(Server.MapPath("~/Assets/document"), LinkDocument.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName_doc = Path.GetExtension(LinkSource.FileName);
                    string filename_doc = LinkSource.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName_doc;
                    path = Path.Combine(Server.MapPath("~/Assets/document"), filename_doc);
                    LinkDocument.SaveAs(path);
                    prv.LinkDocument = filename_doc;
                }
                else
                {
                    LinkDocument.SaveAs(path);
                    prv.LinkDocument = LinkDocument.FileName;
                }
            }

            var a = db.Tbl_ThamGia.FirstOrDefault(x => x.DoAn_ID == entity.ID);

         
          

            var b = new Tbl_ThamGia();
            b.NamBaoVe_ID = Convert.ToInt64(Request.Form["NamBaoVe_ID"]);
            b.SinhVien_ID = Convert.ToInt64(Request.Form["SinhVien_ID"]);
           
            b.DoAn_ID = entity.ID;
            b.Diem = Convert.ToInt32(Request.Form["Diem"]);
          


            if(b.SinhVien_ID==null)
            {
                TempData["add_success"] = "Sửa đồ án không thành công";
                return RedirectToAction("Index");

            }
            else
            {
              
                db.Tbl_ThamGia.Remove(a);
                db.SaveChanges();
                db.Tbl_ThamGia.Add(b);
              

              
              
                db.SaveChanges();
                TempData["add_success"] = "Sửa đồ án thành công";




            }
            return RedirectToAction("Index");







        }

        //Download source code
        public ActionResult DownloadSource(long ID)
        {
            var doan = db.Tbl_DoAn.Find(ID);
            if (doan.LinkSource == null)
            {
                TempData["add_success"] = "Source code đồ án chưa được cập nhật.";
                return RedirectToAction("Index");
            }
            else
            {
                string fullName = Server.MapPath("~/Assets/source/" + doan.LinkSource);

                byte[] fileBytes = GetFile(fullName);
                return File(
                    fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, doan.LinkSource);
            }

        }

        //Download báo cáo
        public ActionResult DownloadDocument(long ID)
        {
            var doan = db.Tbl_DoAn.Find(ID);
            if (doan.LinkDocument == null)
            {
                TempData["add_success"] = "Báo cáo đồ án chưa được cập nhật.";
                return RedirectToAction("Index");
            }
            else
            {
                string fullName = Server.MapPath("~/Assets/document/" + doan.LinkDocument);

                byte[] fileBytes = GetFile(fullName);
                return File(
                    fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, doan.LinkDocument);
            }

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

        //Kết quả tìm kiếm đồ án và tên lớp
        //public ActionResult Search(string TenDoAn = null, long Lop_ID = 0, int page = 1, int pageSize = 5)
        //{
        //    ViewBag.lstLop = db.Tbl_Lop.ToList();

        //    ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();
        //    if (TenDoAn != null && Lop_ID != 0)
        //    {
        //        var model = from cn in db.Tbl_ChuyenNganh
        //                     join doan in db.Tbl_DoAn on cn.ID equals doan.ChuyenNganh_ID
        //                     join lop in db.Tbl_Lop on cn.ID equals lop.ChuyenNganh_ID
        //                     where doan.TenDoAn.Contains(TenDoAn) && lop.ID == Lop_ID
        //                     select new DoAnDTO()
        //                     {
        //                         ID = doan.ID,
        //                         TenDoAn = doan.TenDoAn,
        //                         MaDoAn = doan.MaDoAn,
        //                         LinkDocument = doan.LinkDocument,
        //                         LinkSource = doan.LinkSource,
        //                         NgayTao = doan.NgayTao,
        //                         ChuyenNganh = doan.Tbl_ChuyenNganh.ChuyenNganh
        //                     };
        //        ViewBag.TenDoAn = TenDoAn;
        //        ViewBag.TenLop = db.Tbl_Lop.Find(Lop_ID);
        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
        //    }
        //    else if (TenDoAn != null)
        //    {
        //        var model = from cn in db.Tbl_ChuyenNganh
        //                    join doan in db.Tbl_DoAn on cn.ID equals doan.ChuyenNganh_ID
        //                     where doan.TenDoAn.Contains(TenDoAn)
        //                     select new DoAnDTO()
        //                     {
        //                         ID = doan.ID,
        //                         TenDoAn = doan.TenDoAn,
        //                         MaDoAn = doan.MaDoAn,
        //                         LinkDocument = doan.LinkDocument,
        //                         LinkSource = doan.LinkSource,
        //                         NgayTao = doan.NgayTao,
        //                         ChuyenNganh = doan.Tbl_ChuyenNganh.ChuyenNganh
        //                     };
        //        ViewBag.TenDoAn = TenDoAn;
        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
        //    }
        //    else
        //    {
        //        var model = from cn in db.Tbl_ChuyenNganh
        //                    join doan in db.Tbl_DoAn on cn.ID equals doan.ChuyenNganh_ID
        //                    join lop in db.Tbl_Lop on cn.ID equals lop.ChuyenNganh_ID
        //                    where lop.ID == Lop_ID
        //                     select new DoAnDTO()
        //                     {
        //                         ID = doan.ID,
        //                         TenDoAn = doan.TenDoAn,
        //                         MaDoAn = doan.MaDoAn,
        //                         LinkDocument = doan.LinkDocument,
        //                         LinkSource = doan.LinkSource,
        //                         NgayTao = doan.NgayTao,
        //                         ChuyenNganh = doan.Tbl_ChuyenNganh.ChuyenNganh
        //                     };
        //        ViewBag.TenLop = db.Tbl_Lop.Find(Lop_ID);
        //        return View(model.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize));
        //    }

        //}
        public ActionResult Search(string TenDoAn = "",long NamBaoVe_ID = 0, string MaSinhVien = "", int page = 1, int pageSize = 1)
        {
           
            ViewBag.lstLop = db.Tbl_Lop.ToList();
            ViewBag.lstSV = db.Tbl_SinhVien.ToList();
            ViewBag.listNamBaoVe = db.Tbl_NamBaoVe.ToList();
            ViewBag.lstGV = db.Tbl_GiaoVien.ToList();
            ViewBag.lstHDC = db.Tbl_HDCham.ToList();
            ViewBag.ChuyenNganh = db.Tbl_ChuyenNganh.ToList();




            //rule 7

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

        else if (TenDoAn.Trim() != "" && NamBaoVe_ID == 0 && MaSinhVien.Trim() !="")
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

          else  if (TenDoAn.Trim() != "" && NamBaoVe_ID == 0 && MaSinhVien.Trim() == "")
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
          else  if (TenDoAn.Trim() != "" && NamBaoVe_ID != 0 && MaSinhVien.Trim() == "")
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



    


        public JsonResult GetByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = from doan in db.Tbl_DoAn
                        join cn in db.Tbl_ChuyenNganh on doan.ChuyenNganh_ID equals cn.ID
                        where doan.ID == ID
                        select new DoAnDTO()
                        {
                            ID = doan.ID,
                            ChuyenNganh = cn.ChuyenNganh,
                            GiaoVien_ID = doan.GiaoVien_ID,
                            GVHD = doan.Tbl_GiaoVien.TenGiaoVien,
                            HDCham_ID = doan.HDCham_ID,
                            HDC = doan.Tbl_HDCham.TenHoiDong,
                            MaDoAn = doan.MaDoAn,
                            TenDoAn = doan.TenDoAn,
                            LinkDocument = doan.LinkDocument,
                            LinkSource = doan.LinkSource
                        };
            return Json(model, JsonRequestBehavior.AllowGet);
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
            ViewBag.NamBV = db.Tbl_NamBaoVe.ToList();

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
            //ViewBag.GVPB = db.Tbl_HoiDong.Where(x => x.DoAn_ID == ID && x.Loai == 2).ToList();
            return View();
        }

        //Sinh viên tham gia
        public JsonResult GetLopByChuyenNganh(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = db.Tbl_Lop.Where(x => x.ChuyenNganh_ID == ID).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSVByLop(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = db.Tbl_SinhVien.Where(x => x.Lop_ID == ID).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }






        public JsonResult AddSVTG(long doAn_ID, long sinhVien_ID, long namBaoVe_ID)
        {

            var svth = db.Tbl_ThamGia.SingleOrDefault(x=>x.DoAn_ID==doAn_ID);
            if(svth!=null)
            {
                TempData["add_success"] = "Một đồ án chỉ đc thực hiện một sinh viên";


            }
            else
            {
                var thamgia = new Tbl_ThamGia();
                thamgia.DoAn_ID = doAn_ID;
                thamgia.SinhVien_ID = sinhVien_ID;
                thamgia.NamBaoVe_ID = namBaoVe_ID;
                thamgia.Diem = 0;
                db.Tbl_ThamGia.Add(thamgia);
                db.SaveChanges();

            }
         
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_SVTG(long ID)
        {
            var thamgia = db.Tbl_ThamGia.Find(ID);
           
            db.Tbl_ThamGia.Remove(thamgia);
            
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        //Giáo viên hướng dẫn
        public JsonResult GetChuyenNganhByKhoa(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = db.Tbl_ChuyenNganh.Where(x => x.Khoa_ID == ID).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }



        public JsonResult lopbydoan(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var thamgia = db.Tbl_ThamGia.FirstOrDefault(x => x.DoAn_ID == ID);


            var a = thamgia.SinhVien_ID;
            var model = db.Tbl_SinhVien.FirstOrDefault(x => x.ID == thamgia.SinhVien_ID);
            var model1 = db.Tbl_Lop.Where(x => x.ID == model.Lop_ID).ToList();
            return Json(model1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Sinhvienbydoan1(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var thamgia = db.Tbl_ThamGia.FirstOrDefault(x => x.DoAn_ID == ID);



            var model = db.Tbl_SinhVien.Where(x => x.ID == thamgia.SinhVien_ID).ToList();
        
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult sinhvienbydoan(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = db.Tbl_ThamGia.Where(x =>x.DoAn_ID == ID).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getChuyenNganhByDoAn(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var lstthamgia = db.Tbl_ThamGia.ToList();
            var thamgia = db.Tbl_ThamGia.FirstOrDefault(x => x.DoAn_ID == ID);
            var model = db.Tbl_SinhVien.FirstOrDefault(x => x.ID == thamgia.SinhVien_ID);
            var model1 = db.Tbl_ChuyenNganh.Where(x => x.ID == model.ChuyenNganh_ID).ToList();
            
            
            return Json(model1, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetGVByChuyenNganh(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = db.Tbl_GiaoVien.Where(x => x.ChuyenNganh_ID == ID).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddGVHD(long doAn_ID, long giaoVien_ID)
        {
            var gvhd = new Tbl_GVHD();
            gvhd.DoAn_ID = doAn_ID;
            gvhd.GiaoVien_ID = giaoVien_ID;
            gvhd.HuongNghienCuu = "Xét duyệt đồ án";
            gvhd.NgayTao = DateTime.Now;
            db.Tbl_GVHD.Add(gvhd);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddHuongNC(long doAn_ID, long giaoVien_ID, string huongnc)
        {
            var gvhd = new Tbl_GVHD();
            gvhd.DoAn_ID = doAn_ID;
            gvhd.GiaoVien_ID = giaoVien_ID;
            gvhd.HuongNghienCuu = huongnc;
            gvhd.NgayTao = DateTime.Now;
            db.Tbl_GVHD.Add(gvhd);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_GVHD(long ID)
        {
            var gvhd = db.Tbl_GVHD.Find(ID);

            db.Tbl_GVHD.Remove(gvhd);

            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }


        //Điểm sinh viên
        public JsonResult GetSinhVienByDoAnID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var model = (from tg in db.Tbl_ThamGia
                        join doan in db.Tbl_DoAn on tg.DoAn_ID equals doan.ID
                        join sv in db.Tbl_SinhVien on tg.SinhVien_ID equals sv.ID
                        where tg.ID == ID
                        select new ThamGiaDTO()
                        {
                            ID = tg.ID,
                            TenSinhVien = sv.TenSinhVien,
                            Diem = tg.Diem
                        }).ToList();
            
            return Json(new
            {
                model
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditDiem(long ID, int diem)
        {
            var tg = db.Tbl_ThamGia.Find(ID);
            tg.Diem = diem;
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}