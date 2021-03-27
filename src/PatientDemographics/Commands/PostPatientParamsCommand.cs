using System;
using MediatR;
using PatientDemographics.DTO;

namespace PatientDemographics.Commands
{
    public class PostPatientParamsCommand: IRequest<PatientDto>
    {
        public PostPatientParamsCommand(string family, string given, DateTime dob, string gender, string address, string phone)
        {
            Family = family;
            Given = given;
            Dob = dob;
            Gender = gender;
            Address = address;
            Phone = phone;
        }

        public string Family { get; set; }
        public string Given { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
