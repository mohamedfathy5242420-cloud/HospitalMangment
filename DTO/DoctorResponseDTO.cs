namespace App.DTO
{
    public class DoctorResponseDTO
    {
        public int Id { get; set; }            // من Doctor
        public string FullName { get; set; }   // من ApplicationUser
        public string Email { get; set; }      // من ApplicationUser
        public string Specialty { get; set; }  // من Doctor
    }
}
