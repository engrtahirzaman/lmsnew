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
    
    public partial class MissionUniversity
    {
        public int ID { get; set; }
        public Nullable<int> Number { get; set; }
        public string Title { get; set; }
        public string MissionStatement { get; set; }
        public Nullable<int> ACMID { get; set; }
        public bool isActive { get; set; }
        public Nullable<int> isNotActiveBy { get; set; }
        public Nullable<System.DateTime> isNotActiveDate { get; set; }
        public Nullable<System.TimeSpan> isNotActiveTime { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<System.TimeSpan> CrTime { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.TimeSpan> UTime { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
    
        public virtual ACM ACM { get; set; }
    }
}