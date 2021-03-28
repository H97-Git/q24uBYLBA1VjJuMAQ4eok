using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PatientDemographics.Data;

namespace PatientDemographics.Infrastructure.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetPatient();
        Task<Patient> GetPatient(int id);
        Task UpdatePatient([Required] Patient patient);
        Task SavePatient([Required] Patient patient);
    }
}