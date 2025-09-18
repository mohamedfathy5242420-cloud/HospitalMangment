using App.DTO;
using Hospital.DTO;
using Hospital.Models;
using Hospital.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicalRecordController : ControllerBase
    {
        private readonly GenericRepository<MedicalRecord> _recordRepo;
        private readonly GenericRepository<Patient> _patientRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public MedicalRecordController(
            GenericRepository<MedicalRecord> recordRepo,
            GenericRepository<Patient> patientRepo,
            UserManager<ApplicationUser> userManager)
        {
            _recordRepo = recordRepo;
            _patientRepo = patientRepo;
            _userManager = userManager;
        }

        // GET: api/MedicalRecord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecordResponseDTO>>> GetMedicalRecords()
        {
            var records = await _recordRepo.GetAllAsync();

            var dtos = records.Select(r => new MedicalRecordResponseDTO
            {
                Id = r.Id,
                PatientFullName = r.Patient?.User?.FullName,
                CreatedBy = r.User?.FullName,
                Diagnosis = r.Diagnosis,
                Treatment = r.Treatment,
                CreatedAt = r.CreatedAt
            });

            return Ok(dtos);
        }

        // GET: api/MedicalRecord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecordResponseDTO>> GetMedicalRecord(int id)
        {
            var record = await _recordRepo.FindAsync(r => r.Id == id);

            if (record == null)
                return NotFound();

            var dto = new MedicalRecordResponseDTO
            {
                Id = record.Id,
                PatientFullName = record.Patient?.User?.FullName,
                CreatedBy = record.User?.FullName,
                Diagnosis = record.Diagnosis,
                Treatment = record.Treatment,
                CreatedAt = record.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/MedicalRecord
        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<MedicalRecordResponseDTO>> CreateMedicalRecord([FromBody] MedicalRecordRequestDTO model)
        {
            var patient = await _patientRepo.GetByIdAsync(model.PatientId);
            if (patient == null)
                return BadRequest("Patient not found");

            // المستخدم الحالي اللي عامل الـ record
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var record = new MedicalRecord
            {
                PatientId = model.PatientId,
                Diagnosis = model.Diagnosis,
                Treatment = model.Treatment,
                UserId = user.Id,
                Patient = patient,
                User = user
            };

            await _recordRepo.AddAsync(record);

            var response = new MedicalRecordResponseDTO
            {
                Id = record.Id,
                PatientFullName = patient.User.FullName,
                CreatedBy = user.FullName,
                Diagnosis = record.Diagnosis,
                Treatment = record.Treatment,
                CreatedAt = record.CreatedAt
            };

            return CreatedAtAction(nameof(GetMedicalRecord), new { id = record.Id }, response);
        }

        // PUT: api/MedicalRecord/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateMedicalRecord(int id, [FromBody] MedicalRecordRequestDTO model)
        {
            var record = await _recordRepo.FindAsync(r => r.Id == id);
            if (record == null)
                return NotFound();

            record.Diagnosis = model.Diagnosis;
            record.Treatment = model.Treatment;

            await _recordRepo.UpdateAsync(record);

            return NoContent();
        }

        // DELETE: api/MedicalRecord/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var record = await _recordRepo.FindAsync(r => r.Id == id);
            if (record == null)
                return NotFound();

            await _recordRepo.DeleteAsync(record);

            return NoContent();
        }
    }
}
