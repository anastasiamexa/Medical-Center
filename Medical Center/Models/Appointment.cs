//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Medical_Center.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Appointment
    {
        public int appid { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<System.TimeSpan> startSlotTime { get; set; }
        public Nullable<System.TimeSpan> endSlotTime { get; set; }
        public string PATIENT_patientAMKA { get; set; }
        public string DOCTOR_doctorAMKA { get; set; }
        public Nullable<bool> isAvailable { get; set; }
    
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
