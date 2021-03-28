using Microsoft.EntityFrameworkCore;
using PatientDemographics.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PatientDemographics.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private static PatientContext _patientContext;

        public PatientRepository(PatientContext patientContext)
        {
            _patientContext = patientContext;
        }

        public async Task<List<Patient>> GetPatient()
        {
            var patients = await _patientContext.Patients.ToListAsync();

            return patients;
        }

        public async Task<Patient> GetPatient(int id)
        {
            var patient = await _patientContext.Patients.FindAsync(id);

            return patient;
        }

        public async Task UpdatePatient([Required] Patient patient)
        {
            _patientContext.Patients.UpdateRange(patient);
            await _patientContext.SaveChangesAsync();
        }

        public async Task SavePatient([Required] Patient patient)
        {
            await _patientContext.Patients.AddAsync(patient);
            await _patientContext.SaveChangesAsync();
        }

    }
}
