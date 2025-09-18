using System;

namespace Hospital.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign Keys
        public string UserId { get; set; }  // ApplicationUser
        public int PatientId { get; set; }

        // Navigation
        public ApplicationUser User { get; set; }
        public Patient Patient { get; set; }
    }
}
