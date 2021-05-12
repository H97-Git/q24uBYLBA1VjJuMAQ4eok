using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PatientAssessment.Data;
using PatientAssessment.Features.Queries;
using PatientAssessment.Infrastructure.Services;
using Xunit;

namespace PatientAssessment.Tests.Features.Handlers
{
    public class HandlerTests
    {
        private readonly IAssessmentService _assessmentService;

        public HandlerTests()
        {
            var assessment = new Assessment() { GivenName = "Unit", FamilyName = "Test" };

            _assessmentService = Substitute.For<IAssessmentService>();
            _assessmentService.GetAssessmentByPatientId(Arg.Any<int>()).Returns(assessment);
        }

        [Fact]
        public async Task GetAssessmentByPatientId_ShouldReturnAssessment()
        {
            // Arrange
            int id = new Random().Next();
            var query = new GetAssessmentByPatientId.Query(id);
            var handler = new GetAssessmentByPatientId.Handler(_assessmentService);

            // Act
            var response = await handler.Handle(query, new CancellationToken());

            // Assert
            response.Assessment.GivenName.Should().Be("Unit");
            response.Assessment.FamilyName.Should().Be("Test");
        }
    }
}
