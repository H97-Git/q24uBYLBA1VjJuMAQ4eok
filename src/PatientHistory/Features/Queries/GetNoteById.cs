using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PatientHistory.Data.DTO;
using PatientHistory.Infrastructure.Services;

namespace PatientHistory.Features.Queries
{
    public class GetNoteById
    {
        public record Query(string Id) : IRequest<Response>;

        public class Handler : IRequestHandler<Query,Response>
        {
            private readonly INoteService _noteService;

            public Handler(INoteService noteService)
            {
                _noteService = noteService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                var response = new Response(_noteService.Get(query.Id));
                return await Task.FromResult(response);
            }
        }

        public record Response(NoteDto NoteDto);
    }
}
