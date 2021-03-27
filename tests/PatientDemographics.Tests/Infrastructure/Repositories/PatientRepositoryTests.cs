using FluentAssertions;
using PatientDemographics.Infrastructure.Repositories;
using System.Threading.Tasks;
using PatientDemographics.Models;
using Xunit;

namespace PatientDemographics.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class PatientRepositoryTests
    {
        private readonly DatabaseFixture _fixture;
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
            patients.Count.Should().Be(6);
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
        public async Task GetBid_ShouldReturnNull_WhenDoesNotExist(int id)
        {
            // Arrange
            var sut = new PatientRepository(_fixture.PatientContext);

            // Act
            var patient = await sut.GetPatient(id);

            // Assert
            patient.Should().BeNull();
        }

        [Fact]
        public async Task CreateBid_ShouldAddEntity()
        {
            //Arrange
            var sut = new PatientRepository(_fixture.InMemoryContext);

            //Act
            await sut.SavePatient(new Patient { FamilyName = "Unit Test"});
            var patient = await sut.GetPatient(6);

            //Assert
            patient.Should().NotBeNull();
            patient.FamilyName.Should().Be("Unit Test");
        }

        [Fact]
        public async Task UpdateBid_ShouldChangeEntity()
        {
            //Arrange
            var sut = new PatientRepository(_fixture.InMemoryContext);
            var patient = await sut.GetPatient(1);
            patient.FamilyName = "Unit Test";

            //Act
            await sut.UpdatePatient(patient);
            patient = await sut.GetPatient(1);
            //Assert
            patient.Should().NotBeNull();
            patient.FamilyName.Should().Be("Unit Test");
        }
    }
}
