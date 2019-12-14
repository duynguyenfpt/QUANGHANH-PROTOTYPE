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
    
    public partial class Documentary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documentary()
        {
            this.Acceptances = new HashSet<Acceptance>();
            this.Camera_Acceptance = new HashSet<Camera_Acceptance>();
            this.Documentary_camera_repair_details = new HashSet<Documentary_camera_repair_details>();
            this.Documentary_Improve_Detail = new HashSet<Documentary_Improve_Detail>();
            this.Documentary_big_maintain_details = new HashSet<Documentary_big_maintain_details>();
            this.Documentary_liquidation_details = new HashSet<Documentary_liquidation_details>();
            this.Documentary_maintain_details = new HashSet<Documentary_maintain_details>();
            this.Documentary_moveline_details = new HashSet<Documentary_moveline_details>();
            this.Documentary_repair_details = new HashSet<Documentary_repair_details>();
            this.Documentary_revoke_details = new HashSet<Documentary_revoke_details>();
            this.Supply_Documentary_Camera = new HashSet<Supply_Documentary_Camera>();
            this.Supply_Documentary_Equipment = new HashSet<Supply_Documentary_Equipment>();
            this.NhanViens = new HashSet<NhanVien>();
        }
    
        public int documentary_id { get; set; }
        public string documentary_code { get; set; }
        public int documentary_type { get; set; }
        public string department_id_to { get; set; }
        public System.DateTime date_created { get; set; }
        public string person_created { get; set; }
        public string reason { get; set; }
        public string out_in_come { get; set; }
        public int documentary_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acceptance> Acceptances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Camera_Acceptance> Camera_Acceptance { get; set; }
        public virtual DocumentaryType DocumentaryType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentary_camera_repair_details> Documentary_camera_repair_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentary_Improve_Detail> Documentary_Improve_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentary_big_maintain_details> Documentary_big_maintain_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentary_liquidation_details> Documentary_liquidation_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentary_maintain_details> Documentary_maintain_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentary_moveline_details> Documentary_moveline_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentary_repair_details> Documentary_repair_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentary_revoke_details> Documentary_revoke_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Supply_Documentary_Camera> Supply_Documentary_Camera { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Supply_Documentary_Equipment> Supply_Documentary_Equipment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
