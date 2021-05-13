using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PatientAssessment.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PatientAssessment.Infrastructure.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IHttpClientFactory _clientFactory;
        private static IConfiguration Configuration { get; set; }
        private readonly IWebHostEnvironment _env;
        public AssessmentService(IHttpClientFactory clientFactory, IConfiguration configuration, IWebHostEnvironment env)
        {
            _clientFactory = clientFactory;
            Configuration = configuration;
            _env = env;
        }

        public async Task<Assessment> GetAssessmentByPatientId(int patientId)
        {
            var patientClient = _clientFactory.CreateClient();
            var noteClient = _clientFactory.CreateClient();
            try
            {
                var baseAddressP = "";
                var baseAddressN = "";

                if (_env.IsProduction())
                {
                   baseAddressP = Configuration["AssessmentService:PatientBaseAddressL"];
                   baseAddressN = Configuration["AssessmentService:NoteBaseAddressL"];
                }

                if (_env.IsDevelopment())
                {
                   baseAddressP = Configuration["AssessmentService:PatientBaseAddressW"];
                   baseAddressN = Configuration["AssessmentService:NoteBaseAddressW"];
                }
                string patientApiContent =
                    await GetApiContent(patientClient,baseAddressP,Configuration["AssessmentService:Endpoint:Patient"],patientId);
                if (patientApiContent is null) return null;

                string noteApiContent =
                    await GetApiContent(noteClient,baseAddressN, Configuration["AssessmentService:Endpoint:Note"], patientId);
                if (noteApiContent is null) return null;

                var notes = JsonConvert.DeserializeObject<List<NoteModel>>(noteApiContent);
                var patient = JsonConvert.DeserializeObject<PatientModel>(patientApiContent);

                return CreateAssessment(patient, notes);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("Api can't be reached : {message}", ex.Message);
                return null;
            }
        }

        private static async Task<string> GetApiContent(HttpClient client, string baseAddress, string endpoint, int patientId)
        {
            string apiContent = null;
            client.BaseAddress = new Uri(baseAddress);
            var apiResponse = await client.GetAsync(endpoint + patientId);
            if (apiResponse.IsSuccessStatusCode)
            {
                apiContent = await apiResponse.Content.ReadAsStringAsync();
            }
            return apiContent;
        }

        private static Assessment CreateAssessment(PatientModel patient, IReadOnlyCollection<NoteModel> notes)
        {
            Assessment assessment = new();
            assessment = MapPatient(assessment, patient);
            int totalTriggerCount = CountAssessmentTerm(notes);
            assessment.RiskLevel = SetRiskLevel(totalTriggerCount, assessment);
            return assessment;
        }

        private static Assessment MapPatient(Assessment assessment, PatientModel patient)
        {
            assessment.FamilyName = patient.FamilyName;
            assessment.GivenName = patient.GivenName;
            assessment.Age = patient.GetAge();
            assessment.Gender = patient.Gender;
            return assessment;
        }
        private static int CountAssessmentTerm(IReadOnlyCollection<NoteModel> notes)
        {
            var triggerTerms = Configuration.GetSection("AssessmentService:TriggerTerms").GetChildren();
            var triggerCount = 0;
            foreach (var term in triggerTerms)
            {
                var rx = new Regex($"{term.Value}?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                foreach (var note in notes)
                {
                    var match = rx.Match(note.Message);
                    while (match.Success)
                    {
                        triggerCount++;
                        Log.Debug($"Note.Id = {note.Id} : {match.Value} at index {match.Index} / TriggerCount : {triggerCount}");
                        match = match.NextMatch();
                    }
                }
            }
            return triggerCount;
        }
        private static RiskLevel SetRiskLevel(int totalTriggerCount, Assessment assessment)
        {
            return totalTriggerCount switch
            {
                2 when assessment.Age >= 30 => RiskLevel.Borderline,
                3 when assessment.Gender == Gender.Male && assessment.Age <= 30 => RiskLevel.InDanger,
                4 when assessment.Gender == Gender.Female && assessment.Age <= 30 => RiskLevel.InDanger,
                5 when assessment.Gender == Gender.Male && assessment.Age <= 30 => RiskLevel.EarlyOnset,
                6 when assessment.Age >= 30 => RiskLevel.InDanger,
                7 when assessment.Gender == Gender.Female && assessment.Age <= 30 => RiskLevel.EarlyOnset,
                <= 8 when assessment.Age >= 30 => RiskLevel.EarlyOnset,
                _ => RiskLevel.None
            };
        }
    }
}
