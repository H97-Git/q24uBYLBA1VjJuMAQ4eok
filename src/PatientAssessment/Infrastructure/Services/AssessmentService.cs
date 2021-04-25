using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PatientAssessment.Data;
using Serilog;

namespace PatientAssessment.Infrastructure.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IHttpClientFactory _clientFactory;
        public AssessmentService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Assessment> GetAssessmentByPatientId(int patientId)
        {
            Assessment assessment = new();
            var patientClient = _clientFactory.CreateClient();
            patientClient.BaseAddress = new Uri("https://localhost:5001");
            try
            {
                var patientApiResponse = await patientClient.GetAsync("/Patient/" + patientId);
                string patientApiContent = await patientApiResponse.Content.ReadAsStringAsync();
                if (patientApiResponse.IsSuccessStatusCode)
                {
                    var noteClient = _clientFactory.CreateClient();
                    noteClient.BaseAddress = new Uri("https://localhost:5003");
                    var noteApiResponse = await noteClient.GetAsync("/Note/patient/" + patientId);

                    string noteApiContent = await noteApiResponse.Content.ReadAsStringAsync();
                    var notes = JsonConvert.DeserializeObject<List<NoteModel>>(noteApiContent);
                    var patient = JsonConvert.DeserializeObject<PatientModel>(patientApiContent);

                    assessment = MapPatient(assessment, patient);
                    int triggerTerm = CountAssessmentTerm(notes);
                    assessment.RiskLevel = SetAssessment(triggerTerm, assessment);
                    return assessment;
                }

                return assessment;
            }
            catch (HttpRequestException ex)
            {
                Log.Error("Api can't be reached : {message}", ex.Message);
                return assessment;
            }
        }

        private static RiskLevel SetAssessment(int triggerTerm, Assessment assessment)
        {
            switch (triggerTerm)
            {
                case 2 when assessment.Age >= 30:
                    return RiskLevel.Borderline;
                case 3 when assessment.Gender == Gender.Male && assessment.Age <= 30:
                case 4 when assessment.Gender == Gender.Female && assessment.Age <= 30:
                    return RiskLevel.InDanger;
                case 5 when assessment.Gender == Gender.Male && assessment.Age <= 30:
                    return RiskLevel.EarlyOnset;
                case 6 when assessment.Age >= 30:
                    return RiskLevel.InDanger;
                case 7 when assessment.Gender == Gender.Female && assessment.Age <= 30:
                case <= 8 when assessment.Age >= 30:
                    return RiskLevel.EarlyOnset;
                default:
                    return RiskLevel.None;
            }
        }

        private static int CountAssessmentTerm(IReadOnlyCollection<NoteModel> notes)
        {
            string[] triggerTerms =
            {
                "Hemoglobin A1C","Microalbumin","Body Height","Body Weight","Smoker","Abnormal","Cholesterol","Dizziness","Relapse","Reaction","Antibodies"
            };
            var triggerCount = 0;
            foreach (string term in triggerTerms)
            {
                var rx = new Regex($"{term}?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                foreach (var note in notes)
                {
                    var match = rx.Match(note.Message);
                    while (match.Success)
                    {
                        triggerCount++;
                        Log.Information($"Note.Id = {note.Id} : {match.Value} at index {match.Index} / TriggerCount : {triggerCount}");
                        match = match.NextMatch();
                    }
                }
            }
            return triggerCount;
        }

        private static Assessment MapPatient(Assessment assessment, PatientModel patient)
        {
            assessment.FamilyName = patient.FamilyName;
            assessment.GivenName = patient.GivenName;
            assessment.Age = GetAge(patient.DateOfBirth);
            assessment.Gender = patient.Gender;
            return assessment;
        }

        private static int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
