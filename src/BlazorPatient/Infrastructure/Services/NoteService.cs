using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public NoteService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5003");

        }
        public async Task<List<NoteDto>> Get()
        {
            try
            {
                var apiResponse = await _client.GetAsync("/Note/");
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<NoteDto>();

                string content = await apiResponse.Content.ReadAsStringAsync();
                var notes = JsonConvert.DeserializeObject<List<NoteDto>>(content);
                return notes;
            }
            catch (HttpRequestException exception)
            {
                Log.Error("Api can't be reached : {message}", exception.Message);
                return new List<NoteDto>();
            }
        }

        public async Task<int> Save(NoteDto noteDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(noteDto), Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var apiResponse = noteDto.Id is null
                    ? await _client.PostAsync("/Note/add", content)
                    : await _client.PutAsync("/Note/edit/" + noteDto.Id, content);

                return apiResponse.IsSuccessStatusCode ? noteDto.Id == string.Empty ? 1 : 2 : 0;
                //IsSuccessStatusCode = true && Patient.Id = 0 - It's a Save return 1 : Patient.Id != 0 - It's an Update return 2
                //IsSuccessStatusCode = false ? Something went wrong return 0
            }
            catch (HttpRequestException exception)
            {
                Log.Error("Api can't be reached : {message}", exception.Message);
                return 0;
            }
        }
    }
}
