namespace Hospital.DTO
{
    public class BillResponseDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string PatientFullName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorFullName { get; set; }
    }
}
