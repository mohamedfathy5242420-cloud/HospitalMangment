namespace Hospital.DTO
{
    public class MedicalRecordResponseDTO
    {
        public int Id { get; set; }
        public string PatientFullName { get; set; }
        public string CreatedBy { get; set; } // User who created the record
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
