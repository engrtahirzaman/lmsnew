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
    
    public partial class EmpExperience
    {
        public int ID { get; set; }
        public int FormID { get; set; }
        public string Organization { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Designation { get; set; }
        public Nullable<int> Salary { get; set; }
        public string ReasonLeaving { get; set; }
        public int CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public int UBy { get; set; }
        public System.DateTime UDate { get; set; }
    
        public virtual EmpForm EmpForm { get; set; }
    }
}
