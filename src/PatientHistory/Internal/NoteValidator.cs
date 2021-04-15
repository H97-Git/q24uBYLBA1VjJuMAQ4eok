using FluentValidation;
using PatientHistory.Data.DTO;

namespace PatientHistory.Internal
{
    public class NoteValidator : AbstractValidator<NoteDto>
    {
        public NoteValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .WithMessage("Message can't be empty.")
                .MinimumLength(10)
                .WithMessage($"Minimum length for message is 10");
        }
    }
}
