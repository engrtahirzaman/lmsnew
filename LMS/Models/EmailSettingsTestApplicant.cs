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
    
    public partial class EmailSettingsTestApplicant
    {
        public int ID { get; set; }
        public string ApplicantsType { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string EmailTitle { get; set; }
        public string EmailMessage { get; set; }
        public bool isSendEmail { get; set; }
        public string SMSMessage { get; set; }
        public bool isSendSMS { get; set; }
        public Nullable<System.DateTime> StopSendingDate { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
    }
}