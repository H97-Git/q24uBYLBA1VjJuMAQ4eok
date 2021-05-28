using BlazorPatient.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPatient.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        public IConfiguration Configuration { get; }
        public string ErrorMessage { get; set; }

        private readonly HttpClient _client;
        private record Command(PatientModel PatientDto);
        public PatientService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _client = httpClientFactory.CreateClient();

            if (env.IsEnvironment("Docker"))
            {
                _client.BaseAddress = new Uri(Configuration["BlazorPatient:PatientService:BaseAddressL"]);
            }
            if (env.IsDevelopment())
            {
                _client.BaseAddress = new Uri(Configuration["BlazorPatient:PatientService:BaseAddressW"]);
            }
        }
        public async Task<List<PatientModel>> Get()
        {
            try
            {
                var apiResponse =
                    await _client.GetAsync(Configuration["BlazorPatient:PatientService:Endpoint:Get"]);
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<PatientModel>();

                string content = await apiResponse.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<PatientModel>>(content);

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
                new StringContent(JsonConvert.SerializeObject(new Command(patientDto)), Encoding.UTF8)
                { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };
            using var editContent =
                new StringContent(JsonConvert.SerializeObject(patientDto), Encoding.UTF8)
                { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };

            try
            {
                var apiResponse = patientDto.Id == 0
                    ? await _client.PostAsync(Configuration["BlazorPatient:PatientService:Endpoint:Post"], addContent)
                    : await _client.PutAsync(Configuration["BlazorPatient:PatientService:Endpoint:Put"] + patientDto.Id, editContent);

                string content = await apiResponse.Content.ReadAsStringAsync();
                HandleApiError(content);

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

        private void HandleApiError(string content)
        {
            ErrorMessage = string.Empty;
            if (content.Contains("Validation Exception"))
            {
                if (content.Contains("given name"))
                {
                    ErrorMessage += "Must specify a given name.\n";
                }

                if (content.Contains("family name"))
                {
                    ErrorMessage += "Must specify a family name.\n";
                }
            }
            if (content.Contains("errors"))
            {
                if (content.Contains("Gender"))
                {
                    ErrorMessage += "Must specify a gender.\n";
                }

                if (content.Contains("DateOfBirth"))
                {
                    ErrorMessage += "Must specify a date of birth.\n";
                }
            }
        }
    }
}
