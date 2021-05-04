using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientDemographics.Data.DTO;
using PatientDemographics.Infrastructure.Services;

namespace PatientDemographics.Features.Queries
{
    public static class GetAllPatient
    {
        public record Query() : IRequest<Response>;

        public class Handler : IRequestHandler<Query,Response>
        {
            private readonly IPatientService _patientService;

            public Handler(IPatientService patientService)
            {
                _patientService = patientService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                return new(await _patientService.GetPatient());
            }
        }

        public record Response(List<PatientDto> PatientsDto);
    }
}
