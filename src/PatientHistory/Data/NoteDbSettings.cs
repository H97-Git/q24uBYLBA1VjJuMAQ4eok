namespace PatientHistory.Data
{
    public class NoteDbSettings : INoteDbSettings
    {
        public string NotesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface INoteDbSettings
    {
        public string NotesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
