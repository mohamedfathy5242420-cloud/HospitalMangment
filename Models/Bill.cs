using System;

namespace Hospital.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        // Foreign Keys
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }

        // Navigation
        public Appointment Appointment { get; set; }
        public Patient Patient { get; set; }
    }
}
