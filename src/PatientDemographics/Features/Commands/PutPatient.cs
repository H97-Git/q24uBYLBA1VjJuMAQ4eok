using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientDemographics.Data.DTO;
using PatientDemographics.Infrastructure.Services;

namespace PatientDemographics.Features.Commands
{
    public static class PutPatient
    {
        public record Command(int Id,PatientDto PatientDto) : IRequest<PatientDto>;

        public class Handler : IRequestHandler<Command, PatientDto>
        {
            private readonly IPatientService _patientService;

            public Handler(IPatientService patientService)
            {
                _patientService = patientService;
            }

            public async Task<PatientDto> Handle(Command command, CancellationToken cancellationToken)
            {
                await _patientService.UpdatePatient(command.Id ,command.PatientDto);
                return await _patientService.GetPatient(command.PatientDto.Id);
            }
        }
    }
}
