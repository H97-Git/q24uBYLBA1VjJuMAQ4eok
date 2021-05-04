using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPatient.Models;
using Microsoft.Extensions.Configuration;

namespace BlazorPatient.Infrastructure.Services
{
    public interface IPatientService
    {
        Task<List<PatientModel>> Get();
        Task<int> Save(PatientModel patient);
        public string ErrorMessage { get; set; }
        public IConfiguration Configuration { get; }
    }
}