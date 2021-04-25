using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPatient.Models;

namespace BlazorPatient.Infrastructure.Services
{
    public interface IPatientService
    {
        Task<List<PatientModel>> Get();
        Task<int> Save(PatientModel patient);
        public string ErrorMessage { get; set; }
    }
}