//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLDoAn.Models.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_ChuyenNganh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_ChuyenNganh()
        {
            this.Tbl_DoAn = new HashSet<Tbl_DoAn>();
            this.Tbl_GiaoVien = new HashSet<Tbl_GiaoVien>();
            this.Tbl_SinhVien = new HashSet<Tbl_SinhVien>();
        }
    
        public long ID { get; set; }
        public string ChuyenNganh { get; set; }
        public string MaChuyenNganh { get; set; }
        public Nullable<long> Khoa_ID { get; set; }
        public string Mota { get; set; }
    
        public virtual Tbl_Khoa Tbl_Khoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DoAn> Tbl_DoAn { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_GiaoVien> Tbl_GiaoVien { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_SinhVien> Tbl_SinhVien { get; set; }
    }
}
