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
    
    public partial class BatchWiseCours
    {
        public int ID { get; set; }
        public Nullable<int> CampusID { get; set; }
        public int BatchID { get; set; }
        public Nullable<int> SessionID { get; set; }
        public int SemesterNo { get; set; }
        public int CourseID { get; set; }
        public int CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public int UBy { get; set; }
        public System.DateTime UDate { get; set; }
    
        public virtual Session Session { get; set; }
        public virtual Campus Campus { get; set; }
        public virtual Course Course { get; set; }
        public virtual ProgramOffered ProgramOffered { get; set; }
    }
}
