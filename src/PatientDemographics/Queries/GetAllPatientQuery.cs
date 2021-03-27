using MediatR;
using PatientDemographics.DTO;
using System.Collections.Generic;

namespace PatientDemographics.Queries
{
    public class GetAllPatientQuery : IRequest<List<PatientDto>> { }
}
