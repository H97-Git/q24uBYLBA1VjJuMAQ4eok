using MongoDB.Driver;
using System;

namespace PatientHistory.Data
{
    public class NoteContext : IDisposable
    {
        private readonly IMongoDatabase _db;
        public NoteContext(IMongoClient client, string dbName)
        {
            _db = client.GetDatabase(dbName);

        }
        public virtual IMongoCollection<Note> Notes =>
            _db.GetCollection<Note>("Notes");

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
