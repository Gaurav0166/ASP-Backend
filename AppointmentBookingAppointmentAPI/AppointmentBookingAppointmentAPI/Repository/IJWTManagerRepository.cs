using AppointmentBookingAppointmentAPI.Models;

namespace AppointmentBookingAppointmentAPI.Repository
{
    public interface IJWTManagerRepository
    {
       Tokens Authenticate(Appointment Appointment);
    }
}
