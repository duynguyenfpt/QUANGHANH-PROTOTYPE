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
    
    public partial class Acceptance
    {
        public int acceptance_id { get; set; }
        public int equipmentStatus { get; set; }
        public Nullable<System.DateTime> acceptance_date { get; set; }
        public int documentary_id { get; set; }
        public string equipmentId { get; set; }
        public string attach_to { get; set; }
    
        public virtual Equipment Equipment { get; set; }
        public virtual Documentary Documentary { get; set; }
        public virtual Equipment Equipment1 { get; set; }
    }
}
