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
    
    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            this.TestApplicants = new HashSet<TestApplicant>();
            this.TestCalenders = new HashSet<TestCalender>();
            this.TestVouchers = new HashSet<TestVoucher>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CampusID { get; set; }
        public int TestSessionID { get; set; }
        public string Title { get; set; }
        public string MajorSubjects { get; set; }
        public string EligibilityPrograms { get; set; }
        public string SubjectsPercentage { get; set; }
        public Nullable<int> Amount { get; set; }
        public string SampleLink { get; set; }
        public Nullable<int> CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
        public Nullable<int> TotalMarks { get; set; }
    
        public virtual Campus Campus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestApplicant> TestApplicants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestCalender> TestCalenders { get; set; }
        public virtual TestSession TestSession { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestVoucher> TestVouchers { get; set; }
    }
}
