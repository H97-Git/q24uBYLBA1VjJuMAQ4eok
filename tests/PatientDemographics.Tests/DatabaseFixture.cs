using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PatientDemographics.Data;
using PatientDemographics.Tests.Properties;
using System;
using Xunit;

namespace PatientDemographics.Tests
{
    [CollectionDefinition("SharedDbContext")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

    public class DatabaseFixture : IDisposable
    {

        public PatientContext PatientContext;
        public PatientContext InMemoryContext;

        public DatabaseFixture()
        {
            // Test for real database READ
            var contextOptions = new DbContextOptionsBuilder<PatientContext>()
                .UseSqlServer(Resources.ConnectionString)
                .Options;
            //// Test InMemory CREATE UPDATE DELETE
            var inMemoryContextOptions = DbContextOptionsBuilder();

            PatientContext = new PatientContext(contextOptions);
            InMemoryContext = new PatientContext(inMemoryContextOptions);

            SeedInMemoryDb();
        }

        private static DbContextOptions<PatientContext> DbContextOptionsBuilder() =>
            new DbContextOptionsBuilder<PatientContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

        private void SeedInMemoryDb()
        {
            var faker = new Faker<Patient>()
                .Rules((f, o) =>
                {
                    o.FamilyName = f.Person.LastName;
                    o.GivenName = f.Person.FirstName;
                    o.Gender = f.PickRandom<Gender>();
                    o.DateOfBirth = f.Person.DateOfBirth;
                    o.HomeAddress =
                        $"{f.Person.Address.Street} - {f.Person.Address.City} - {f.Person.Address.State} - {f.Person.Address.ZipCode}";
                    o.PhoneNumber = f.Person.Phone;
                });

            var patients = new[]
            {
                faker.Generate(),
                faker.Generate(),
                faker.Generate(),
                faker.Generate(),
                faker.Generate()
            };

            foreach (var patient in patients)
            {
                InMemoryContext.Patients.AddRange(patient);
            }

            InMemoryContext.SaveChanges();
        }

        public void Dispose()
        {
            PatientContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
