using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientDemographics.Data.DTO;
using PatientDemographics.Features.Commands;
using PatientDemographics.Features.Queries;

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
        [HttpGet("{id}")]
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
        /// <param name="family"> Patient family name</param>
        /// <param name="given"> Patient given name</param>
        /// <param name="dob"> Patient date of birth</param>
        /// <param name="gender"> Patient gender</param>
        /// <param name="address"> Patient home address</param>
        /// <param name="phone"> Patient phone number</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/> The patient created.
        /// </returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientDto>> PostAsync(
            [FromQuery] string family,
            [FromQuery] string given,
            [FromQuery] DateTime dob,
            [FromQuery] string gender,
            [FromQuery] string address,
            [FromQuery] string phone)
        {
            var command = new PostPatientParams.Command(family, given, dob, gender, address, phone);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        /// <summary>
        /// Save a patient.
        /// </summary>
        /// <param name="command"> The command with the patient information from the body</param>
        /// <returns>
        /// The <see cref="PatientDto"/> The patient created.
        /// </returns>
        [HttpPost("addBody")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientDto>> PostBodyAsync([FromBody] PostPatientBody.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update a patient by id.
        /// </summary>
        /// <param name="command"> The command with the id and data of a patient.</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/> The patient updated.
        /// </returns>
        [HttpPut("edit/{id}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> PutAsync([FromBody] PutPatient.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
