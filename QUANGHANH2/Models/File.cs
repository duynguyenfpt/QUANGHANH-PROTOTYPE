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
    
    public partial class File
    {
        public int file_id { get; set; }
        public Nullable<int> report_file_id { get; set; }
        public string file_name { get; set; }
        public string file_name_display { get; set; }
        public Nullable<int> account_id { get; set; }
        public Nullable<System.DateTime> uploaded_time { get; set; }
        public string note { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual ReportFile ReportFile { get; set; }
    }
}
