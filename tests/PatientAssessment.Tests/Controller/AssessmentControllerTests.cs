using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PatientAssessment.Controllers;
using PatientAssessment.Data;
using PatientAssessment.Features.Queries;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PatientAssessment.Tests.Controller
{
    public class AssessmentControllerTests
    {
        private readonly AssessmentController _sut;
        private readonly IMediator _mediator;
        public AssessmentControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new AssessmentController(_mediator);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListPatient()
        {
            // Arrange
            var assessment = new Assessment() { GivenName = "Unit", FamilyName = "Test" };
            _mediator.Send(Arg.Any<GetAssessmentByPatientId.Query>())
                .Returns(new GetAssessmentByPatientId.Response(assessment));

            // Act
            var actionResult = await _sut.GetByIdAsync(new Random().Next());

            // Assert
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is Assessment value)
                {
                    value.GivenName.Should().Be("Unit");
                    value.FamilyName.Should().Be("Test");
                }
            }
        }
    }
}
