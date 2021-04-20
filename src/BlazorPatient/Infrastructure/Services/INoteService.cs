using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPatient.DTO;

namespace BlazorPatient.Infrastructure.Services
{
    public interface INoteService
    {
        //Task<List<NoteDto>> Get();
        Task<List<NoteDto>> GetByPatientId(int patientId);
        Task<int> Save(NoteDto noteDto);
        public string ErrorMessage { get; set; }
    }
}