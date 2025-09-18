using Hospital.Models;
using System.Collections.Generic;

public class Department
{
    public int DepartmentId { get; set; }
    public string Name { get; set; }

    // Navigation
    public ICollection<Doctor> Doctors { get; set; }
}
