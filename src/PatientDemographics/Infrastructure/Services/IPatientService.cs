using System.Collections.Generic;
using System.Threading.Tasks;
using PatientDemographics.Data.DTO;

namespace PatientDemographics.Infrastructure.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetPatient();
        Task<PatientDto> GetPatient(int id);
        Task UpdatePatient(int id,PatientDto patientDto);
        Task SavePatient(PatientDto patientDto);
    }
}