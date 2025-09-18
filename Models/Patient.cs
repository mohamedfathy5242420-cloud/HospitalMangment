using System.Collections.Generic;

namespace Hospital.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public int Age { get; set; }

        // Foreign Key
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Navigation
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Bill> Bills { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
