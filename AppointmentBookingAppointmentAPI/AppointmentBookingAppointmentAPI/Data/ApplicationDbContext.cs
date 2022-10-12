using AppointmentBookingAppointmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBookingAppointmentAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Appointment> Appointment { get; set; }
    }
}
