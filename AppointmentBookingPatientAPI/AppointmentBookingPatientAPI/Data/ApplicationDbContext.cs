using AppointmentBookingPatientAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBookingPatientAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<PatientRegistration> PatientRegistration { get; set; }
        public DbSet<LoginPatient> LoginPatient { get; set; }
    }
}
