using System.Threading.Tasks;

namespace PatientHistory.Infrastructure.Services
{
    public interface IPatientService
    {
        Task<bool> Get(int id);
    }
}