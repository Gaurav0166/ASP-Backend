using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentBookingPatientAPI.Data;
using AppointmentBookingPatientAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentBookingPatientAPI.Repository
{
    public class JWTManagerRepository : IJWTManagerRepository
    {

        private readonly IConfiguration iconfiguration;
        private readonly ApplicationDbContext _context;
        public JWTManagerRepository(IConfiguration iconfiguration, ApplicationDbContext _context)
        {
            this.iconfiguration = iconfiguration;
            this._context = _context;
        }
        public Tokens Authenticate(LoginPatient LoginPatient)
        {
            if (!_context.PatientRegistration.Any(x => x.EmailID ==
                LoginPatient.EmailID && x.Password == LoginPatient.Password))
            {
                return null;
            }

            /*var y = _context.PatientRegistration.FirstOrDefaultAsync(s => s.EmailID == LoginPatient.EmailID && s.Password == LoginPatient.Password);
            if (y == null)
            {
                return null;
            }*/

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
             new Claim(ClaimTypes.Name, LoginPatient.EmailID)
              }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { Token = tokenHandler.WriteToken(token) };
        }
    }
}
