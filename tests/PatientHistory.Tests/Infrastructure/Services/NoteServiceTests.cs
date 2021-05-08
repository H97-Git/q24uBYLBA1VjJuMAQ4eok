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
        public void GetNotesByPatientId_ShouldReturnList()
        {
            // Arrange
            _noteRepository.Get().Returns(new List<Note>());

            // Act
            var notes = _sut.GetByPatientId(Arg.Any<int>());

            // Assert
            notes.Should().BeOfType<List<NoteDto>>();
        }

        [Fact]
        public void CreateNote_ShouldReturnNote()
        {
            // Arrange
            int counter = 0;
            var noteDto = new NoteDto {Id = "randomString", Message = "Unit Test is cool!"};
            _noteRepository.When(x => x.Create(Arg.Any<Note>())).Do(x => counter++);
            _patientService.Get(Arg.Any<int>()).Returns(true);
            // Act
            _sut.Create(noteDto);

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
            _sut.Update("",new NoteDto());

            // Assert
            _noteRepository.Received().Update("",Arg.Any<Note>());
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteNote_ShouldReturnNote()
        {
            // Arrange
            int counter = 0;
            const string received = "Unit Test is cool!";
            _noteRepository.Get(Arg.Any<string>()).Returns(new Note());
            _noteRepository.When(x => x.Remove(Arg.Any<string>())).Do(x => counter++);
            
            // Act
            _sut.Remove(received);

            // Assert
            _noteRepository.Received().Remove(received);
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
        public void CreateNotes_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            _noteRepository.Get(Arg.Any<string>()).ReturnsNull();
            _patientService.Get(Arg.Any<int>()).Returns(false);
            var note = new NoteDto{PatientId = 99,Message = "Unit Test is cool."};

            // Act
            Func<Task> act = async () => await _sut.Create(note);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage($"{note.PatientId}");
        }

        [Fact]
        public void CreateNotes_ShouldThrowArgumentNullException_WhenNoteIsNull()
        {
            // Arrange

            // Act
            Func<Task> act = async () => await _sut.Create(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateNotes_ShouldThrowArgumentNullException_WhenNoteIsNull()
        {
            // Arrange

            // Act
            Action act = () => _sut.Update("",null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
