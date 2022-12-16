using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLDoAn.Models.DTO
{
    public class LopDTO
    {
        public long ID { get; set; }
        public string TenLop { get; set; }
        public string MaLop { get; set; }
        public Nullable<long> ChuyenNganh_ID { get; set; }
        public Nullable<long> HeDaoTao_ID { get; set; }
        public Nullable<int> SiSo { get; set; }

        public string ChuyenNganh { get; set; }
        public string TenHe { get; set; }
    }
}