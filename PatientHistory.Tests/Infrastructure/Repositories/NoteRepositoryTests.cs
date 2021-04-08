using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
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
            //var dbSettings = new NoteDbSettings
            //{
            //    NotesCollectionName = "Notes",
            //    ConnectionString = "mongodb://localhost:27017",
            //    DatabaseName = "NotesDbTest"
            //};
            //_sut = Substitute.For<NoteRepository>(dbSettings);
            _sut = new NoteRepository(Substitute.For<NoteDbSettings>());
        }

        [Fact]
        public void GetNotes_ShouldReturnList()
        {
            // Arrange
            var t = new List<Note>();
            _sut.Get().Returns(t);

            // Act
            var notes = _sut.Get();

            // Assert
            notes.Should().BeEmpty();

        }

        //[Fact]
        //public void GetNotesById_ShouldReturnNote()
        //{
        //    // Arrange
        //    const string testedIdString = "Unit Test";
        //    _sut.Get(Arg.Any<string>()).Returns(new Note{Id = testedIdString});

        //    // Act
        //    var notes = _sut.Get("");

        //    // Assert
        //    notes.Id.Should().NotBeNullOrEmpty("Unit Test");
        //}

        //[Fact]
        //public void CreateNote_ShouldReturnNote()
        //{
        //    // Arrange
        //    _sut.Create(Arg.Any<Note>()).Returns(new Note{Id = "Unit Test"});

        //    // Act
        //    var notes = _sut.Create(new Note());

        //    // Assert
        //    notes.Id.Should().NotBeNullOrEmpty("Unit Test");
        //}

        //[Fact]
        //public void UpdateNote_ShouldReturnNote()
        //{
        //    // Arrange
        //    int counter = 0;
        //    _sut.When(x => x.Update("",Arg.Any<Note>())).Do(x => counter++);

        //    // Act
        //    _sut.Update("",new Note());

        //    // Assert
        //    _sut.Received().Update("",Arg.Any<Note>());
        //    counter.Should().Be(1);
        //}

        //[Fact]
        //public void DeleteNote_ShouldReturnNote()
        //{
        //    // Arrange
        //    int counter = 0;
        //    _sut.When(x => x.Remove(Arg.Any<string>())).Do(x => counter++);

        //    // Act
        //    _sut.Remove("");

        //    // Assert
        //    _sut.Received().Remove("");
        //    counter.Should().Be(1);
        //}
    }
}
