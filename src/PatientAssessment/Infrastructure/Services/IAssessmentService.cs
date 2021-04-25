using System.Threading.Tasks;
using PatientAssessment.Data;

namespace PatientAssessment.Infrastructure.Services
{
    public interface IAssessmentService
    {
        Task<Assessment> GetAssessmentByPatientId(int patientId);
    }
}