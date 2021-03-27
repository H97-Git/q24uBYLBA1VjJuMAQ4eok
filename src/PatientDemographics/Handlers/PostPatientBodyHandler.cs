using MediatR;
using PatientDemographics.Commands;
using PatientDemographics.DTO;
using PatientDemographics.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;

namespace PatientDemographics.Handlers
{
    public class PostPatientBodyHandler : IRequestHandler<PostPatientBodyCommand, PatientDto>
    {
        private readonly IPatientService _patientService;

        public PostPatientBodyHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<PatientDto> Handle(PostPatientBodyCommand request, CancellationToken cancellationToken)
        {
            await _patientService.SavePatient(request.PatientDto);
            var list = await _patientService.GetPatient();
            var patient = list.Find(p => p.FamilyName == request.PatientDto.FamilyName);
            return patient;
        }
    }
}
