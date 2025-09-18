namespace Hospital.DTO
{
    public class AppointmentResponseDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DoctorFullName { get; set; }
        public string PatientFullName { get; set; }
        public decimal? BillAmount { get; set; } // Optional
    }
}
