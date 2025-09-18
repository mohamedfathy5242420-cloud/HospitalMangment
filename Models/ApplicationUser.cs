using Hospital.Models;
using Microsoft.AspNetCore.Identity;
using System;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }
    public ICollection<MedicalRecord> MedicalRecords { get; set; }
}
