using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentBookingDoctorAPI.Data;
using AppointmentBookingDoctorAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentBookingDoctorAPI.Repository
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
        public Tokens Authenticate(Login Login)
        {
            if (!_context.DoctorRegistration.Any(x => x.Email == 
                Login.Email && x.Password == Login.Password))
               {
                  return null;
               }
            /*var y = _context.DoctorRegistration.FirstOrDefaultAsync(s => s.Email == Login.Email && s.Password == Login.Password);
            if (y == null)
            {
                return null;
            }*/
            else 
            {
                // Else we generate JSON Web Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
             new Claim(ClaimTypes.Name, Login.Email)
                  }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new Tokens { Token = tokenHandler.WriteToken(token) };

            }

            

        }

    }
}
