using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PatientHistory.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _client;

        public PatientService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }

        public async Task<bool> Get(int id)
        {
            var apiResponse = await _client.GetAsync("/Patient/" + id);
            return apiResponse.IsSuccessStatusCode;
        }

    }
}
