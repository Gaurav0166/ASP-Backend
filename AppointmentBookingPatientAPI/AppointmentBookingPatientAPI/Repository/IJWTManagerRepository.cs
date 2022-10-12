using AppointmentBookingPatientAPI.Models;

namespace AppointmentBookingPatientAPI.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(LoginPatient LoginPatient);
    }
}
