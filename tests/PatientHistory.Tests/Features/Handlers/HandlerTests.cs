using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PatientHistory.Data.DTO;
using PatientHistory.Features.Command;
using PatientHistory.Features.Queries;
using PatientHistory.Infrastructure.Services;
using Xunit;

namespace PatientHistory.Tests.Features.Handlers
{
    public class HandlerTests
    {
        private readonly INoteService _noteService;
        private readonly NoteDto _note;
        public HandlerTests()
        {
            _note = new NoteDto() { Id = "99",PatientId = 99};
            var list = new List<NoteDto> { _note };

            _noteService = Substitute.For<INoteService>();
            _noteService.Get().Returns(list);
            _noteService.Get(Arg.Any<string>()).Returns(_note);
            _noteService.GetByPatientId(Arg.Any<int>()).Returns(list);
        }

        [Fact]
        public async Task GetAllPatient()
        {
            // Arrange
            var query = new GetAllNote.Query();
            var handler = new GetAllNote.Handler(_noteService);

            // Act
            var response = await handler.Handle(query, new CancellationToken());

            // Assert
            response.NotesDto.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetById()
        {
            // Arrange
            var query = new GetNoteById.Query("99");
            var sut = new GetNoteById.Handler(_noteService);

            // Act
            var response = await sut.Handle(query, new CancellationToken());

            // Assert
            response.NoteDto.Id.Should().Be("99");
        }

        [Fact]
        public async Task GetPatientById()
        {
            // Arrange
            var query = new GetNoteByPatientId.Query(99);
            var sut = new GetNoteByPatientId.Handler(_noteService);

            // Act
            var response = await sut.Handle(query, new CancellationToken());

            // Assert
            response.NotesDto.Should().OnlyContain(x => x.PatientId == 99);
        }

        [Fact]
        public async Task PostPatient()
        {
            // Arrange
            var command = new PostPutNote.Command(_note);
            var sut = new PostPutNote.Handler(_noteService);

            // Act
            var response = await sut.Handle(command, new CancellationToken());

            // Assert
            response.Id.Should().Be("99");
        }

        [Fact]
        public async Task PutPatient()
        {
            // Arrange
            var command = new PostPutNote.Command(_note);
            var sut = new PostPutNote.Handler(_noteService);

            // Act
            var response = await sut.Handle(command, new CancellationToken());

            // Assert
            response.Id.Should().Be("99");
        }
    }
}
