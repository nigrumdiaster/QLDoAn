using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLDoAn.Models.DTO
{
    public class ThamGiaDTO
    {
        public long ID { get; set; }
        public Nullable<long> DoAn_ID { get; set; }
        public Nullable<long> SinhVien_ID { get; set; }
        public Nullable<int> Diem { get; set; }

        public string TenSinhVien { get; set; }
        public string TenDoAn { get; set; }
        public string ChuyenNganh { get; set; }
        public int? DotBaoVe { get; set; }
        public int? NamBaoVe { get; set; }

    }
}