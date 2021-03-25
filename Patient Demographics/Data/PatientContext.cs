using Microsoft.EntityFrameworkCore;
using Patient_Demographics.Models;

namespace Patient_Demographics
{
    public class PatientContext:DbContext
    {
        public PatientContext(DbContextOptions<PatientContext> options):base(options)
        {
            
        }
        public DbSet<Patient> Patients { get; set; }
    }
}