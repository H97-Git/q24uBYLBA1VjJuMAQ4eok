using MediatR;
using PatientHistory.Data.DTO;
using PatientHistory.Infrastructure.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                string id = await PostPut(command);
                return _noteService.Get(id);
            }

            public async Task<string> PostPut(Command command)
            {
                (var notedDto, string id) = command;
                switch (id)
                {
                    //If id (command.id) is "0" it's a save
                    case "0":
                        notedDto.CreatedTime = DateTime.Now;
                        return await _noteService.Create(notedDto);
                    //otherwise it's an update.
                    default:
                        _noteService.Update(id, notedDto);
                        return id;
                }
            }
        }
    }
}
