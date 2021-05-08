using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PatientDemographics.Data;
using PatientDemographics.Data.DTO;
using PatientDemographics.Infrastructure.Repositories;
using PatientDemographics.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Gender = PatientDemographics.Data.DTO.Gender;

namespace PatientDemographics.Tests.Infrastructure.Services
{
    public class PatientServicesTest
    {
        private readonly PatientService _sut;
        private readonly IPatientRepository _patientRepository;

        public PatientServicesTest()
        {
            _patientRepository = Substitute.For<IPatientRepository>();
            _sut = new PatientService(_patientRepository);
        }

        [Fact]
        public async Task GetPatient_ShouldReturnEmptyList_WhenExist()
        {
            // Arrange
            _patientRepository.GetPatient().Returns(new List<Patient>());

            // Act
            var patientsDto = await _sut.GetPatient();

            // Assert
            patientsDto.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPatientId_ShouldReturnPatient_WhenExist()
        {
            // Arrange
            var patient = new Patient { Id = 99, FamilyName = "Blazor", GivenName = "MediatR" };
            _patientRepository.GetPatient(patient.Id).Returns(patient);

            // Act
            var patientDto = await _sut.GetPatient(patient.Id);

            // Assert
            patientDto.Should().NotBeNull();
            patientDto.Id.Should().Be(99);
            patientDto.FamilyName.Should().Be("Blazor");
            patientDto.GivenName.Should().Be("MediatR");
        }
        
        [Fact]
        public void GetPatientId_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _patientRepository.GetPatient(Arg.Any<int>()).ReturnsNull();
            int id = new Random().Next();

            // Act
            Func<Task> act = async () => await _sut.GetPatient(id);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage($"{id}");
        }

        [Fact]
        public async Task SavePatient_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var patientDto = new PatientDto
            {
                Id = 99,
                FamilyName = "Blazor",
                GivenName = "MediatR",
                Gender = Gender.Male,
                DateOfBirth = DateTime.Now.Subtract(new TimeSpan(24, 0, 0))
            };
            _patientRepository.When(p => p.SavePatient(Arg.Any<Patient>())).Do(x => counter++);

            // Act
            await _sut.SavePatient(patientDto);

            // Assert
            await _patientRepository.Received().SavePatient(Arg.Any<Patient>());
            counter.Should().Be(1);
        }
        
        [Fact]
        public void SavePatient_ShouldThrowValidationException_WhenPatientIsNotValid()
        {
            // Arrange
            var patientDto = new PatientDto
            {
                Id = 99,
                FamilyName = "Blazor",
                Gender = Gender.Female,
                DateOfBirth = DateTime.Now.Subtract(new TimeSpan(24, 0, 0))
            };

            // Act
            Func<Task> act = async () => await _sut.SavePatient(patientDto);

            // Assert
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void SavePatient_ShouldThrowArgumentNullException_WhenPatientIsNull()
        {
            // Arrange

            // Act
            Func<Task> act = async () => await _sut.SavePatient(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePatient_ShouldCallRepo_WhenExist()
        {
            // Arrange
            var counter = 0;
            var patient = new Patient { Id = 99};
            _patientRepository.GetPatient(patient.Id).Returns(patient);
            _patientRepository.When(x => x.UpdatePatient(patient)).Do(x => counter++);

            // Act
            var patientDto = new PatientDto
            {
                Id = 99,
                FamilyName = "FamilyName",
                GivenName = "GivenName",
                Gender = Gender.Female,
                DateOfBirth = DateTime.Now.Subtract(new TimeSpan(24, 0, 0))
            };
             await _sut.UpdatePatient(99, patientDto);

            // Assert
            await _patientRepository.Received().UpdatePatient(patient);
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdatePatient_ShouldThrowArgumentNullException_WhenPatientIsNull()
        {
            // Arrange
            
            // Act
            Func<Task> act = async () => await _sut.UpdatePatient(new Random().Next(), null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage($"Value cannot be null. (Parameter '{nameof(PatientDto)}')" );
        }

        [Fact]
        public void UpdatePatient_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var patientDto = new PatientDto
            {
                Id = 99,
                FamilyName = "Blazor",
                GivenName = "MediatR",
                Gender = Gender.Female,
                DateOfBirth = DateTime.Now.Subtract(new TimeSpan(24, 0, 0))
            };

            // Act
            Func<Task> act = async () => await _sut.UpdatePatient(new Random().Next(), patientDto);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage($"{patientDto.Id}");
        }

        [Fact]
        public void UpdatePatient_ShouldThrowValidationException_WhenPatientIsNotValid()
        {
            // Arrange
            var patientDto = new PatientDto
            {
                Id = 99,
                FamilyName = "Blazor",
                Gender = Gender.Female,
                DateOfBirth = DateTime.Now.Subtract(new TimeSpan(24, 0, 0))
            };

            // Act
            Func<Task> act = async () => await _sut.UpdatePatient(new Random().Next(),patientDto);

            // Assert
            act.Should().Throw<ValidationException>();
        }
    }
}
