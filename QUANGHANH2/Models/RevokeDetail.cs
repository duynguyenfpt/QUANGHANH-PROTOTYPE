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
    
    public partial class RevokeDetail
    {
        public int documentary_revoke_id { get; set; }
        public int equipment_revoke_status { get; set; }
        public int documentary_id { get; set; }
        public string equipmentId { get; set; }
        public string equipment_revoke_reason { get; set; }
        public string department_id_from { get; set; }
    
        public virtual Documentary Documentary { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Department Department { get; set; }
    }
}
