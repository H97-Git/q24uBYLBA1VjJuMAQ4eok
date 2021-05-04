using System.Threading.Tasks;
using BlazorPatient.Models;
using Microsoft.Extensions.Configuration;

namespace BlazorPatient.Infrastructure.Services
{
    public interface IAssessmentService
    {
        Task<AssessmentModel> GetByPatientId(int patientId);
        public IConfiguration Configuration { get; }
    }
}