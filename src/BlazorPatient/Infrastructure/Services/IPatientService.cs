using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPatient.DTO;

namespace BlazorPatient.Infrastructure.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> Get();
        Task<int> Save(PatientDto patientDto);
    }
}