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
    
    public partial class AdmissionEducationDetail
    {
        public int ID { get; set; }
        public int AdmissionID { get; set; }
        public int AdmissionEducationID { get; set; }
        public int EducationLevelID { get; set; }
        public int MajorSubjectID { get; set; }
        public Nullable<int> MajorSubjectListOfCoursesID { get; set; }
        public string CourseTitle { get; set; }
        public string CourseStudied { get; set; }
        public Nullable<int> ObtainMarks { get; set; }
        public Nullable<int> TotalMarks { get; set; }
        public Nullable<bool> IsVerify { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
    
        public virtual Admission Admission { get; set; }
        public virtual AdmissionEducation AdmissionEducation { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        public virtual MajorSubject MajorSubject { get; set; }
    }
}
