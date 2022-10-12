using AppointmentBookingAppointmentAPI.Data;
using AppointmentBookingAppointmentAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBookingAppointmentAPI.Controllers
{
    [Authorize]
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAllAppointments()
        {
            try
            {
                return _context.Appointment;//Table Name
            }
            catch (Exception)
            {
                throw;
            }           
        }
        [HttpGet("id")]
        public ActionResult<Appointment> GetAppointment(int id)
        {
            try
            {
                var data = _context.Appointment.FirstOrDefault
                (s => s.AppointmentID == id);
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

        [HttpPost("AddAppointment")]
        public async Task<ActionResult<Appointment>> AddAppointment(Appointment data)
        {
            try
            {
                //Adding for Validation of Time slot

                if (_context.Appointment.Any(x => x.DoctorID ==
                    data.DoctorID && x.AppointmentDate == data.AppointmentDate &&
                    x.TimeSlot == data.TimeSlot))//Checking for available time slots for that particular doctor
                {
                    //return null;
                    return NotFound("Please give another Date or Time Slot");
                }

                //Done

                var _create = new Appointment();
                _create.AppointmentID = data.AppointmentID;
                _create.DoctorID = data.DoctorID;
                _create.PatientID = data.PatientID;
                _create.AppointmentDate = data.AppointmentDate;
                _create.TimeSlot = data.TimeSlot;
                _create.Issue = data.Issue;
                var save_data = await _context.Appointment.AddAsync(_create);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, "Appointment Registered Successfully");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("CancelAppointment/{id}")]
        public async Task<ActionResult<Appointment>> CancelAppointment(int id)
        {
            try
            {
                var data = await _context.Appointment.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _context.Appointment.Remove(data);
                await _context.SaveChangesAsync();

                return Ok("Appointment Cancelled Successfully");
            }
            catch (Exception)
            {
                throw;
            }           
        }

        [HttpPut("UpdateAppointment/{id}")]
        public async Task<ActionResult<Appointment>> UpdateAppointment(Appointment data)
        {
            try
            {
                var entity = await _context.Appointment.FirstOrDefaultAsync
                (s => s.AppointmentID == data.AppointmentID);
                if (entity == null)
                {
                    return NotFound();
                }

                entity.AppointmentID = data.AppointmentID;
                entity.DoctorID = data.DoctorID;
                entity.PatientID = data.PatientID;
                entity.AppointmentDate = data.AppointmentDate;
                entity.TimeSlot = data.TimeSlot;
                entity.Issue = data.Issue;

                await _context.SaveChangesAsync();
                return Ok("Appointment Updated Successfully");
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
