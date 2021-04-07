using System.Collections.Generic;
using MongoDB.Driver;
using PatientHistory.Data;

namespace PatientHistory.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IMongoCollection<Note> _notes;

        public NoteRepository(INoteDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _notes = database.GetCollection<Note>(settings.NotesCollectionName);
        }

        public List<Note> Get() =>
             _notes.Find(n => true).ToList();

        public Note Get(string id) =>
            _notes.Find(n => n.Id == id).FirstOrDefault();

        public Note Create(Note note)
        {
            _notes.InsertOne(note);
            return note;
        }

        public void Update(string id, Note note)
        {
            _notes.ReplaceOne(n => n.Id == id, note);
        }

        public void Remove(string id) =>
            _notes.DeleteOne(n => n.Id == id);
    }
}
