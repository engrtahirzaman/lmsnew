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
    
    public partial class AssessmentDetail
    {
        public int ID { get; set; }
        public Nullable<int> CampusID { get; set; }
        public int AssessmentsID { get; set; }
        public int StudentID { get; set; }
        public decimal ObtainedMarks { get; set; }
        public bool IsAbsent { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<System.TimeSpan> CrTime { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
        public Nullable<System.TimeSpan> UTime { get; set; }
    
        public virtual Assessment Assessment { get; set; }
        public virtual Student Student { get; set; }
    }
}
