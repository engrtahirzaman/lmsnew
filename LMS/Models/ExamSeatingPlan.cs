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
    
    public partial class ExamSeatingPlan
    {
        public int ID { get; set; }
        public int ExamDateSheetID { get; set; }
        public int LocationID { get; set; }
        public int StudentID { get; set; }
        public int CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public System.TimeSpan CrTime { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
        public Nullable<System.TimeSpan> UTime { get; set; }
    
        public virtual ExamDateSheet ExamDateSheet { get; set; }
        public virtual Location Location { get; set; }
        public virtual Student Student { get; set; }
    }
}