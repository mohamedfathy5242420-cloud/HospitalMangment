using App.DTO;
using Hospital.DTO;
using Hospital.Models;
using Hospital.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly GenericRepository<Patient> _patientRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientController(GenericRepository<Patient> patientRepo, UserManager<ApplicationUser> userManager)
        {
            _patientRepo = patientRepo;
            _userManager = userManager;
        }

        // GET: api/Patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponseDTO>>> GetPatients()
        {
            var patients = await _patientRepo.GetAllAsync();

            var dtos = patients.Select(p => new PatientResponseDTO
            {
                Id = p.Id,
                FullName = p.User.FullName,
                Email = p.User.Email,
                Age = p.Age
            });

            return Ok(dtos);
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientResponseDTO>> GetPatient(int id)
        {
            var patient = await _patientRepo.FindAsync(p => p.Id == id);

            if (patient == null)
                return NotFound();

            var dto = new PatientResponseDTO
            {
                Id = patient.Id,
                FullName = patient.User.FullName,
                Email = patient.User.Email,
                Age = patient.Age
            };

            return Ok(dto);
        }

        // POST: api/Patient
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PatientResponseDTO>> CreatePatient([FromBody] PatientRequestDTO model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _userManager.IsInRoleAsync(user, "Patient"))
            {
                await _userManager.AddToRoleAsync(user, "Patient");
            }

            var patient = new Patient
            {
                Age = model.Age,
                UserId = user.Id,
                User = user
            };

            await _patientRepo.AddAsync(patient);

            var response = new PatientResponseDTO
            {
                Id = patient.Id,
                FullName = user.FullName,
                Email = user.Email,
                Age = patient.Age
            };

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, response);
        }

        // PUT: api/Patient/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientRequestDTO model)
        {
            var patient = await _patientRepo.FindAsync(p => p.Id == id);
            if (patient == null)
                return NotFound();

            patient.User.FullName = model.FullName;
            patient.User.Email = model.Email;
            patient.Age = model.Age;

            await _patientRepo.UpdateAsync(patient);

            return NoContent();
        }

        // DELETE: api/Patient/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _patientRepo.FindAsync(p => p.Id == id);
            if (patient == null)
                return NotFound();

            await _userManager.DeleteAsync(patient.User);

            return NoContent();
        }
    }
}
