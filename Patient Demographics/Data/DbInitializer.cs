using System.Linq;
using Bogus;
using Patient_Demographics.Models;
using Serilog;

namespace Patient_Demographics.Data
{
    public class DbInitializer
    {
        public static void Initialize(PatientContext context)
        {
            Log.Information("DbInitializer : EnsureCreated()");
            context.Database.EnsureCreated();

            if (context.Patients.Any())
            {
                return;
            }
            var faker = new Faker<Patient>()
                .Rules((f,o) =>
                {
                    o.FamilyName = f.Person.LastName;
                    o.GivenName = f.Person.FirstName;
                    o.Gender = f.PickRandom<Gender>();
                    o.DateOfBirth = f.Person.DateOfBirth;
                    o.HomeAddress = $"{f.Person.Address.Street} - {f.Person.Address.City} - {f.Person.Address.State} - {f.Person.Address.ZipCode}";
                    o.PhoneNumber = f.Person.Phone;
                });

            Patient[] patients = new Patient[]
            {
                faker.Generate(),
                faker.Generate(),
                faker.Generate(),
                faker.Generate(),
                faker.Generate()
            };

            Log.Information("DbInitializer : Seeding Db ...");
            foreach (Patient p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();
        }
    }
}
