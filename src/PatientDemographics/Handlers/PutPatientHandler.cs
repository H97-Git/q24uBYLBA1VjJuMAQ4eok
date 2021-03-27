using MediatR;
using PatientDemographics.Commands;
using PatientDemographics.DTO;
using PatientDemographics.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;

namespace PatientDemographics.Handlers
{
    public class PutPatientHandler : IRequestHandler<PutPatientCommand, PatientDto>
    {
        private readonly IPatientService _patientService;

        public PutPatientHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<PatientDto> Handle(PutPatientCommand request, CancellationToken cancellationToken)
        {
            await _patientService.UpdatePatient(request.PatientDto);
            return await _patientService.GetPatient(request.Id);
        }
    }
}
