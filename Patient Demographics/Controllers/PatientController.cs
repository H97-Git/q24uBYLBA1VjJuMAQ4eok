using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patient_Demographics.DTO;
using Patient_Demographics.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patient_Demographics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Gets all the patients.
        /// </summary>
        /// <returns>
        /// The <see cref="List{PatientDto}"/>A list of patient.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        public async Task<List<PatientDto>> GetAsync()
        {
            return await _patientService.GetPatient();
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
        public async Task<PatientDto> GetByIdAsync(int id)
        {
            var patient = await _patientService.GetPatient(id);
            return patient;
        }

        /// <summary>
        /// Save a patient.
        /// </summary>
        /// <param name="patientDto">The patient dto.</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/>The patient created.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientDto>> PostAsync([FromBody] PatientDto patientDto)
        {
            await _patientService.SavePatient(patientDto);
            return CreatedAtAction("Get", new { id = patientDto.Id }, patientDto);
        }

        /// <summary>
        /// Update a patient by id.
        /// </summary>
        /// <param name="id">The id of a patient.</param>
        /// <param name="patientDto">The patient dto to update.</param>
        /// <returns>
        /// The <see cref="ActionResult{PatientDto}"/>The patient updated.
        /// </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> PutAsync(int id, [FromBody] PatientDto patientDto)
        {
            await _patientService.UpdatePatient(patientDto);
            return Ok(_patientService.GetPatient(id));
        }
    }
}
