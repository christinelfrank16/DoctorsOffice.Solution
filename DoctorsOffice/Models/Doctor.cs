using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoctorsOffice.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Specialty { get; set; }

        [Display(Name = "Name")]
        public string WholeName 
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        public ICollection<DoctorPatient> Patients { get; set; }

        public Doctor(){
            this.Patients = new HashSet<DoctorPatient>();
        }
    }
}