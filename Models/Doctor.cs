using System.Collections.Generic;

namespace Hospital.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Specialty { get; set; }

        // Foreign Key
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Navigation
        public ICollection<Appointment> Appointments { get; set; }
    }
}
