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
    public class BillController : ControllerBase
    {
        private readonly GenericRepository<Bill> _billRepo;
        private readonly GenericRepository<Appointment> _appointmentRepo;
        private readonly GenericRepository<Patient> _patientRepo;

        public BillController(
            GenericRepository<Bill> billRepo,
            GenericRepository<Appointment> appointmentRepo,
            GenericRepository<Patient> patientRepo)
        {
            _billRepo = billRepo;
            _appointmentRepo = appointmentRepo;
            _patientRepo = patientRepo;
        }

        // GET: api/Bill
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillResponseDTO>>> GetBills()
        {
            var bills = await _billRepo.GetAllAsync();

            var dtos = bills.Select(b => new BillResponseDTO
            {
                Id = b.Id,
                Amount = b.Amount,
                Date = b.Date,
                PatientFullName = b.Patient?.User?.FullName,
                AppointmentDate = b.Appointment?.Date ?? DateTime.MinValue,
                DoctorFullName = b.Appointment?.Doctor?.User?.FullName
            });

            return Ok(dtos);
        }

        // GET: api/Bill/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillResponseDTO>> GetBill(int id)
        {
            var bill = await _billRepo.FindAsync(b => b.Id == id);

            if (bill == null)
                return NotFound();

            var dto = new BillResponseDTO
            {
                Id = bill.Id,
                Amount = bill.Amount,
                Date = bill.Date,
                PatientFullName = bill.Patient?.User?.FullName,
                AppointmentDate = bill.Appointment?.Date ?? DateTime.MinValue,
                DoctorFullName = bill.Appointment?.Doctor?.User?.FullName
            };

            return Ok(dto);
        }

        // POST: api/Bill
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BillResponseDTO>> CreateBill([FromBody] BillRequestDTO model)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(model.AppointmentId);
            var patient = await _patientRepo.GetByIdAsync(model.PatientId);

            if (appointment == null || patient == null)
                return BadRequest("Appointment or Patient not found");

            var bill = new Bill
            {
                AppointmentId = model.AppointmentId,
                PatientId = model.PatientId,
                Amount = model.Amount,
                Date = DateTime.Now
            };

            await _billRepo.AddAsync(bill);

            var response = new BillResponseDTO
            {
                Id = bill.Id,
                Amount = bill.Amount,
                Date = bill.Date,
                PatientFullName = patient.User.FullName,
                AppointmentDate = appointment.Date,
                DoctorFullName = appointment.Doctor?.User?.FullName
            };

            return CreatedAtAction(nameof(GetBill), new { id = bill.Id }, response);
        }

        // PUT: api/Bill/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBill(int id, [FromBody] BillRequestDTO model)
        {
            var bill = await _billRepo.FindAsync(b => b.Id == id);
            if (bill == null)
                return NotFound();

            var appointment = await _appointmentRepo.GetByIdAsync(model.AppointmentId);
            var patient = await _patientRepo.GetByIdAsync(model.PatientId);

            if (appointment == null || patient == null)
                return BadRequest("Appointment or Patient not found");

            bill.AppointmentId = model.AppointmentId;
            bill.PatientId = model.PatientId;
            bill.Amount = model.Amount;

            await _billRepo.UpdateAsync(bill);

            return NoContent();
        }

        // DELETE: api/Bill/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await _billRepo.FindAsync(b => b.Id == id);
            if (bill == null)
                return NotFound();

            await _billRepo.DeleteAsync(bill);

            return NoContent();
        }
    }
}
