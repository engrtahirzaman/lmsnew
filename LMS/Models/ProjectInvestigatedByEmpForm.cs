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
    
    public partial class ProjectInvestigatedByEmpForm
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int EmpFormID { get; set; }
        public string Comments { get; set; }
        public Nullable<int> ProjectInvestigationTypeID { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<System.TimeSpan> CrTime { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
        public Nullable<System.TimeSpan> Utime { get; set; }
    
        public virtual EmpForm EmpForm { get; set; }
        public virtual Project Project { get; set; }
        public virtual ProjectInvestigationType ProjectInvestigationType { get; set; }
    }
}