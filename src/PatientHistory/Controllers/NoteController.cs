using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientHistory.Data.DTO;
using PatientHistory.Features.Command;
using PatientHistory.Features.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientHistory.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NoteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <returns>
        /// The <see cref="List{NoteDto}"/> A list of note.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<List<NoteDto>>> GetAllAsync()
        {
            var query = new GetAllNote.Query();
            var result = await _mediator.Send(query);
            return result.NotesDto;
        }

        /// <summary>
        /// Get a note by id.
        /// </summary>
        /// <param name="id">The id of a note</param>
        /// <returns>
        /// The <see cref="NoteDto"/> A note.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetByIdAsync(string id)
        {
            var query = new GetNoteById.Query(id);
            var result = await _mediator.Send(query);
            return result.NoteDto;
        }

        /// <summary>
        /// Get all notes for a patient.
        /// </summary>
        /// <param name="patientId">The id of the patient</param>
        /// <returns>
        /// The <see cref="List{NoteDto}"/> A list of note for one patient.
        /// </returns>
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<NoteDto>>> GetByPatientIdAsync(int patientId)
        {
            var query = new GetNoteByPatientId.Query(patientId);
            var result = await _mediator.Send(query);
            return result.NotesDto;
        }

        /// <summary>
        /// Save a note for a patient.
        /// </summary>
        /// <param name="noteDto">The dto with the note information</param>
        /// <returns>
        /// The <see cref="NoteDto"/> The note created.
        /// </returns>
        [HttpPost("add")]
        public async Task<ActionResult<NoteDto>> PostAsync([FromBody] NoteDto noteDto)
        {
            var command = new PostPutNote.Command(noteDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { result.Id }, result);

        }

        /// <summary>
        /// Update a note for a patient.
        /// </summary>
        /// <param name="id">The id for a note </param>
        /// <param name="noteDto">The dto with the note information</param>
        /// <returns>
        /// The <see cref="NoteDto"/> The note created.
        /// </returns>
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<NoteDto>> PutAsync(string id, NoteDto noteDto)
        {
            var command = new PostPutNote.Command(noteDto, id);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdAsync), new { result.Id }, result);
        }
    }
}
