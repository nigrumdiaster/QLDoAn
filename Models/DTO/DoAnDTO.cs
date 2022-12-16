using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLDoAn.Models.DTO
{
    public class DoAnDTO
    {
        public long ID { get; set; }
        public string TenDoAn { get; set; }
        public string MaDoAn { get; set; }
        public string LinkSource { get; set; }
        public string LinkDocument { get; set; }
        public Nullable<long> ChuyenNganh_ID { get; set; }
        public Nullable<long> GiaoVien_ID { get; set; }
        public Nullable<long> HDCham_ID { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }

        public string ChuyenNganh { get; set; }
        public string GVHD { get; set; }
        public string HDC { get; set; }
    }
}