using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientDemographics.Data.DTO;
using PatientDemographics.Infrastructure.Services;

namespace PatientDemographics.Features.Queries
{
    public static class GetPatientById
    {
        public record Query(int Id) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IPatientService _patientService;

            public Handler(IPatientService patientService)
            {
                _patientService = patientService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                var patient = await _patientService.GetPatient(query.Id);
                return new Response(patient);
            }
        }

        public record Response(PatientDto PatientDto);
    }
}
