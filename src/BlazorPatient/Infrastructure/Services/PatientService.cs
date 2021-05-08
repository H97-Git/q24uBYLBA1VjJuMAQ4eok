using BlazorPatient.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorPatient.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        public IConfiguration Configuration { get; }
        public string ErrorMessage { get; set; }

        private readonly HttpClient _client;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHostEnvironment _env;
        private record Command(PatientModel PatientDto);
        public PatientService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHostEnvironment env)
        {
            _env = env;
            _clientFactory = httpClientFactory;
            Configuration = configuration;
            string baseAddress = _env.IsDevelopment() ? Configuration["BlazorPatient:PatientService:BaseAddressW"] : Configuration["BlazorPatient:PatientService:BaseAddressL"];
            _client = httpClientFactory.CreateClient();
            //_client.BaseAddress = new Uri(baseAddress);
        }
        public async Task<List<PatientModel>> Get()
        {
            if (_env.IsProduction())
            {
                await TestBaseAddress();
                
                return new List<PatientModel>();
            }

            try
            {
                var apiResponse =
                    await _client.GetAsync(Configuration["BlazorPatient:PatientService:Endpoint:Get"]);
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<PatientModel>();

                string content = await apiResponse.Content.ReadAsStringAsync();
                var patients = JsonSerializer.Deserialize<List<PatientModel>>(content);
                
                return patients;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);
                
                return new List<PatientModel>();
            }

        }

        public async Task<int> Save(PatientModel patientDto)
        {
            using var addContent =
                new StringContent(JsonSerializer.Serialize(new Command(patientDto)), Encoding.UTF8)
                { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };
            using var editContent =
                new StringContent(JsonSerializer.Serialize(patientDto), Encoding.UTF8)
                { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };

            try
            {
                var apiResponse = patientDto.Id == 0
                    ? await _client.PostAsync(Configuration["BlazorPatient:PatientService:Endpoint:Post"], addContent)
                    : await _client.PutAsync(Configuration["BlazorPatient:PatientService:Endpoint:Put"] + patientDto.Id, editContent);

                string content = await apiResponse.Content.ReadAsStringAsync();
                ErrorMessage = JsonSerializer.Deserialize<string>(content);

                return apiResponse.IsSuccessStatusCode ? patientDto.Id == 0 ? 1 : 2 : 0;
                //IsSuccessStatusCode = true && Patient.Id = 0 - It's a Save return 1 : Patient.Id != 0 - It's an Update return 2
                //IsSuccessStatusCode = false ? Something went wrong return 0
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);
                return 0;
            }
        }

        private void HandleHttpRequestException(HttpRequestException ex)
        {
            ErrorMessage = "Api can't be reached.";
            Log.Error("Api can't be reached : {message}", ex.Message);
        }

        private async Task TestBaseAddress()
        {
            var index = 0;
            var baseAddressStrings = new[] { "http://0.0.0.0:5000", "http://127.0.0.1:5000", "http://localhost:5000", "http://patientdemographics-api:5000" };
            foreach (string addressString in baseAddressStrings)
            {
                try
                {
                    Log.Debug($"Index : {index}");
                    index++;
                    Log.Debug(addressString);
                    var client = _clientFactory.CreateClient();
                    client.BaseAddress = new Uri(addressString);
                    var apiResponse = await client.GetAsync(Configuration["BlazorPatient:PatientService:Endpoint:Get"]);
                    string content = await apiResponse.Content.ReadAsStringAsync();
                    Log.Debug("content :");
                    Log.Debug(content);
                }
                catch (Exception ex)
                {
                    Log.Debug($"Message : {ex.Message}");
                }
            }
        }
    }
}
