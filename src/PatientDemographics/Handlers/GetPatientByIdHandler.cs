using MediatR;
using PatientDemographics.DTO;
using PatientDemographics.Infrastructure.Services;
using PatientDemographics.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace PatientDemographics.Handlers
{
    public class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery,PatientDto>
    {
        private readonly IPatientService _patientService;

        public GetPatientByIdHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await _patientService.GetPatient(request.Id);
            return patient;
        }
    }
}
