using BlazorPatient.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BlazorPatient.Infrastructure.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly HttpClient _client;
        public IConfiguration Configuration { get; }
        public AssessmentService(IHttpClientFactory httpClientFactory,IHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _client = httpClientFactory.CreateClient();
            if (env.IsEnvironment("Docker"))
            {
                _client.BaseAddress = new Uri(Configuration["BlazorPatient:AssessmentService:BaseAddressL"]);
            }
            if (env.IsDevelopment())
            {
                _client.BaseAddress = new Uri(Configuration["BlazorPatient:AssessmentService:BaseAddressW"]);
            }
        }

        public async Task<AssessmentModel> GetByPatientId(int patientId)
        {
            try
            {
                var apiResponse = await _client.GetAsync(Configuration["BlazorPatient:AssessmentService:Endpoint:Get"] + patientId);
                if (!apiResponse.IsSuccessStatusCode)
                    return new AssessmentModel();

                string content = await apiResponse.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content)) 
                    return new AssessmentModel();

                var assessment = JsonConvert.DeserializeObject<AssessmentModel>(content);
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
