using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPatient.DTO;

namespace BlazorPatient.Infrastructure.Services
{
    public interface INoteService
    {
        Task<List<NoteDto>> Get();
        Task<int> Save(NoteDto noteDto);
    }
}