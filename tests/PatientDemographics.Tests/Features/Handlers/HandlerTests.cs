using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PatientDemographics.Data.DTO;
using PatientDemographics.Features.Commands;
using PatientDemographics.Features.Queries;
using PatientDemographics.Infrastructure.Services;
using Xunit;

namespace PatientDemographics.Tests.Features.Handlers
{
    public class HandlerTests
    {
        private readonly IPatientService _patientService;
        private readonly PatientDto _patient;
        public HandlerTests()
        {
            _patient = new PatientDto() { Id = 99 };
            var list = new List<PatientDto> { _patient };

            _patientService = Substitute.For<IPatientService>();
            _patientService.GetPatient().Returns(list);
            _patientService.GetPatient(Arg.Any<int>()).Returns(_patient);
        }

        [Fact]
        public async Task GetAllPatient()
        {
            // Arrange
            var query = new GetAllPatient.Query();
            var handler = new GetAllPatient.Handler(_patientService);

            // Act
            var response = await handler.Handle(query, new CancellationToken());

            // Assert
            response.PatientsDto.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetPatientById()
        {
            // Arrange
            var query = new GetPatientById.Query(99);
            var sut = new GetPatientById.Handler(_patientService);

            // Act
            var response = await sut.Handle(query, new CancellationToken());

            // Assert
            response.PatientDto.Id.Should().Be(99);
        }

        [Fact]
        public async Task PostPatient()
        {
            // Arrange
            var command = new PostPatient.Command(_patient);
            var sut = new PostPatient.Handler(_patientService);

            // Act
            var response = await sut.Handle(command, new CancellationToken());

            // Assert
            response.Id.Should().Be(99);
        }

        [Fact]
        public async Task PutPatient()
        {
            // Arrange
            var command = new PutPatient.Command(new Random().Next(),_patient);
            var sut = new PutPatient.Handler(_patientService);

            // Act
            var response = await sut.Handle(command, new CancellationToken());

            // Assert
            response.Id.Should().Be(99);
        }
    }
}
