using MediatR;
using PatientHistory.Data.DTO;
using PatientHistory.Infrastructure.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Serilog;

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
                try
                {
                    PostPut(command);
                }
                catch (ValidationException ex)
                {
                    Log.Information(ex.Message);
                    throw;
                }
                return await Task.FromResult(_noteService.Get(command.Id));
            }

            public void PostPut(Command command)
            {
                (var notedDto, string id) = command;
                switch (id)
                {
                    case "0":
                        notedDto.CreatedTime = DateTime.Now;
                        _noteService.Create(notedDto);
                        break;
                    default:
                        _noteService.Update(id, notedDto);
                        break;
                }
            }
        }
    }
}
