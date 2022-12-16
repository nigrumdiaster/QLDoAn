using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLDoAn.Models.DTO
{
    public class GiaoVienDTO
    {
        public long ID { get; set; }
        public string TenGiaoVien { get; set; }
        public string MaGiaoVien { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public Nullable<long> ChuyenNganh_ID { get; set; }
        public string Email { get; set; }
        public string QueQuan { get; set; }
        public string SoDienThoai { get; set; }
        public string HinhAnh { get; set; }
        public Nullable<bool> GioiTinh { get; set; }

        public string ChuyenNganh { get; set; }
        public string TenDoAn { get; set; }
        public string HuongNghienCuu { get; set; }
        public long GVHD_ID { get; set; }
    }
}