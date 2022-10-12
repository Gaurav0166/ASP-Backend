using System.Net.Http.Headers;
using AppointmentBookingDoctorAPI.Data;
using AppointmentBookingDoctorAPI.Models;
using AppointmentBookingDoctorAPI.Repository;
//using AppointmentBookinkDoctorAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppointmentBookingDoctorAPI.Controllers
{
    [EnableCors("MyPolicy")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJWTManagerRepository _jWTManager;
        public DoctorController(ApplicationDbContext context, IJWTManagerRepository jWTManager)
        {
            _context = context;
            _jWTManager = jWTManager;
        }
        [HttpGet]
        public ActionResult<IEnumerable<DoctorRegistration>> GetAllDoctors()
        {
            try
            {
                return _context.DoctorRegistration;//Here Doctor Registration is the table name that we are getting from the database referenece i.e, _context
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        [HttpGet("id")]
        public ActionResult<DoctorRegistration> GetDoctor(int id)
        {
            try
            {
                var data = _context.DoctorRegistration.FirstOrDefault(s => s.DoctorID == id);
                if (data == null)
                {
                    return NotFound();
                }
                return data;
            }
            catch(Exception)
            {
                throw;
            }
            
        }
        [AllowAnonymous] //Will override the [Authorize]
        [HttpPost("AddDoctor")]
        public async Task<ActionResult<DoctorRegistration>> AddDoctor(DoctorRegistration data)
        {
            try
            {
                var _create = new DoctorRegistration();
                _create.DoctorID = data.DoctorID;
                _create.DoctorName = data.DoctorName;
                _create.Speciality = data.Speciality;
                _create.Gender = data.Gender;
                _create.Email = data.Email;
                _create.Password = data.Password;
                var save_data = await _context.DoctorRegistration.AddAsync(_create);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, "Doctor Registered successfully");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            //return Ok();
        }

        [HttpDelete("RemoveDoctor/{id}")]
        public async Task<ActionResult<DoctorRegistration>> RemoveDoctor(int id)
        {
            try
            {
                var data = await _context.DoctorRegistration.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _context.DoctorRegistration.Remove(data);
                await _context.SaveChangesAsync();
                return Ok("Doctor Deleted Successfully");

            }
            catch(Exception){
                throw;
            }           
        }

        [HttpPut("UpdateDoctor/{Id}")]
        public async Task<IActionResult> Put([FromRoute] int Id, [FromBody] DoctorRegistration data)
        {
            if (Id != data.DoctorID)
            {
                return BadRequest();
            }
            _context.Entry(data).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok("Doctor Updation is Successful.");
        }

        //Added for api linking
        /*[HttpGet]
       // [AllowAnonymous]
        [Route("ViewPatients")]
        public async Task<ActionResult> ViewPatients() //From Patient api
        {
            List<PatientRegistration> EmpInfo = new List<PatientRegistration>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri("https://localhost:7153/");
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Patient");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    EmpInfo = JsonConvert.DeserializeObject<List<PatientRegistration>>(EmpResponse);
                }
                //returning the employee list to view
                return Ok(EmpInfo);
            }
        }*/


        [HttpGet("getAppointment")]
        public string GetAppointment()
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
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);//Adding Token to the header
                        var response = client.GetAsync("api/Appointment").Result;
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

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(Login Logindata)
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

        /*[HttpPut("UpdateDoctor/{id}")]
        public async Task<ActionResult<DoctorRegistration>> UpdateDoctor(DoctorRegistration data)
        {
            var entity = await _context.DoctorRegistration.FirstOrDefaultAsync(s => s.DoctorID == data.DoctorID);
            if (entity == null)
            {
                return NotFound();
            }
            //data is newly created that we will enter in the api
            //and entity is existing entry from the DB


            entity.DoctorID = data.DoctorID;
            entity.DoctorName = data.DoctorName;
            entity.Speciality = data.Speciality;
            entity.Gender = data.Gender;
            entity.Email = data.Email;
            entity.Password = data.Password;

            await _context.SaveChangesAsync();
            return Ok();
            
        }*/

    }
}
