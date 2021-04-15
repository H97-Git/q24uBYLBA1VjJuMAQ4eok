using System.Collections.Generic;
using System.Threading.Tasks;
using PatientHistory.Data;
using PatientHistory.Data.DTO;

namespace PatientHistory.Infrastructure.Services
{
    public interface INoteService
    {
        List<NoteDto> Get();
        List<NoteDto> GetByPatientId(int patientId);
        NoteDto Get(string id);
        Task Create(NoteDto note);
        void Update(string id, NoteDto note);
        void Remove(string id);
    }
}