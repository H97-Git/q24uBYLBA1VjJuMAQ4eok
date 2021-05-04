using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDemographics.Data.DTO;
using PatientDemographics.Features.Commands;
using PatientDemographics.Features.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientDemographics.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <returns>
        /// The <see cref="List{PatientDto}"/> A list of patient.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatientDto>>> GetAllAsync()
        {
            var query = new GetAllPatient.Query();
            var result = await _mediator.Send(query);
            return Ok(result.PatientsDto);
        }

        /// <summary>
        /// Gets a patient by id.
        /// </summary>
        /// <param name="id">The id of a patient.</param>
        /// <returns>
        /// The <see cref="PatientDto"/> A patient information.
        /// </returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> GetByIdAsync(int id)
        {
            var query = new GetPatientById.Query(id);
            var response = await _mediator.Send(query);
            return Ok(response.PatientDto);
        }

        /// <summary>
        /// Save a patient.
        /// </summary>
        /// <param name="command"> The command with the patient information from the query</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/> The patient created.
        /// </returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<PatientDto>> PostAsync([FromQuery] PostPatient.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { result.Id }, result);
        }

        /// <summary>
        /// Save a patient.
        /// </summary>
        /// <param name="command"> The command with the patient information from the body</param>
        /// <returns>
        /// The <see cref="PatientDto"/> The patient created.
        /// </returns>
        [HttpPost("addBody")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<PatientDto>> PostBodyAsync([FromBody] PostPatient.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { result.Id }, result);
        }

        /// <summary>
        /// Update a patient.
        /// </summary>
        /// <param name="id">The id of a patient.</param>
        /// <param name="patientDto">The date of the up to date patient.</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/> The patient updated.
        /// </returns>
        [HttpPut("edit/{id:int}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> PutAsync(int id, [FromBody] PatientDto patientDto)
        {
            var command = new PutPatient.Command(id, patientDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
