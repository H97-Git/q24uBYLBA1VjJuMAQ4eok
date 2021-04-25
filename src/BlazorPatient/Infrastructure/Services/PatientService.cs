using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BlazorPatient.Models;
using Serilog;

namespace BlazorPatient.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        public string ErrorMessage { get; set; }
        private record Command(PatientModel PatientDto);
        private readonly HttpClient _client;
        public PatientService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            _client.BaseAddress = isWindows ? new Uri("https://localhost:5001") : new Uri("http://localhost:80");

        }
        public async Task<List<PatientModel>> Get()
        {
            try
            {
                var apiResponse = await _client.GetAsync("/Patient/");
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
                    ? await _client.PostAsync("/Patient/addBody", addContent)
                    : await _client.PutAsync("/Patient/edit/" + patientDto.Id, editContent);

                string content = await apiResponse.Content.ReadAsStringAsync();
                ErrorMessage = JsonConvert.DeserializeObject(content)?.ToString();

                return apiResponse.IsSuccessStatusCode ? patientDto.Id == 0 ? 1 : 2 : 0;
                //IsSuccesStatusCode = true && Patient.Id = 0 - It's a Save return 1 : Patient.Id != 0 - It's an Update return 2
                //IsSuccesStatusCode = false ? Something went wrong return 0
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
    }
}
