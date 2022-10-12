using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingDoctorAPI.Models
{
    public class DoctorRegistration
    {
        [Key]
        [Required]
        public int DoctorID { get; set; }
        [Required]
        public string DoctorName { get; set; }
        [Required]
        public string Speciality { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}
