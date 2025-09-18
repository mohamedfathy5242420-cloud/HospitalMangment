namespace Hospital.DTO
{
    public class BillRequestDTO
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public decimal Amount { get; set; }
    }
}
