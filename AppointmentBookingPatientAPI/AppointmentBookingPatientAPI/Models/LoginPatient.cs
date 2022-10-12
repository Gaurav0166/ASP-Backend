using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingPatientAPI.Models
{
    public class LoginPatient
    {
        [Key]
        [Required]
        public string EmailID { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
