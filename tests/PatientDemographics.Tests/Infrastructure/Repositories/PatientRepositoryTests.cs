using System.Collections.Generic;
using FluentAssertions;
using PatientDemographics.Data;
using PatientDemographics.Infrastructure.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace PatientDemographics.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class PatientRepositoryTests
    {
        private readonly DatabaseFixture _fixture;
        private const string FamilyName = "Blazor";
        public PatientRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetPatient_ShouldReturnList_WhenListExist()
        {
            // Arrange
            var sut = new PatientRepository(_fixture.PatientContext);

            // Act
            var patients = await sut.GetPatient();

            // Assert
            patients.Should().BeOfType<List<Patient>>();
        }
        [Fact]
        public async Task GetPatient_ShouldReturnPatient_WhenDoesExist()
        {
            // Arrange
            var sut = new PatientRepository(_fixture.InMemoryContext);

            // Act
            var patient = await sut.GetPatient(1);

            // Assert
            patient.Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public async Task GetPatient_ShouldReturnNull_WhenDoesNotExist(int id)
        {
            // Arrange
            var sut = new PatientRepository(_fixture.PatientContext);

            // Act
            var patient = await sut.GetPatient(id);

            // Assert
            patient.Should().BeNull();
        }

        [Fact]
        public async Task CreatePatient_ShouldAddEntity()
        {
            //Arrange
            var sut = new PatientRepository(_fixture.InMemoryContext);

            //Act
            await sut.SavePatient(new Patient { FamilyName = FamilyName });
            var patient = await sut.GetPatient(6);

            //Assert
            patient.Should().NotBeNull();
            patient.FamilyName.Should().Be(FamilyName);
        }

        [Fact]
        public async Task UpdatePatient_ShouldChangeEntity()
        {
            //Arrange
            var sut = new PatientRepository(_fixture.InMemoryContext);
            var patient = await sut.GetPatient(1);
            patient.FamilyName = FamilyName;

            //Act
            await sut.UpdatePatient(patient);
            patient = await sut.GetPatient(1);

            //Assert
            patient.Should().NotBeNull();
            patient.FamilyName.Should().Be(FamilyName);
        }
    }
}
