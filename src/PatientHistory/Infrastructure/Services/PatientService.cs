using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

namespace PatientHistory.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _client;
        public IConfiguration Configuration { get; }
        public PatientService(IHttpClientFactory httpClientFactory, IHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = env.IsDevelopment() ? new Uri(Configuration["PatientService:BaseAddressW"]) : new Uri(Configuration["PatientService:BaseAddressL"]);
        }

        public async Task<bool> Get(int id)
        {
            var apiResponse = await _client.GetAsync(Configuration["PatientService:Endpoint:Get"] + id);
            return apiResponse.IsSuccessStatusCode;
        }

    }
}
