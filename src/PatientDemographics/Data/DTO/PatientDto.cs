using System;

namespace PatientDemographics.Data.DTO
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string HomeAddress { get; set; }
        public string PhoneNumber { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
