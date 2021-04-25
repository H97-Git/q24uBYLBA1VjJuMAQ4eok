using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientAssessment.Data;
using PatientAssessment.Infrastructure.Services;

namespace PatientAssessment.Features.Queries
{
    public class GetAssessment
    {
        public record Query():IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IAssessmentService _assessmentService;

            public Handler(IAssessmentService assessmentService)
            {
                _assessmentService = assessmentService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                //var assessments = await _assessmentService.GetAssessment();
                return new Response(null);
            }
        }

        public record Response(List<Assessment> Assessments);
    }
}
