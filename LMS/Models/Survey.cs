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
    
    public partial class Survey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Survey()
        {
            this.SurveyHeadings = new HashSet<SurveyHeading>();
            this.SurveyInitiateds = new HashSet<SurveyInitiated>();
            this.SurveyInitiatedResponses = new HashSet<SurveyInitiatedResponse>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; }
        public Nullable<int> isNotActiveBy { get; set; }
        public Nullable<System.DateTime> isNotActiveDate { get; set; }
        public Nullable<System.TimeSpan> isNotActiveTime { get; set; }
        public Nullable<int> SurveyTypeID { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
        public Nullable<System.TimeSpan> CrTime { get; set; }
        public Nullable<System.TimeSpan> UTime { get; set; }
    
        public virtual SurveyType SurveyType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyHeading> SurveyHeadings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyInitiated> SurveyInitiateds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyInitiatedResponse> SurveyInitiatedResponses { get; set; }
    }
}