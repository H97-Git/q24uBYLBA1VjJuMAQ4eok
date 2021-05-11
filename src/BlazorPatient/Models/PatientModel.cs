using System;

namespace BlazorPatient.Models
{
    public class PatientModel
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string HomeAddress { get; set; }
        public string PhoneNumber { get; set; }

        public int GetAge()
        {
            if (DateOfBirth == null)
            {
                return 0;
            }

            var today = DateTime.Today;
            int age = today.Year - DateOfBirth.Value.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (DateOfBirth.Value.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
