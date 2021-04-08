using System;
using FluentAssertions;
using NSubstitute;
using PatientHistory.Data;
using PatientHistory.Data.DTO;
using PatientHistory.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute.ReturnsExtensions;
using PatientHistory.Infrastructure.Repositories;
using Xunit;

namespace PatientHistory.Tests.Infrastructure.Services
{
    public class NoteServiceTests
    {
        private readonly INoteService _sut;
        private readonly INoteRepository _noteRepository;
        private readonly IPatientService _patientService;

        public NoteServiceTests()
        {
            _patientService = Substitute.For<IPatientService>();

            _noteRepository = Substitute.For<INoteRepository>();
            _noteRepository.Get().Returns(new List<Note>());

            _sut = new NoteService(_noteRepository,_patientService);
        }

        [Fact]
        public void GetNotes_ShouldReturnList()
        {
            // Arrange
            _noteRepository.Get().Returns(new List<Note>());

            // Act
            var notes = _sut.Get();

            // Assert
            notes.Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetNotesById_ShouldReturnNote()
        {
            // Arrange
            const string testedIdString = "Unit Test";
            _noteRepository.Get(Arg.Any<string>()).Returns(new Note{Id = testedIdString});

            // Act
            var notes = _sut.Get("");

            // Assert
            notes.Id.Should().NotBeNullOrEmpty("Unit Test");
        }

        [Fact]
        public void CreateNote_ShouldReturnNote()
        {
            // Arrange
            int counter = 0;
            _noteRepository.When(x => x.Create(Arg.Any<Note>())).Do(x => counter++);
            _patientService.Get(Arg.Any<int>()).Returns(true);
            // Act
            _sut.Create(new Note());

            // Assert
            _noteRepository.Received().Create(Arg.Any<Note>());
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdateNote_ShouldReturnNote()
        {
            // Arrange
            int counter = 0;
            _noteRepository.When(x => x.Update("",Arg.Any<Note>())).Do(x => counter++);

            // Act
            _sut.Update("",new Note());

            // Assert
            _noteRepository.Received().Update("",Arg.Any<Note>());
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteNote_ShouldReturnNote()
        {
            // Arrange
            int counter = 0;
            _noteRepository.When(x => x.Remove(Arg.Any<string>())).Do(x => counter++);

            // Act
            _sut.Remove("");

            // Assert
            _noteRepository.Received().Remove("");
            counter.Should().Be(1);
        }

        [Fact]
        public void GetNotesId_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _noteRepository.Get(Arg.Any<string>()).ReturnsNull();

            // Act
            Func<NoteDto> act = () => _sut.Get("Unit Test");

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage($"Unit Test");
        }

        [Fact]
        public void GetNotesPatientId_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _noteRepository.Get(Arg.Any<string>()).ReturnsNull();
            int r = new Random().Next();
            // Act
            Func<List<NoteDto>> act = () => _sut.GetByPatientId(r);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage($"{r}");
        }

        [Fact]
        public void CreateNotes_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            _noteRepository.Get(Arg.Any<string>()).ReturnsNull();
            _patientService.Get(Arg.Any<int>()).Returns(false);
            var note = new Note{PatientId = 99};

            // Act
            Func<Task> act = async () => await _sut.Create(note);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage($"{note.PatientId}");
        }
    }
}
