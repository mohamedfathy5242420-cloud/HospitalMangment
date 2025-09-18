namespace Hospital.DTO
{
    public class MedicalRecordRequestDTO
    {
        public int PatientId { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }
}
