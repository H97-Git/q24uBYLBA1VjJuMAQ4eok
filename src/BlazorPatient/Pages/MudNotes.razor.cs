using BlazorPatient.Models;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPatient.Pages
{
    public partial class MudNotes
    {
        private List<NoteModel> _notes;
        private NoteModel _note = new();
        private List<PatientModel> _patients = new();
        private PatientModel _patient = new();

        private string _searchString = "";
        private string _cardHeaderString = "Add a note :";

        private int _age;
        private string _riskLevel;
        private string _riskLevelIcon;
        private Color _riskLevelColor;

        protected override async Task OnInitializedAsync()
        {
            _patients = await PatientService.Get();
            if (_patients.All(x => x.Id != PatientId))
            {
                SnackBar.Add($"Patient n° {PatientId} not found.", Severity.Error);
                NavigationManager.NavigateTo("/Patients");
            }

            _notes = await NoteService.GetByPatientId(PatientId);
            _patient = _patients.Find(x => x.Id == PatientId);
            if (_patient != null) _age = _patient.GetAge();
            var assessment = await AssessmentService.GetByPatientId(PatientId);
            _riskLevel = assessment.RiskLevel.ToString();
            switch (assessment.RiskLevel)
            {
                case RiskLevel.None:
                    _riskLevelIcon = Icons.Material.Filled.CheckCircle;
                    _riskLevelColor = Color.Tertiary;
                    break;
                case RiskLevel.Borderline:
                    _riskLevelIcon = Icons.Material.Filled.BrokenImage;
                    _riskLevelColor = Color.Dark;
                    break;
                case RiskLevel.EarlyOnset:
                    _riskLevelIcon = Icons.Material.Filled.Warning;
                    _riskLevelColor = Color.Warning;
                    break;
                case RiskLevel.InDanger:
                    _riskLevelIcon = Icons.Material.Filled.Dangerous;
                    _riskLevelColor = Color.Error;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SearchNote(NoteModel note)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return note.Id.Contains(_searchString, StringComparison.OrdinalIgnoreCase)
                   || note.PatientId.Equals(int.Parse(_searchString))
                   || note.Message.Contains(_searchString, StringComparison.OrdinalIgnoreCase);
        }

        private async Task SaveNote()
        {
            _cardHeaderString = "Add a note :";
            _note.PatientId = PatientId;
            int result = await NoteService.Save(_note);
            var cacheNote = _note;
            _note = new NoteModel();
            switch (result)
            {
                case 0:
                    SnackBar.Add("Something went wrong : ", Severity.Error);
                    SnackBar.Add(NoteService.ErrorMessage,Severity.Info);
                    _note = cacheNote;
                    break;
                case 1:
                    SnackBar.Add("Note Saved.", Severity.Success);
                    break;
                case 2:
                    SnackBar.Add("Note Updated.", Severity.Success);
                    break;
                default:
                    SnackBar.Add("Something went wrong.", Severity.Error);
                    break;
            }
            _notes = await NoteService.GetByPatientId(PatientId);
        }


        private void EditNote(string id)
        {
            _cardHeaderString = "Edit a note :";
            _note = _notes.FirstOrDefault(p => p.Id == id);
        }

        private static string GetPatientDate(DateTime? dateOfBirth)
        {
            return dateOfBirth != null ? dateOfBirth.Value.ToShortDateString() : "";
        }
    }
}
