using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlazorPatient.DTO;
using Newtonsoft.Json;

namespace BlazorPatient.Infrastructure.Services
{
    public class NoteService : INoteService
    {
        private readonly HttpClient _client;

        private record Command(NoteDto NoteDto);

        public NoteService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5003");

        }
        public async Task<List<NoteDto>> Get()
        {
            var apiResponse = await _client.GetAsync("/Note/");

            if (!apiResponse.IsSuccessStatusCode)
                return new List<NoteDto>();

            string content = await apiResponse.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<List<NoteDto>>(content);
            return notes;
        }

        public async Task<int> Save(NoteDto noteDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Command(noteDto)), Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var apiResponse = noteDto.Id == string.Empty
                ? await _client.PostAsync("/Note/add", content)
                : await _client.PutAsync("/Note/edit/" + noteDto.Id, content);

            return apiResponse.IsSuccessStatusCode ? noteDto.Id == string.Empty ? 1 : 2 : 0;
            //IsSuccesStatusCode = true && Patient.Id = 0 - It's a Save return 1 : Patient.Id != 0 - It's an Update return 2
            //IsSuccesStatusCode = false ? Something went wrong return 0
        }
    }
}
