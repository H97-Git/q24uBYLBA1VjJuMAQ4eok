using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BlazorPatient.Models;
using Newtonsoft.Json;
using Serilog;

namespace BlazorPatient.Infrastructure.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly HttpClient _client;

        public AssessmentService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            _client.BaseAddress = isWindows ? new Uri("https://localhost:5005") : new Uri("http://localhost:80");

        }

        public async Task<AssessmentModel> GetByPatientId(int patientId)
        {
            try
            {
                var apiResponse = await _client.GetAsync("api/Assessment/" + patientId);
                if (!apiResponse.IsSuccessStatusCode)
                    return new AssessmentModel();

                string content = await apiResponse.Content.ReadAsStringAsync();
                var assessment = JsonConvert.DeserializeObject<AssessmentModel>(content);
                return assessment;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);
                return new AssessmentModel();
            }
        }

        private void HandleHttpRequestException(HttpRequestException ex)
        {
            Log.Error("Api can't be reached : {message}", ex.Message);
        }
    }
}
