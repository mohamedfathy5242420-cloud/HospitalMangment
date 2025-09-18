namespace Hospital.DTO
{
    public class PatientRequestDTO
    {
        public string FullName { get; set; }   // User
        public string Email { get; set; }      // User
        public string Password { get; set; }   // User
        public int Age { get; set; }           // Patient
    }
}
