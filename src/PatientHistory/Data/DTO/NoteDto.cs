using System;

namespace PatientHistory.Data.DTO
{
    public class NoteDto
    {
        public string Id { get; set; }

        public int PatientId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
