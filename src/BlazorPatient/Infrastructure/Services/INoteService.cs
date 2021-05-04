using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPatient.Models;
using Microsoft.Extensions.Configuration;

namespace BlazorPatient.Infrastructure.Services
{
    public interface INoteService
    {
        Task<List<NoteModel>> GetByPatientId(int patientId);
        Task<int> Save(NoteModel Note);
        public string ErrorMessage { get; set; }
        public IConfiguration Configuration { get; }
    }
}