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
    
    public partial class MajorSubjectListOfCours
    {
        public int ID { get; set; }
        public Nullable<int> MajorSubjectID { get; set; }
        public string SubjectTitle { get; set; }
        public int CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public int UBy { get; set; }
        public System.DateTime UDate { get; set; }
    
        public virtual MajorSubject MajorSubject { get; set; }
    }
}