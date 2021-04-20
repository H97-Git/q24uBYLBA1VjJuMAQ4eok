using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BlazorPatient.DTO;
using Newtonsoft.Json;
using Serilog;

namespace BlazorPatient.Infrastructure.Services
{
    public class NoteService : INoteService
    {
        private readonly HttpClient _client;
        public string ErrorMessage { get; set; }

        public NoteService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            _client.BaseAddress = isWindows ? new Uri("https://localhost:5003") : new Uri("http://localhost:80");

        }

        public async Task<List<NoteDto>> GetByPatientId(int patientId)
        {
            try
            {
                var apiResponse = await _client.GetAsync("/Note/patient/" + patientId);
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<NoteDto>();

                string content = await apiResponse.Content.ReadAsStringAsync();
                var notes = JsonConvert.DeserializeObject<List<NoteDto>>(content);
                return notes;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);
                return new List<NoteDto>();
            }
        }

        public async Task<int> Save(NoteDto noteDto)
        {
            var addContent = new StringContent(JsonConvert.SerializeObject(noteDto), Encoding.UTF8);
            addContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var apiResponse = noteDto.Id is null
                    ? await _client.PostAsync("/Note/add", addContent)
                    : await _client.PutAsync("/Note/edit/" + noteDto.Id, addContent);

                string content = await apiResponse.Content.ReadAsStringAsync();
                ErrorMessage = JsonConvert.DeserializeObject(content)?.ToString();

                return apiResponse.IsSuccessStatusCode ? noteDto.Id is null ? 1 : 2 : 0;
                //IsSuccessStatusCode = true && noteDto.Id is null ? - It's a Save return 1 : noteDto.Id is not null - It's an Update return 2
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
    }
}
