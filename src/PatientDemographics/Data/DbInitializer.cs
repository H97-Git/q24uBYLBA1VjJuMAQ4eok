using System;
using Serilog;
using System.Linq;

namespace PatientDemographics.Data
{
    public class DbInitializer
    {
        public static void Initialize(PatientContext context)
        {
            Log.Debug("DbInitializer : EnsureCreated()");
            context.Database.EnsureCreated();

            if (context.Patients.Any())
            {
                return;
            }

            var patients = new Patient[]
            {
                new()
                {
                    FamilyName = "Ferguson",
                    GivenName = "Lucas",
                    DateOfBirth = new DateTime(1968,06,22),
                    Gender = Gender.Male,
                    HomeAddress = "2 Warren Street",
                    PhoneNumber = "387-866-1399"
                },
                new()
                {
                    FamilyName = "Rees",
                    GivenName = "Pippa",
                    DateOfBirth = new DateTime(1952,09,27),
                    Gender = Gender.Female,
                    HomeAddress = "745 West Valley Farms Drive",
                    PhoneNumber = "628-423-0993"
                },
                new()
                {
                    FamilyName = "Arnold",
                    GivenName = "Edward",
                    DateOfBirth = new DateTime(1952,11,11),
                    Gender = Gender.Male,
                    HomeAddress = "599 East Garden Ave",
                    PhoneNumber = "123-727-2779"
                },
                new()
                {
                    FamilyName = "Sharp",
                    GivenName = "Anthony",
                    DateOfBirth = new DateTime(1946,11,26),
                    Gender = Gender.Male,
                    HomeAddress = "894 Hall Street",
                    PhoneNumber = "451-761-8383"
                },
                new()
                {
                    FamilyName = "Ince",
                    GivenName = "Wendy",
                    DateOfBirth = new DateTime(1958,06,29),
                    Gender = Gender.Female,
                    HomeAddress = "4 Southampton Road",
                    PhoneNumber = "802-911-9975"
                },
                new()
                {
                    FamilyName = "Ross",
                    GivenName = "Tracey",
                    DateOfBirth = new DateTime(1949,12,07),
                    Gender = Gender.Female,
                    HomeAddress = "40 Sulphur Springs DR",
                    PhoneNumber = "131-396-5049"
                },
                new()
                {
                    FamilyName = "Wilson",
                    GivenName = "Claire",
                    DateOfBirth = new DateTime(1966,12,31),
                    Gender = Gender.Female,
                    HomeAddress = "12 Cobblestone St",
                    PhoneNumber = "300-452-1091"
                },
                new()
                {
                    FamilyName = "Buckland",
                    GivenName = "Max",
                    DateOfBirth = new DateTime(1945,06,24),
                    Gender = Gender.Male,
                    HomeAddress = "193 Vale St",
                    PhoneNumber = "833-534-0864"
                },
                new()
                {
                    FamilyName = "Clark",
                    GivenName = "Natalie",
                    DateOfBirth = new DateTime(1964,06,18),
                    Gender = Gender.Female,
                    HomeAddress = "12 Beechwood Road",
                    PhoneNumber = "241-467-9197"
                },
                new()
                {
                    FamilyName = "Bailey",
                    GivenName = "Piers",
                    DateOfBirth = new DateTime(1959,06,28),
                    Gender = Gender.Male,
                    HomeAddress = "1202 Bumble Dr",
                    PhoneNumber = "747-815-0557"
                }
            };

            Log.Debug("DbInitializer : Seeding Db ...");
            foreach (var p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();
        }
    }
}
