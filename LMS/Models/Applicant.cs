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
    
    public partial class Applicant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Applicant()
        {
            this.Admissions = new HashSet<Admission>();
            this.ApplicantEducations = new HashSet<ApplicantEducation>();
            this.ApplicantEducationDetails = new HashSet<ApplicantEducationDetail>();
            this.ApplicantPreferences = new HashSet<ApplicantPreference>();
            this.TestApplicants = new HashSet<TestApplicant>();
            this.TestVouchers = new HashSet<TestVoucher>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CampusID { get; set; }
        public string PicturePath { get; set; }
        public byte[] Photo { get; set; }
        public int TitleID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherCNIC { get; set; }
        public string FatherCNICCopy { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherOrganizationName { get; set; }
        public string FatherIncome { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string MobileNo { get; set; }
        public string GuardianMobileNo { get; set; }
        public string LandLineNo { get; set; }
        public string CNICNo { get; set; }
        public string CNICCopy { get; set; }
        public string PassportNo { get; set; }
        public string Gender { get; set; }
        public Nullable<int> NationalityID { get; set; }
        public Nullable<int> ReligionID { get; set; }
        public Nullable<int> ProvinceID { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public Nullable<int> DomicileID { get; set; }
        public Nullable<int> SessionID { get; set; }
        public string BloodGroup { get; set; }
        public Nullable<bool> IsDisable { get; set; }
        public string DescribeDisability { get; set; }
        public string SponsoredBy { get; set; }
        public Nullable<int> CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Admission> Admissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicantEducation> ApplicantEducations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicantEducationDetail> ApplicantEducationDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicantPreference> ApplicantPreferences { get; set; }
        public virtual District District { get; set; }
        public virtual Nationality Nationality { get; set; }
        public virtual Province Province { get; set; }
        public virtual Religion Religion { get; set; }
        public virtual Session Session { get; set; }
        public virtual Title Title { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestApplicant> TestApplicants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestVoucher> TestVouchers { get; set; }
    }
}
