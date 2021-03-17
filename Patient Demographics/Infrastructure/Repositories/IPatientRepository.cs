using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Patient_Demographics.Models;

namespace Patient_Demographics.Infrastructure.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetPatient();
        Task<Patient> GetPatient(int id);
        Task UpdatePatient([Required] Patient patient);
        Task SavePatient([Required] Patient patient);
        //Task DeletePatient(int id);
    }
}