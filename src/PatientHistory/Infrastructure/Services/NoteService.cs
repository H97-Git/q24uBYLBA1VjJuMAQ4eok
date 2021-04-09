using PatientHistory.Data;
using PatientHistory.Data.DTO;
using PatientHistory.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;

namespace PatientHistory.Infrastructure.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IPatientService _patientService;

        public NoteService(INoteRepository noteRepository, IPatientService patientService)
        {
            _noteRepository = noteRepository;
            _patientService = patientService;
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
            if (notesByPatient.Count is 0)
            {
                throw new KeyNotFoundException($"{patientId}");
            }

            return notesByPatient;
        }

        public async Task Create(Note note)
        {
            bool patient = await _patientService.Get(note.PatientId);
            if (!patient)
            {
                throw new KeyNotFoundException($"{note.PatientId}");
            }
            _noteRepository.Create(note);
        }

        public void Update(string id, Note note) =>
            _noteRepository.Update(id, note);

        public void Remove(string id) =>
            _noteRepository.Remove(id);
    }
}
