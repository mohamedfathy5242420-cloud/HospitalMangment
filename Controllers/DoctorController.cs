using App.DTO;
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
    [Authorize] // كل الأكشنز محتاجين توكن
    public class DoctorController : ControllerBase
    {
        private readonly GenericRepository<Doctor> _doctorRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorController(GenericRepository<Doctor> doctorRepo, UserManager<ApplicationUser> userManager)
        {
            _doctorRepo = doctorRepo;
            _userManager = userManager;
        }

        // GET: api/Doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorResponseDTO>>> GetDoctors()
        {
            var doctors = await _doctorRepo.GetAllAsync();

            var doctorDtos = doctors.Select(d => new DoctorResponseDTO
            {
                Id = d.Id,
                FullName = d.User.FullName,
                Email = d.User.Email,
                Specialty = d.Specialty
            });

            return Ok(doctorDtos);
        }

        // GET: api/Doctor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorResponseDTO>> GetDoctor(int id)
        {
            var doctor = await _doctorRepo.FindAsync(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            var dto = new DoctorResponseDTO
            {
                Id = doctor.Id,
                FullName = doctor.User.FullName,
                Email = doctor.User.Email,
                Specialty = doctor.Specialty
            };

            return Ok(dto);
        }

        // POST: api/Doctor
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DoctorResponseDTO>> CreateDoctor([FromBody] DoctorRequestDTO model)
        {
            // إنشاء ApplicationUser
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // إضافة الدور Doctor
            if (!await _userManager.IsInRoleAsync(user, "Doctor"))
            {
                await _userManager.AddToRoleAsync(user, "Doctor");
            }

            // إنشاء Doctor
            var doctor = new Doctor
            {
                Specialty = model.Specialty,
                UserId = user.Id,
                User = user
            };

            await _doctorRepo.AddAsync(doctor);

            var response = new DoctorResponseDTO
            {
                Id = doctor.Id,
                FullName = user.FullName,
                Email = user.Email,
                Specialty = doctor.Specialty
            };

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, response);
        }

        // PUT: api/Doctor/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorRequestDTO model)
        {
            var doctor = await _doctorRepo.FindAsync(d => d.Id == id);
            if (doctor == null)
                return NotFound();

            // تحديث معلومات User
            doctor.User.FullName = model.FullName;
            doctor.User.Email = model.Email;

            // تحديث Specialty
            doctor.Specialty = model.Specialty;

            await _doctorRepo.UpdateAsync(doctor);

            return NoContent();
        }

        // DELETE: api/Doctor/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _doctorRepo.FindAsync(d => d.Id == id);
            if (doctor == null)
                return NotFound();

            // حذف الـ User (سيحذف تلقائياً الـ Doctor لأن عندنا 1:1 relationship)
            await _userManager.DeleteAsync(doctor.User);

            return NoContent();
        }
    }
}
