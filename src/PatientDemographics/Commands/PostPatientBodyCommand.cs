using MediatR;
using PatientDemographics.DTO;

namespace PatientDemographics.Commands
{
    public class PostPatientBodyCommand: IRequest<PatientDto>
    {
        public PostPatientBodyCommand(PatientDto patientDto)
        {
            PatientDto = patientDto;
        }

        public PatientDto PatientDto { get; set; }
    }
}
