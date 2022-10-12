using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingDoctorAPI.Models
{
    public class Login
    {
        [Key]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
