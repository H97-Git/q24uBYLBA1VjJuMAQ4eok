using System;
using FluentValidation;
using Mapster;
using PatientDemographics.Infrastructure.Repositories;
using PatientDemographics.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientDemographics.Data;
using PatientDemographics.Data.DTO;

namespace PatientDemographics.Infrastructure.Services
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

            return patient is null
                ? throw new KeyNotFoundException($"{id}")
                : patient.Adapt<PatientDto>();
        }

        public async Task UpdatePatient(int id, PatientDto patientDto)
        {
            if (patientDto is null)
            {
                throw new ArgumentNullException(nameof(patientDto));
            }
            var validationResult = await _patientValidator.ValidateAsync(patientDto);
            if (validationResult.IsValid is false)
            {
                throw new ValidationException(validationResult.ToString(), validationResult.Errors);
            }

            var patient = await _patientRepository.GetPatient(id);
            if (patient is null)
            {
                throw new KeyNotFoundException($"{patientDto.Id}");
            }

            patientDto.Adapt(patient);
            await _patientRepository.UpdatePatient(patient);
        }

        public async Task SavePatient(PatientDto patientDto)
        {
            if (patientDto is null)
            {
                throw new ArgumentNullException(nameof(patientDto));
            }

            var validationResult = await _patientValidator.ValidateAsync(patientDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString(), validationResult.Errors);
            }

            var patient = patientDto.Adapt<Patient>();
            await _patientRepository.SavePatient(patient);
        }
    }
}
