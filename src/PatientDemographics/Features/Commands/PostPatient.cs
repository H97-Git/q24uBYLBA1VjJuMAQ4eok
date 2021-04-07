using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientDemographics.Data.DTO;
using PatientDemographics.Infrastructure.Services;

namespace PatientDemographics.Features.Commands
{
    public static class PostPatient
    {
        public record Command(PatientDto PatientDto) : IRequest<PatientDto>;

        public class Handler : IRequestHandler<Command, PatientDto>
        {
            private readonly IPatientService _patientService;

            public Handler(IPatientService patientService)
            {
                _patientService = patientService;
            }

            public async Task<PatientDto> Handle(Command command, CancellationToken cancellationToken)
            {
                await _patientService.SavePatient(command.PatientDto);
                var list = await _patientService.GetPatient();
                var patient = list.Find(p => p.FamilyName == command.PatientDto.FamilyName);
                return patient;
            }
        }
    }
}
