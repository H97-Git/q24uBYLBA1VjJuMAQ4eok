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

        public async Task<List<NoteModel>> GetByPatientId(int patientId)
        {
            try
            {
                var apiResponse = await _client.GetAsync("/Note/patient/" + patientId);
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<NoteModel>();

                string content = await apiResponse.Content.ReadAsStringAsync();
                var notes = JsonConvert.DeserializeObject<List<NoteModel>>(content);
                return notes;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);
                return new List<NoteModel>();
            }
        }

        public async Task<int> Save(NoteModel note)
        {
            var addContent = new StringContent(JsonConvert.SerializeObject(note), Encoding.UTF8);
            addContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var apiResponse = note.Id is null
                    ? await _client.PostAsync("/Note/add", addContent)
                    : await _client.PutAsync("/Note/edit/" + note.Id, addContent);

                string content = await apiResponse.Content.ReadAsStringAsync();
                ErrorMessage = JsonConvert.DeserializeObject(content)?.ToString();

                return apiResponse.IsSuccessStatusCode ? note.Id is null ? 1 : 2 : 0;
                //IsSuccessStatusCode = true && NoteModel.Id is null ? - It's a Save return 1 : NoteModel.Id is not null - It's an Update return 2
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
