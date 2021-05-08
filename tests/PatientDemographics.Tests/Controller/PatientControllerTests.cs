using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PatientDemographics.Controllers;
using PatientDemographics.Data.DTO;
using PatientDemographics.Features.Commands;
using PatientDemographics.Features.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PatientDemographics.Tests.Controller
{
    public class PatientControllerTests
    {
        private readonly PatientController _sut;
        private readonly IMediator _mediator;
        public PatientControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new PatientController(_mediator);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListPatient()
        {
            // Arrange
            var patientDto = new PatientDto { Id = 99 };
            _mediator.Send(Arg.Any<GetAllPatient.Query>())
                .Returns(new GetAllPatient.Response(new List<PatientDto> { patientDto }));

            // Act
            var actionResult = await _sut.GetAllAsync();

            // Assert
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is List<PatientDto> value)
                {
                    value.Should().HaveCount(1);
                }
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatient_WhenExist()
        {
            // Arrange
            int r = new Random().Next();
            _mediator.Send(Arg.Any<GetPatientById.Query>())
                .Returns(new GetPatientById.Response(new PatientDto { Id = r, FamilyName = "Blazor" }));

            // Act
            var actionResult = await _sut.GetByIdAsync(r);

            // Assert
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is PatientDto value)
                {
                    value.Id.Should().Be(r);
                    value.FamilyName.Should().Be("Blazor");
                }
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PostAsync_ShouldAddEntity(bool body)
        {
            // Arrange
            var patientDto = new PatientDto { Id = 99 };
            _mediator.Send(Arg.Any<PostPatient.Command>()).Returns(patientDto);

            // Act
            var command = new PostPatient.Command(patientDto);
            var actionResult = body ? await _sut.PostAsync(command) : await _sut.PostBodyAsync(command);

            // Assert
            if (actionResult.Result is CreatedAtActionResult result)
            {
                result.StatusCode.Should().Be(201);
                if (result.Value is PatientDto value)
                {
                    value.Id.Should().Be(99);
                }
            }
        }
        

        [Fact]
        public async Task PutAsync_ShouldUpdateEntity()
        {
            // Arrange
            var patientDto = new PatientDto { Id = 99 };
            _mediator.Send(Arg.Any<PutPatient.Command>()).Returns(patientDto);

            // Act
            var actionResult = await _sut.PutAsync(new Random().Next(),patientDto);

            // Assert
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is PatientDto value)
                {
                    value.Id.Should().Be(99);
                }
            }
        }
    }
}
