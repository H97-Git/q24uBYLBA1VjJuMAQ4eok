using BlazorPatient.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorPatient.Infrastructure.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly HttpClient _client;
        public IConfiguration Configuration { get; }
        public AssessmentService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            Configuration = configuration;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Configuration["BlazorPatient:AssessmentService:BaseAddress"]);
        }

        public async Task<AssessmentModel> GetByPatientId(int patientId)
        {
            try
            {
                var apiResponse = await _client.GetAsync(Configuration["BlazorPatient:AssessmentService:Endpoint:Get"] + patientId);
                if (!apiResponse.IsSuccessStatusCode)
                    return new AssessmentModel();

                string content = await apiResponse.Content.ReadAsStringAsync();
                var assessment = JsonSerializer.Deserialize<AssessmentModel>(content);
                return assessment;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);
                return new AssessmentModel();
            }
        }

        private static void HandleHttpRequestException(HttpRequestException ex)
        {
            Log.Error("Api can't be reached : {message}", ex.Message);
        }
    }
}
