using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PatientDemographics.DTO;

namespace PatientDemographics.Commands
{
    public class PutPatientCommand : IRequest<PatientDto>
    {
        public PutPatientCommand(int id,PatientDto patientDto)
        {
            PatientDto = patientDto;
            Id = id;
        }

        public int Id { get; set; }
        public PatientDto PatientDto { get; set; }
    }
}
