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
    
    public partial class Transfer
    {
        public int decision_id { get; set; }
        public string employee_id { get; set; }
        public string transfer_reason { get; set; }
        public string new_department_id { get; set; }
        public Nullable<int> new_work_id { get; set; }
        public string old_department_id { get; set; }
        public Nullable<int> old_work_id { get; set; }
        public Nullable<int> new_salary_id { get; set; }
        public Nullable<int> old_salary_id { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Department Department1 { get; set; }
        public virtual Decision Decision { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Salary Salary { get; set; }
        public virtual Salary Salary1 { get; set; }
        public virtual Work Work { get; set; }
        public virtual Work Work1 { get; set; }
    }
}
