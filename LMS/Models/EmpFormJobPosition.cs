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
    
    public partial class EmpFormJobPosition
    {
        public int ID { get; set; }
        public Nullable<int> PositionID { get; set; }
        public Nullable<System.DateTime> PositionAssignedDate { get; set; }
        public Nullable<int> EmpFormID { get; set; }
        public Nullable<bool> IsExpired { get; set; }
        public Nullable<int> IsExpiredBy { get; set; }
        public Nullable<System.DateTime> IsExpiredDate { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
    
        public virtual JobPosition JobPosition { get; set; }
    }
}