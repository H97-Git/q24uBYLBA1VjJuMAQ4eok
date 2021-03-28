using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientDemographics.Data.DTO;
using PatientDemographics.Infrastructure.Services;

namespace PatientDemographics.Features.Commands
{
    public static class PostPatientParams
    {
        public record Command(string Family, string Given, DateTime Dob, string Gender, string Address, string Phone): IRequest<PatientDto>;

        public class Handler : IRequestHandler<Command,PatientDto>
        {
            private readonly IPatientService _patientService;

            public Handler(IPatientService patientService)
            {
                _patientService = patientService;
            }

            public async Task<PatientDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var patientDto = MapPatient(command.Family, command.Given, command.Dob, command.Gender, command.Address, command.Phone);
                await _patientService.SavePatient(patientDto);
                var list = await _patientService.GetPatient();
                var patient = list.Find(p => p.FamilyName == command.Family);
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
}
