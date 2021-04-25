using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientAssessment.Data;
using PatientAssessment.Infrastructure.Services;

namespace PatientAssessment.Features.Queries
{
    public class GetAssessmentByPatientId
    {
        public record Query(int PatientId):IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IAssessmentService _assessmentService;

            public Handler(IAssessmentService assessmentService)
            {
                _assessmentService = assessmentService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var assessment = await _assessmentService.GetAssessmentByPatientId(request.PatientId);
                return new Response(assessment);
            }
        }

        public record Response(Assessment Assessment);
    }
}
