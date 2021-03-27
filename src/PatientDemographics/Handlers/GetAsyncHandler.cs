using MediatR;
using PatientDemographics.DTO;
using PatientDemographics.Infrastructure.Services;
using PatientDemographics.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PatientDemographics.Handlers
{
    public class GetAsyncHandler : IRequestHandler<GetAllPatientQuery, List<PatientDto>>
    {
        private readonly IPatientService _patientService;

        public GetAsyncHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<List<PatientDto>> Handle(GetAllPatientQuery request, CancellationToken cancellationToken)
        {
            return await _patientService.GetPatient();
        }
    }
}
