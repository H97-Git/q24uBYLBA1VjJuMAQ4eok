using System.Threading.Tasks;
using BlazorPatient.Models;

namespace BlazorPatient.Infrastructure.Services
{
    public interface IAssessmentService
    {
        Task<AssessmentModel> GetByPatientId(int patientId);
    }
}