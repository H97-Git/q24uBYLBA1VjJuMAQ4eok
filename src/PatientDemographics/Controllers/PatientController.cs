using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDemographics.Commands;
using PatientDemographics.DTO;
using PatientDemographics.Queries;
using System;
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
        /// Gets all the patients.
        /// </summary>
        /// <returns>
        /// The <see cref="List{PatientDto}"/>A list of patient.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatientDto>>> GetAllPatientAsync()
        {
            var query = new GetAllPatientQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets the patient by id.
        /// </summary>
        /// <param name="id">The id of a patient.</param>
        /// <returns>
        /// The <see cref="PatientDto"/>A patient information.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> GetByIdAsync(int id)
        {
            var query = new GetPatientByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Save a patient.
        /// </summary>
        /// <param name="family">Patient family name</param>
        /// <param name="given">Patient given name</param>
        /// <param name="dob">Patient date of birth</param>
        /// <param name="gender">Patient gender</param>
        /// <param name="address">Patient home address</param>
        /// <param name="phone">Patient phone number</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/>The patient created.
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
            var command = new PostPatientParamsCommand(family, given, dob, gender, address, phone);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        /// <summary>
        /// Save a patient.
        /// </summary>
        /// <param name="patientDto">the patient information from the body</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/>The patient created.
        /// </returns>
        [HttpPost("addBody")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientDto>> PostAsync([FromBody] PatientDto patientDto)
        {
            var command = new PostPatientBodyCommand(patientDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update a patient by id.
        /// </summary>
        /// <param name="id">The id of a patient.</param>
        /// <param name="patientDto">The patient dto to update.</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/>The patient updated.
        /// </returns>
        [HttpPut("edit/{id}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> PutAsync(int id, [FromBody] PatientDto patientDto)
        {
            var command = new PutPatientCommand(id, patientDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
