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
    
    public partial class MajorSubject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MajorSubject()
        {
            this.AdmissionEducations = new HashSet<AdmissionEducation>();
            this.AdmissionEducationDetails = new HashSet<AdmissionEducationDetail>();
            this.ApplicantEducations = new HashSet<ApplicantEducation>();
            this.ApplicantEducationDetails = new HashSet<ApplicantEducationDetail>();
            this.MajorSubjectListOfCourses = new HashSet<MajorSubjectListOfCours>();
            this.ProgramPreferences = new HashSet<ProgramPreference>();
        }
    
        public int ID { get; set; }
        public string Subject { get; set; }
        public int EducationLevel_ID { get; set; }
        public Nullable<int> CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdmissionEducation> AdmissionEducations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdmissionEducationDetail> AdmissionEducationDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicantEducation> ApplicantEducations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicantEducationDetail> ApplicantEducationDetails { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MajorSubjectListOfCours> MajorSubjectListOfCourses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProgramPreference> ProgramPreferences { get; set; }
    }
}