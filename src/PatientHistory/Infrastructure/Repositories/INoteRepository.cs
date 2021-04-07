using System.Collections.Generic;
using PatientHistory.Data;

namespace PatientHistory.Infrastructure.Repositories
{
    public interface INoteRepository
    {
        List<Note> Get();
        Note Get(string id);
        Note Create(Note note);
        void Update(string id, Note note);
        void Remove(string id);
    }
}