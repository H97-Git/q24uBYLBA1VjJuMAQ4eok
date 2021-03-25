using FluentValidation;
using Mapster;
using Patient_Demographics.DTO;
using Patient_Demographics.Infrastructure.Repositories;
using Patient_Demographics.Internal;
using Patient_Demographics.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patient_Demographics.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly PatientValidator _patientValidator;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
            _patientValidator = new PatientValidator();
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
                ? throw new KeyNotFoundException($"{id}")
                : patient.Adapt<PatientDto>();
        }

        public async Task UpdatePatient(PatientDto patientDto)
        {
            var validationResult = await _patientValidator.ValidateAsync(patientDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString(),validationResult.Errors);
            }
            var patient = await _patientRepository.GetPatient(patientDto.Id);
            patientDto.Adapt(patient);
            await _patientRepository.UpdatePatient(patient);

        }

        public async Task SavePatient(PatientDto patientDto)
        {
            var validationResult = await _patientValidator.ValidateAsync(patientDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString(),validationResult.Errors);
            }
            var patient = patientDto.Adapt<Patient>();
            await _patientRepository.SavePatient(patient);
        }
    }
}
