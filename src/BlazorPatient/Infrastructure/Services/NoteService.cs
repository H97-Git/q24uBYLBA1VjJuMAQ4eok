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
    public class NoteService : INoteService
    {
        public IConfiguration Configuration { get; }
        public string ErrorMessage { get; set; }
        private readonly HttpClient _client;

        public NoteService(IHttpClientFactory httpClientFactory, IHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _client = httpClientFactory.CreateClient();
            if (env.IsEnvironment("Docker"))
            {
                _client.BaseAddress = new Uri(Configuration["BlazorPatient:NoteService:BaseAddressL"]);
            }
            if (env.IsDevelopment())
            {
                _client.BaseAddress = new Uri(Configuration["BlazorPatient:NoteService:BaseAddressW"]);
            }

        }

        public async Task<List<NoteModel>> GetByPatientId(int patientId)
        {
            try
            {
                var apiResponse = await _client.GetAsync(Configuration["BlazorPatient:NoteService:Endpoint:Get"] + patientId);
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
                    ? await _client.PostAsync(Configuration["BlazorPatient:NoteService:Endpoint:Post"], addContent)
                    : await _client.PutAsync(Configuration["BlazorPatient:NoteService:Endpoint:Put"] + note.Id, addContent);

                string content = await apiResponse.Content.ReadAsStringAsync();
                ErrorMessage = string.Empty;
                if (content.Contains("Validation"))
                {
                    ErrorMessage = "Minimum length for message is 10.\n";
                }

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
