using System;

namespace PatientAssessment.Data
{
    public class NoteModel
    {
        public string Id { get; set; }

        public int PatientId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
