using System;
using System.Collections.Generic;
using MongoDB.Driver;
using PatientHistory.Data;
using Serilog;

namespace PatientHistory.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteContext _context;
        public NoteRepository(NoteContext context)
        {
            _context = context;
        }

        public List<Note> Get() =>
             _context.Notes.Find(n => true).ToList();

        public Note Get(string id) =>
            _context.Notes.Find(n => n.Id == id).FirstOrDefault();

        public string Create(Note note)
        {
            try
            {
                _context.Notes.InsertOne(note);
            }
            catch (Exception ex)
            {
                Log.Information(ex.Message);
                throw;
            }

            return note.Id;
        }

        public void Update(string id, Note note)
        {
            _context.Notes.ReplaceOne(n => n.Id == id, note);
        }

        public void Remove(string id) =>
            _context.Notes.DeleteOne(n => n.Id == id);
    }
}
