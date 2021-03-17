using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patient_Demographics.DTO;
using Patient_Demographics.Infrastructure.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET: api/<PatientController>
        [HttpGet]
        public async Task<List<PatientDto>> Get()
        {
            return await _patientService.GetPatient();
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public async Task<PatientDto> Get(int id)
        {
            var patient = await _patientService.GetPatient(id);
            return patient;
        }

        // POST api/<PatientController>
        [HttpPost]
        public ActionResult<PatientDto> Post([FromBody] PatientDto patientDto)
        {
            _patientService.SavePatient(patientDto);
            return CreatedAtAction("Get", new {id = patientDto.Id}, patientDto);
        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PatientDto patientDto)
        {
            await _patientService.UpdatePatient(patientDto);
            return Ok(_patientService.GetPatient(id));
        }

        //// DELETE api/<PatientController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
