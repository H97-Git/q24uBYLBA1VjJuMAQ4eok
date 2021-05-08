using FluentAssertions;
using NSubstitute;
using PatientAssessment.Data;
using PatientAssessment.Infrastructure.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PatientAssessment.Tests.Infrastructure.Services
{
    public class AssessmentServicesTest
    {
        private readonly IAssessmentService _sut;

        public AssessmentServicesTest()
        {
            _sut = Substitute.For<IAssessmentService>();
        }

        [Fact]
        public async Task GetAssessmentByPatientId_ShouldReturnAssessment_WhenPatientExist()
        {
            // Arrange
            _sut.GetAssessmentByPatientId(Arg.Any<int>()).Returns(new Assessment());

            // Act
            int id = new Random().Next();
            var assessment = await _sut.GetAssessmentByPatientId(id);

            // Assert
            assessment.Should().NotBe(null);
        }
    }
}
