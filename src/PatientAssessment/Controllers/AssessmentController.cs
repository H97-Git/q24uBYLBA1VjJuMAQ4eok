using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using PatientAssessment.Data;
using PatientAssessment.Features.Queries;

namespace PatientAssessment.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssessmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a patient assessment by id.
        /// </summary>
        /// <param name="patientId">The id of a patient.</param>
        /// <returns>
        /// The <see cref="Assessment"/> A patient assessment.
        /// </returns>
        [HttpGet("{patientId:int}")]
        public async Task<ActionResult<Assessment>> GetByIdAsync(int patientId)
        {
            var query = new GetAssessmentByPatientId.Query(patientId);
            var response = await _mediator.Send(query);
            return Ok(response.Assessment);
        }
    }
}
