using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLDoAn.Models.DTO
{
    public class SinhVienDTO
    {
        public long ID { get; set; }
        public string TenSinhVien { get; set; }
        public string MaSinhVien { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public Nullable<long> ChuyenNganh_ID { get; set; }
        public Nullable<long> Lop_ID { get; set; }
        public Nullable<long> HeDaoTao_ID { get; set; }
        public Nullable<long> NienKhoa_ID { get; set; }
        public string Email { get; set; }
        public string QueQuan { get; set; }
        public string SoDienThoai { get; set; }
        public Nullable<bool> GioiTinh { get; set; }

        public string ChuyenNganh { get; set; }
        public string TenHe { get; set; }
        public string TenLop { get; set; }
        public string NienKhoa { get; set; }
    }
}