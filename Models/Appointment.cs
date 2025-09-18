using System;

namespace Hospital.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        // Foreign Keys
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        // Navigation
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public Bill Bill { get; set; }
    }
}
