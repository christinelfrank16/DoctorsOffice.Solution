using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace DoctorsOffice.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string WholeName 
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        public DateTime Birthday { get; set; }
        public ICollection<DoctorPatient> Doctors { get; set; }

        public Patient(){
            this.Doctors = new HashSet<DoctorPatient>();
        }
    }
}