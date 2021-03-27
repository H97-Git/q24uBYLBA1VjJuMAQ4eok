using MediatR;
using PatientDemographics.DTO;

namespace PatientDemographics.Queries
{
    public class GetPatientByIdQuery : IRequest<PatientDto>
    {
        public int Id { get; set; }

        public GetPatientByIdQuery(int id)
        {
            Id = id;
        }
    }
}
