using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientHistory.Data.DTO;
using PatientHistory.Infrastructure.Services;

namespace PatientHistory.Features.Queries
{
    public class GetNoteByPatientId
    {
        public record Query(int PatientId) : IRequest<Response>;

        public class Handler: IRequestHandler<Query,Response>
        {
            private readonly INoteService _noteService;

            public Handler(INoteService noteService)
            {
                _noteService = noteService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                var response = new Response(_noteService.GetByPatientId(query.PatientId));
                return await Task.FromResult(response);
            }
        }

        public record Response(List<NoteDto> NotesDto);
    }
}
