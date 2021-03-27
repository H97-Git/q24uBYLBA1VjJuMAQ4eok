using MediatR;
using PatientDemographics.Commands;
using PatientDemographics.DTO;
using PatientDemographics.Infrastructure.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PatientDemographics.Handlers
{
    public class PostPatientParamsHandler : IRequestHandler<PostPatientParamsCommand, PatientDto>
    {
        private readonly IPatientService _patientService;

        public PostPatientParamsHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<PatientDto> Handle(PostPatientParamsCommand request, CancellationToken cancellationToken)
        {
            var patientDto = MapPatient(request.Family, request.Given, request.Dob, request.Gender, request.Address, request.Phone);
            await _patientService.SavePatient(patientDto);
            var list = await _patientService.GetPatient();
            var patient = list.Find(p => p.FamilyName == request.Family);
            return patient;
        }

        private static PatientDto MapPatient(string family, string given, DateTime dob, string gender, string address, string phone)
        {
            var patientDto = new PatientDto
            {
                FamilyName = family,
                GivenName = given,
                DateOfBirth = dob,
                HomeAddress = address,
                PhoneNumber = phone
            };
            switch (gender)
            {
                case "0":
                case "M":
                case "Male":
                    patientDto.Gender = Gender.Male;
                    break;
                case "1":
                case "F":
                case "Female":
                    patientDto.Gender = Gender.Female;
                    break;
            }

            return patientDto;
        }
    }
}
