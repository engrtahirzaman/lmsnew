//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Grading
    {
        public int ID { get; set; }
        public string GradeLetter { get; set; }
        public Nullable<decimal> GradePoint { get; set; }
        public string GradeInterpretion { get; set; }
        public string Description { get; set; }
        public int GradingPolicyID { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
    
        public virtual GradingPolicy GradingPolicy { get; set; }
    }
}
