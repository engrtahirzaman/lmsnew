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
    
    public partial class MeritListMarksCriteria
    {
        public int ID { get; set; }
        public int SessionID { get; set; }
        public int DepartmentProgramID { get; set; }
        public int EducationLevelID { get; set; }
        public decimal Score { get; set; }
        public int CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public System.DateTime UDate { get; set; }
    
        public virtual DepartmentProgram DepartmentProgram { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        public virtual Session Session { get; set; }
    }
}