namespace BlazorPatient.Models
{
    public class AssessmentModel
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public RiskLevel RiskLevel { get; set; }

        public override string ToString()
        {
            return $"Patient: {GivenName} {FamilyName} (Age: {Age}) diabetes assessment is: {RiskLevel}";
        }
    }

    public enum RiskLevel
    {
        None,
        Borderline,
        InDanger,
        EarlyOnset
    }
}
