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
    
    public partial class QuestionTypeAnswerOption
    {
        public int ID { get; set; }
        public int QuestionTypeID { get; set; }
        public string OptionName { get; set; }
        public string InputType { get; set; }
        public Nullable<int> CrBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<System.TimeSpan> CrTime { get; set; }
        public Nullable<int> UBy { get; set; }
        public Nullable<System.DateTime> UDate { get; set; }
        public Nullable<System.TimeSpan> UTime { get; set; }
        public Nullable<int> OptionValue { get; set; }
    
        public virtual QuestionType QuestionType { get; set; }
    }
}
