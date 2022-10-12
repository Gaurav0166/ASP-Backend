using AppointmentBookingDoctorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBookingDoctorAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<DoctorRegistration> DoctorRegistration { get; set; }
        public DbSet<Login> Login { get; set; }
    }
}
