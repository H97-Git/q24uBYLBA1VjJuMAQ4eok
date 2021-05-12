using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PatientHistory.Infrastructure.Services;
using Xunit;

namespace PatientHistory.Tests.Infrastructure.Services
{
    public class PatientServiceTests
    {
        private readonly IPatientService _sut;

        public PatientServiceTests()
        {
            _sut = Substitute.For<IPatientService>();
        }

        [Fact]
        public async Task Get_ShouldReturn_IsSuccessStatusCode()
        {
            // Arrange
            _sut.Get(Arg.Any<int>()).Returns(true);
            // Act
            bool isSuccessStatusCode = await _sut.Get(new Random().Next());
            // Assert
            isSuccessStatusCode.Should().BeTrue();
        }
    }
}
