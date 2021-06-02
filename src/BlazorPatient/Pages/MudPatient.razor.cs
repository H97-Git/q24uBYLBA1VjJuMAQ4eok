using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPatient.Models;
using MudBlazor;

namespace BlazorPatient.Pages
{
    public partial class MudPatient
    {
        private string _searchString;
        private string _cardHeaderString = "Add a patient :";

        private List<PatientModel> _patients;
        private PatientModel _patient = new();
        private PatientModel _cachePatient = new();
        private readonly List<AssessmentModel> _assessments = new();

        protected override async Task OnInitializedAsync()
        {
            _patients = await PatientService.Get();
            if (PatientService.ErrorMessage is not null && PatientService.ErrorMessage != "")
            {
                SnackBar.Add(PatientService.ErrorMessage, Severity.Error);
            }
            //foreach (var patientModel in _patients)
            //{
            //    var assessment = await AssessmentService.GetByPatientId(patientModel.Id);
            //    _assessments.Add(assessment);
            //}
        }

        private string GetAssessmentIcon(PatientModel patientModel)
        {
            var riskLevel = _assessments.Find(p => p.FamilyName == patientModel.FamilyName)?.RiskLevel;
            return riskLevel switch
            {
                RiskLevel.None => Icons.Material.Filled.CheckCircle,
                RiskLevel.Borderline => Icons.Material.Filled.BrokenImage,
                RiskLevel.EarlyOnset => Icons.Material.Filled.Warning,
                RiskLevel.InDanger => Icons.Material.Filled.Dangerous,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        private Color GetAssessmentColor(PatientModel patientModel)
        {
            var riskLevel = _assessments.Find(p => p.FamilyName == patientModel.FamilyName)?.RiskLevel;
            return riskLevel switch
            {
                RiskLevel.None => Color.Tertiary,
                RiskLevel.Borderline => Color.Dark,
                RiskLevel.EarlyOnset => Color.Warning,
                RiskLevel.InDanger => Color.Error,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        private string GetAssessmentText(PatientModel patientModel)
        {
            return _assessments.Find(p => p.FamilyName == patientModel.FamilyName)?.RiskLevel.ToString();
        }
        private static string GetPatientDate(DateTime? dateOfBirth)
        {
            return dateOfBirth != null ? dateOfBirth.Value.ToShortDateString() : "";
        }
        private bool SearchPatient(PatientModel patientParam)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return patientParam.GivenName.Contains(_searchString, StringComparison.OrdinalIgnoreCase)
                   || patientParam.FamilyName.Contains(_searchString, StringComparison.OrdinalIgnoreCase)
                   || patientParam.PhoneNumber.Contains(_searchString, StringComparison.OrdinalIgnoreCase);
        }
        private async Task GetPatients()
        {
            _patients = await PatientService.Get();
        }
        private async Task SavePatient()
        {
            _cardHeaderString = "Add a patient :";
            int result = await PatientService.Save(_patient);
            _patient = new PatientModel();
            switch (result)
            {
                case 0:
                    SnackBar.Add("Something went wrong : ", Severity.Error);
                    SnackBar.Add(PatientService.ErrorMessage, Severity.Info);
                    _patient = _cachePatient;
                    break;
                case 1:
                    SnackBar.Add("Patient Saved.", Severity.Success);
                    break;
                case 2:
                    SnackBar.Add("Patient Updated.", Severity.Success);
                    break;
            }
            await GetPatients();
        }
        private void EditPatient(int id)
        {
            _cardHeaderString = "Edit a patient :";
            _patient = _patients.FirstOrDefault(p => p.Id == id);
            _cachePatient = _patient;
        }
        private void OnRowClicked(TableRowClickEventArgs<PatientModel> args)
        {
            NavigationManager.NavigateTo($"/Patient/{args.Item.Id}");
        }
    }
}
