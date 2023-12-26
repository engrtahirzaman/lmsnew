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
    
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            this.ExamSeatingPlans = new HashSet<ExamSeatingPlan>();
        }
    
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int Slots { get; set; }
        public int CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public System.TimeSpan CrTime { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
        public Nullable<System.TimeSpan> UTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamSeatingPlan> ExamSeatingPlans { get; set; }
    }
}