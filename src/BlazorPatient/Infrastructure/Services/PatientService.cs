using BlazorPatient.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace BlazorPatient.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _client;

        private record Command(PatientDto PatientDto);

        public PatientService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");

        }
        public async Task<List<PatientDto>> Get()
        {
            try
            {
                var apiResponse = await _client.GetAsync("/Patient/");

                if (!apiResponse.IsSuccessStatusCode)
                    return new List<PatientDto>();

                string content = await apiResponse.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<PatientDto>>(content);
                return patients;
            }
            catch(HttpRequestException exception)
            {
                Log.Error("Api can't be reached : {message}",exception.Message);
                return new List<PatientDto>();
            }
        }

        public async Task<int> Save(PatientDto patientDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Command(patientDto)), Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var apiResponse = patientDto.Id == 0
                    ? await _client.PostAsync("/Patient/addBody", content)
                    : await _client.PutAsync("/Patient/edit/" + patientDto.Id, content);

                return apiResponse.IsSuccessStatusCode ? patientDto.Id == 0 ? 1 : 2 : 0;
                //IsSuccesStatusCode = true && Patient.Id = 0 - It's a Save return 1 : Patient.Id != 0 - It's an Update return 2
                //IsSuccesStatusCode = false ? Something went wrong return 0
            }
            catch (HttpRequestException exception)
            {
                Log.Error("Api can't be reached : {message}",exception.Message);
                return 0;
            }
        }

    }
}
