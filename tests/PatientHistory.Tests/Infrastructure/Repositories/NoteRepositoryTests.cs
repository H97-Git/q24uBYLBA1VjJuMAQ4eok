using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using PatientHistory.Data;
using PatientHistory.Infrastructure.Repositories;
using Xunit;

namespace PatientHistory.Tests.Infrastructure.Repositories
{
    public class NoteRepositoryTests
    {
        private readonly INoteRepository _sut;

        public NoteRepositoryTests()
        {
            _sut = Substitute.For<INoteRepository>();
        }

        [Fact]
        public void GetNotes_ShouldReturnList()
        {
            // Arrange
            _sut.Get().Returns(new List<Note>());

            // Act
            var notes = _sut.Get();

            // Assert
            notes.Should().BeEmpty();

        }

        [Fact]
        public void GetNotesById_ShouldReturnNote()
        {
            // Arrange
            const string testedIdString = "Unit Test";
            _sut.Get(Arg.Any<string>()).Returns(new Note { Id = testedIdString });

            // Act
            var notes = _sut.Get("");

            // Assert
            notes.Id.Should().NotBeNullOrEmpty("Unit Test");
        }

        [Fact]
        public void CreateNote_ShouldReturnNote()
        {
            // Arrange
            var noteToCreate = new Note() {Id = "Unit Test"};
            _sut.Create(Arg.Any<Note>()).Returns(noteToCreate.Id);

            // Act
            var note = _sut.Create(noteToCreate);

            // Assert
            note.Should().Be("Unit Test");
        }

        [Fact]
        public void UpdateNote_ShouldReturnNote()
        {
            // Arrange
            var counter = 0;
            _sut.When(x => x.Update("", Arg.Any<Note>())).Do(x => counter++);

            // Act
            _sut.Update("", new Note());

            // Assert
            _sut.Received().Update("", Arg.Any<Note>());
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteNote_ShouldReturnNote()
        {
            // Arrange
            var counter = 0;
            _sut.When(x => x.Remove(Arg.Any<string>())).Do(x => counter++);

            // Act
            _sut.Remove("");

            // Assert
            _sut.Received().Remove("");
            counter.Should().Be(1);
        }
    }
}
