namespace Hospital.DTO
{
    public class AppointmentRequestDTO
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime Date { get; set; }
    }
}
