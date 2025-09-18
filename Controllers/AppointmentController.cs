using App.DTO;
using Hospital.DTO;
using Hospital.Models;

using Hospital.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly GenericRepository<Appointment> _appointmentRepo;
        private readonly GenericRepository<Doctor> _doctorRepo;
        private readonly GenericRepository<Patient> _patientRepo;

        public AppointmentController(
            GenericRepository<Appointment> appointmentRepo,
            GenericRepository<Doctor> doctorRepo,
            GenericRepository<Patient> patientRepo)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> GetAppointments()
        {
            var appointments = await _appointmentRepo.GetAllAsync();

            var dtos = appointments.Select(a => new AppointmentResponseDTO
            {
                Id = a.Id,
                Date = a.Date,
                DoctorFullName = a.Doctor?.User?.FullName,
                PatientFullName = a.Patient?.User?.FullName,
                BillAmount = a.Bill?.Amount
            });

            return Ok(dtos);
        }

        // GET: api/Appointment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentResponseDTO>> GetAppointment(int id)
        {
            var appointment = await _appointmentRepo.FindAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            var dto = new AppointmentResponseDTO
            {
                Id = appointment.Id,
                Date = appointment.Date,
                DoctorFullName = appointment.Doctor?.User?.FullName,
                PatientFullName = appointment.Patient?.User?.FullName,
                BillAmount = appointment.Bill?.Amount
            };

            return Ok(dto);
        }

        // POST: api/Appointment
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AppointmentResponseDTO>> CreateAppointment([FromBody] AppointmentRequestDTO model)
        {
            var doctor = await _doctorRepo.GetByIdAsync(model.DoctorId);
            var patient = await _patientRepo.GetByIdAsync(model.PatientId);

            if (doctor == null || patient == null)
                return BadRequest("Doctor or Patient not found");

            var appointment = new Appointment
            {
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                Date = model.Date
            };

            await _appointmentRepo.AddAsync(appointment);

            var response = new AppointmentResponseDTO
            {
                Id = appointment.Id,
                Date = appointment.Date,
                DoctorFullName = doctor.User.FullName,
                PatientFullName = patient.User.FullName,
                BillAmount = appointment.Bill?.Amount
            };

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, response);
        }

        // PUT: api/Appointment/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentRequestDTO model)
        {
            var appointment = await _appointmentRepo.FindAsync(a => a.Id == id);
            if (appointment == null)
                return NotFound();

            var doctor = await _doctorRepo.GetByIdAsync(model.DoctorId);
            var patient = await _patientRepo.GetByIdAsync(model.PatientId);

            if (doctor == null || patient == null)
                return BadRequest("Doctor or Patient not found");

            appointment.DoctorId = model.DoctorId;
            appointment.PatientId = model.PatientId;
            appointment.Date = model.Date;

            await _appointmentRepo.UpdateAsync(appointment);

            return NoContent();
        }

        // DELETE: api/Appointment/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentRepo.FindAsync(a => a.Id == id);
            if (appointment == null)
                return NotFound();

            await _appointmentRepo.DeleteAsync(appointment);

            return NoContent();
        }
    }
}
