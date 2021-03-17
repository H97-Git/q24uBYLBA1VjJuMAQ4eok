using System.Collections.Generic;
using System.Threading.Tasks;
using Patient_Demographics.DTO;

namespace Patient_Demographics.Infrastructure.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetPatient();
        Task<PatientDto> GetPatient(int id);
        Task UpdatePatient(PatientDto patientDto);
        Task SavePatient(PatientDto patientDto);
        //Task DeletePatient(int id);
    }
}