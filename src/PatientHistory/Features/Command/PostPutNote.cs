using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using PatientHistory.Data;
using PatientHistory.Data.DTO;
using PatientHistory.Infrastructure.Services;

namespace PatientHistory.Features.Command
{
    public class PostPutNote
    {
        public record Command(NoteDto NotedDto, string Id = "0") : IRequest<NoteDto>;

        public class Handler : IRequestHandler<Command, NoteDto>
        {
            private readonly INoteService _noteService;

            public Handler(INoteService noteService)
            {
                _noteService = noteService;
            }

            public async Task<NoteDto> Handle(Command command, CancellationToken cancellationToken)
            {
                PostPut(command);
                return await Task.FromResult(_noteService.Get(command.Id));
            }

            public void PostPut(Command command)
            {
                (var notedDto, string id) = command;
                var note = notedDto.Adapt<Note>();
                switch (id)
                {
                    case "0":
                        note.CreatedTime = DateTime.Now;
                        _noteService.Create(note);
                        break;
                    default:
                        _noteService.Update(id, note);
                        break;
                }
            }
        }
    }
}
