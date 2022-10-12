using System.Net.Http.Headers;
using System.Text;
using AppointmentBookingPatientAPI.Data;
using AppointmentBookingPatientAPI.Models;
using AppointmentBookingPatientAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppointmentBookingPatientAPI.Controllers
{
    [EnableCors("MyPolicy")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJWTManagerRepository _jWTManager;
        public PatientController(ApplicationDbContext context, IJWTManagerRepository jWTManager)
        {
            _context = context;
            _jWTManager = jWTManager;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PatientRegistration>> GetAllPatients()
        {
            try
            {
                return _context.PatientRegistration;//Table Name
            }
            catch (Exception)
            {
                throw;
            }           
        }

        /*[HttpGet("{id}")]*/
        /*[HttpGet("id")]*/
        [HttpGet("GetPatient/{id}")]
        public ActionResult<PatientRegistration> GetPatient(int id)
        {
            try
            {
                var data = _context.PatientRegistration.FirstOrDefault
                (s => s.PatientID == id);
                if (data == null)
                {
                    return NotFound();
                }
                return data;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        [AllowAnonymous]
        [HttpPost("AddPatient")]
        public async Task<ActionResult<PatientRegistration>> AddPatient(PatientRegistration data)
        {
            try
            {
                var _create = new PatientRegistration();
                _create.PatientID = data.PatientID;
                _create.Mobile_Number = data.Mobile_Number;
                _create.Name = data.Name;
                _create.EmailID = data.EmailID;
                _create.Password = data.Password;
                _create.Gender = data.Gender;
                _create.Dob = data.Dob;
                _create.Address = data.Address;
                var save_data = await _context.PatientRegistration.AddAsync(_create);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, "Patient Registered successfully");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    "Database Failure");
            }
        }

        [HttpDelete("RemovePatient/{id}")]
        public async Task<ActionResult<PatientRegistration>> RemovePatient(int id)
        {
            try
            {
                var data = await _context.PatientRegistration.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _context.PatientRegistration.Remove(data);
                await _context.SaveChangesAsync();

                return Ok("Patient Deleted Successfully");
            }
            catch (Exception)
            {
                throw;
            }           
        }

        [HttpPut("UpdatePatient/{id}")]
        public async Task<ActionResult<PatientRegistration>> UpdatePatient(PatientRegistration data)
        {
            try
            {
                var entity = await _context.PatientRegistration.FirstOrDefaultAsync
                (s => s.PatientID == data.PatientID);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.PatientID = data.PatientID;
                entity.Mobile_Number = data.Mobile_Number;
                entity.Name = data.Name;
                entity.EmailID = data.EmailID;
                entity.Password = data.Password;
                entity.Gender = data.Gender;
                entity.Dob = data.Dob;
                entity.Address = data.Address;

                await _context.SaveChangesAsync();
                return Ok("Patient Updated Successfully");
            }
            catch (Exception)
            {
                throw;
            }           
        }

        [HttpGet("getAppointment")]
        public string GetAppointment()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var strToken);
                if (!string.IsNullOrEmpty(strToken))
                {
                    string token = ((string)strToken).Split(" ")[1];//It will contain Bearer completeToken, Hence removing Bearer keyword from the string
                    string responseString = string.Empty;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7082/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var response = client.GetAsync("api/Appointment").Result;//Appointment url from Patient microservice
                        if (response.IsSuccessStatusCode)
                        {
                            responseString = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    return responseString;
                }
                return "Something went wrong";
            }
            catch (Exception)
            {
                throw;
            }            
        }

        /*[HttpPost("bookappointment")]
        public Appointment BookAppointment(Appointment value)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var strToken);
                if (!string.IsNullOrEmpty(strToken))
                {
                    string token = ((string)strToken).Split(" ")[1];
                    string responseString = string.Empty;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7082/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        StringContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                        var response = client.PostAsync("api/Appointment/AddAppointment", content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString1 = response.Content.ReadAsStringAsync().Result;
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    return value;
                }
                return value;
                
            }
            catch (Exception)
            {
                throw;
            }           
        }*/


        [HttpPost("bookappointment")]
        public string BookAppointment(Appointment value)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var strToken);
                if (!string.IsNullOrEmpty(strToken))
                {
                    string token = ((string)strToken).Split(" ")[1];
                    string responseString = string.Empty;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7082/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        StringContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                        var response = client.PostAsync("api/Appointment/AddAppointment", content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString1 = response.Content.ReadAsStringAsync().Result;
                            response.EnsureSuccessStatusCode();
                            return responseString1;
                        }
                    }                   
                }
                return "Please enter Another Date or Time Slot ---> Doctor Unavailable";
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(LoginPatient Logindata)
        {
            try
            {
                var token = _jWTManager.Authenticate(Logindata);

                if (token == null)
                {
                    return Unauthorized("Invalid Login Credentials");
                }
                return Ok(token);
            }
            catch (Exception)
            {
                throw;
            }           
        }
    }
}
