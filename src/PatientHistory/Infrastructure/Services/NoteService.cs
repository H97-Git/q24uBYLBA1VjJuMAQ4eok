using FluentValidation;
using Mapster;
using PatientHistory.Data;
using PatientHistory.Data.DTO;
using PatientHistory.Infrastructure.Repositories;
using PatientHistory.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientHistory.Infrastructure.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IPatientService _patientService;
        private readonly NoteValidator _noteValidator;

        public NoteService(INoteRepository noteRepository, IPatientService patientService)
        {
            _noteRepository = noteRepository;
            _patientService = patientService;
            _noteValidator = new NoteValidator();
        }

        public List<NoteDto> Get()
        {
            var notes = _noteRepository.Get();
            var dto = notes.Adapt<List<NoteDto>>();

            return dto;
        }

        public NoteDto Get(string id)
        {
            var note = _noteRepository.Get(id);
            if (note is null)
            {
                throw new KeyNotFoundException($"{id}");
            }
            var dto = note.Adapt<NoteDto>();

            return dto;
        }

        public List<NoteDto> GetByPatientId(int patientId)
        {
            var notes = Get();
            var notesByPatient = notes.FindAll(x => x.PatientId == patientId);

            return notesByPatient;
        }

        public async Task<string> Create(NoteDto noteDto)
        {
            if (noteDto is null)
            {
                throw new ArgumentNullException(nameof(NoteDto));
            }

            var validationResult = await _noteValidator.ValidateAsync(noteDto);
            if (validationResult.IsValid is false)
            {
                throw new ValidationException(validationResult.ToString(), validationResult.Errors);
            }

            bool patient = await _patientService.Get(noteDto.PatientId);
            if (patient is false)
            {
                throw new KeyNotFoundException($"{noteDto.PatientId}");
            }
            var note = noteDto.Adapt<Note>();

            return _noteRepository.Create(note);
        }

        public void Update(string id, NoteDto noteDto)
        {
            if (noteDto is null)
            {
                throw new ArgumentNullException(nameof(NoteDto));
            }

            var note = noteDto.Adapt<Note>();
            _noteRepository.Update(id, note);
        }

        public void Remove(string id)
        {
            var noteDto = Get(id);
            if (noteDto is null)
            {
                throw new KeyNotFoundException($"{id}");
            }
            _noteRepository.Remove(id);
        }
    }
}
