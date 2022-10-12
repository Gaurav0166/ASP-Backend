using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingPatientAPI.Models
{
    public class Appointment
    {
        [Key]
        [Required]
        public int AppointmentID { get; set; }
        [Required]
        public int DoctorID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        [Required]
        [StringLength(200)]
        public string Issue { get; set; }
    }
}
