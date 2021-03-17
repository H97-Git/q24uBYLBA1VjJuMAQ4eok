using Mapster;
using Patient_Demographics.DTO;
using Patient_Demographics.Infrastructure.Repositories;
using Patient_Demographics.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patient_Demographics.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly PatientRepository _patientRepository;

        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<List<PatientDto>> GetPatient()
        {
            var patients = await _patientRepository.GetPatient();

            return patients.Adapt<List<PatientDto>>();
        }

        public async Task<PatientDto> GetPatient(int id)
        {
            var patient = await _patientRepository.GetPatient(id);

            return patient == null
                ? throw new KeyNotFoundException("Patient Not Found")
                : patient.Adapt<PatientDto>();
        }

        public async Task UpdatePatient(PatientDto patientDto)
        {
            var patient = await _patientRepository.GetPatient(patientDto.Id);
            patientDto.Adapt(patient);
                await _patientRepository.UpdatePatient(patient);
            
        }

        public async Task SavePatient(PatientDto patientDto)
        {
            var patient = patientDto.Adapt<Patient>();
            await _patientRepository.SavePatient(patient);
        }

        //public async Task DeletePatient(int id)
        //{
        //    //Delete isn't specified
        //}
    }
}
