//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QUANGHANH2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TieuChi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TieuChi()
        {
            this.KeHoach_TieuChi = new HashSet<KeHoach_TieuChi>();
            this.ThucHien_TieuChi = new HashSet<ThucHien_TieuChi>();
            this.TieuChi_VatLieuSanXuat = new HashSet<TieuChi_VatLieuSanXuat>();
        }
    
        public int MaTieuChi { get; set; }
        public string TenTieuChi { get; set; }
        public string DonViDo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KeHoach_TieuChi> KeHoach_TieuChi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThucHien_TieuChi> ThucHien_TieuChi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TieuChi_VatLieuSanXuat> TieuChi_VatLieuSanXuat { get; set; }
    }
}
