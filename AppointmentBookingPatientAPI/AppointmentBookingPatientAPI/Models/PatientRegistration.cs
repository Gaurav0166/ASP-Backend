using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingPatientAPI.Models
{
    public class PatientRegistration
    {
        [Required]
        [Key]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Format")]
        public long Mobile_Number { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string EmailID { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Dob { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
