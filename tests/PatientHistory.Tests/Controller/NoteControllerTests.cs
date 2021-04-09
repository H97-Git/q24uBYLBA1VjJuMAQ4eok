using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PatientHistory.Controllers;
using PatientHistory.Data.DTO;
using PatientHistory.Features.Command;
using PatientHistory.Features.Queries;
using Xunit;

namespace PatientHistory.Tests.Controller
{
    public class NoteControllerTests
    {
        private readonly NoteController _sut;
        private readonly IMediator _mediator;
        public NoteControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new NoteController(_mediator);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListNote()
        {
            // Arrange
            var noteDto = new NoteDto { Id = "99" };
            _mediator.Send(Arg.Any<GetAllNote.Query>())
                .Returns(new GetAllNote.Response(new List<NoteDto> { noteDto }));

            // Act
            var actionResult = await _sut.GetAllAsync();

            // Assert
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is List<NoteDto> value)
                {
                    value.Should().HaveCount(1);
                }
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNote_WhenExist()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetNoteById.Query>())
                .Returns(new GetNoteById.Response(new NoteDto { Id = "Unit Test"}));

            // Act
            var actionResult = await _sut.GetByIdAsync("Unit Test");

            // Assert
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is NoteDto value)
                {
                    value.Id.Should().Be("Unit Test");
                }
            }
        }

        [Fact]
        public async Task PostAsync_ShouldAddEntity()
        {
            // Arrange
            var noteDto = new NoteDto{Id = "Unit Test"};
            _mediator.Send(Arg.Any<PostPutNote.Command>()).Returns(noteDto);

            // Act
            var command = new PostPutNote.Command(noteDto);
            var actionResult = await _sut.PostAsync(new NoteDto());

            // Assert
            if (actionResult.Result is CreatedAtActionResult result)
            {
                result.StatusCode.Should().Be(201);
                if (result.Value is NoteDto value)
                {
                    value.Id.Should().Be("Unit Test");
                }
            }
        }

        [Fact]
        public async Task PutAsync_ShouldUpdateEntity()
        {
            // Arrange
            var noteDto = new NoteDto { Id = "99" };
            _mediator.Send(Arg.Any<PostPutNote.Command>()).Returns(noteDto);

            // Act
            var command = new PostPutNote.Command(noteDto);
            var actionResult = await _sut.PutAsync("99",noteDto);

            // Assert
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is NoteDto value)
                {
                    value.Id.Should().Be("99");
                }
            }
        }
    }
}
