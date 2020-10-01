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
    
    public partial class Specialization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Specialization()
        {
            this.RecordsPapers = new HashSet<RecordsPaper>();
            this.Recruitments = new HashSet<Recruitment>();
        }
    
        public int specializations_id { get; set; }
        public string specializations_number { get; set; }
        public string name { get; set; }
        public string career_id { get; set; }
        public Nullable<int> education_level_id { get; set; }
    
        public virtual Career Career { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecordsPaper> RecordsPapers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recruitment> Recruitments { get; set; }
    }
}
