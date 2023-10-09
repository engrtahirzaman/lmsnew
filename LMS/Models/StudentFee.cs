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
    
    public partial class StudentFee
    {
        public int ID { get; set; }
        public Nullable<int> CampusID { get; set; }
        public int StudentID { get; set; }
        public int SessionID { get; set; }
        public int FeeHeadID { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string VoucherNo { get; set; }
        public string BankVoucherNo { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string BankBranch { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<System.TimeSpan> PaymentTime { get; set; }
        public bool IsPaid { get; set; }
        public bool IsVerified { get; set; }
        public Nullable<int> VerifiedBy { get; set; }
        public Nullable<System.DateTime> VerifiedDate { get; set; }
        public Nullable<System.TimeSpan> VerifiedTime { get; set; }
        public string Attachment { get; set; }
        public bool IsFake { get; set; }
        public Nullable<int> FakeBy { get; set; }
        public Nullable<System.DateTime> FakeDate { get; set; }
        public Nullable<System.TimeSpan> FakeTime { get; set; }
        public bool IsRefund { get; set; }
        public Nullable<int> IsRefundBy { get; set; }
        public Nullable<System.DateTime> IsRefundDate { get; set; }
        public Nullable<System.TimeSpan> IsRefundTime { get; set; }
        public string IsRefundComments { get; set; }
        public Nullable<decimal> IsRefundPercent { get; set; }
        public int CrBy { get; set; }
        public System.DateTime CrDate { get; set; }
        public int UBy { get; set; }
        public System.DateTime UDate { get; set; }
    
        public virtual Campus Campus { get; set; }
        public virtual FeeHead FeeHead { get; set; }
        public virtual Session Session { get; set; }
        public virtual Student Student { get; set; }
    }
}