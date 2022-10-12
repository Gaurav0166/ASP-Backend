using AppointmentBookingDoctorAPI.Models;

namespace AppointmentBookingDoctorAPI.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(Login Login);
    }
}
