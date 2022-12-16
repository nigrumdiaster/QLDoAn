using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLDoAn.Models.DTO
{
    public class NganhDTO
    {
        public long ID { get; set; }
        public string ChuyenNganh { get; set; }
        public string MaChuyenNganh { get; set; }
        public Nullable<long> Khoa_ID { get; set; }
        public string Mota { get; set; }

        public string TenKhoa { get; set; }
    }
}