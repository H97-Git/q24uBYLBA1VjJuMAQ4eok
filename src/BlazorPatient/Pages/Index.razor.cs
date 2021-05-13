using BlazorPatient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPatient.Pages
{
    public partial class Index
    {
        private List<AssessmentModel> _assessments;
        private List<PatientModel> _patients = new();

        protected override async Task OnInitializedAsync()
        {
            _patients = await PatientService.Get();
            _assessments = new List<AssessmentModel>();
            foreach (var item in _patients)
            {
                var assessment = await AssessmentService.GetByPatientId(item.Id);
                _assessments.Add(assessment);
            }
        }
    }
}
