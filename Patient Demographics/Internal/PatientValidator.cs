using FluentValidation;
using Patient_Demographics.DTO;
using System;

namespace Patient_Demographics.Internal
{
    public class PatientValidator : AbstractValidator<PatientDto>
    {
        public PatientValidator()
        {
            const int minChar = 2;

            RuleFor(x => x.GivenName)
                .NotEmpty()
                .WithMessage("Must specify a given name")
                .MinimumLength(minChar)
                .WithMessage($"Minimum length for given name is {minChar}")
                .MaximumLength(50)
                .WithMessage("Maximum length for given name is 50");

            RuleFor(x => x.FamilyName)
                .NotEmpty()
                .WithMessage("Must specify a family name")
                .MinimumLength(minChar)
                .WithMessage($"Minimum length for family name is {minChar}")
                .MaximumLength(50)
                .WithMessage("Maximum length for family name is 50");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Must specify a gender");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .Must(BeWithinRange)
                .WithMessage("DoB must between today and 1900.");

            RuleFor(x => x.HomeAddress)
               .Matches("^\\s*((?:(?:\\d+(?:\\x20+\\w+\\.?)+(?:(?:\\x20+STREET|ST|DRIVE|DR|AVENUE|AVE|ROAD|RD|LOOP|COURT|CT|CIRCLE|LANE|LN|BOULEVARD|BLVD)\\.?)?)|(?:(?:P\\.\\x20?O\\.|P\\x20?O)\\x20*Box\\x20+\\d+)|(?:General\\x20+Delivery)|(?:C[\\\\\\/]O\\x20+(?:\\w+\\x20*)+))\\,?\\x20*(?:(?:(?:APT|BLDG|DEPT|FL|HNGR|LOT|PIER|RM|S(?:LIP|PC|T(?:E|OP))|TRLR|UNIT|\\x23)\\.?\\x20*(?:[a-zA-Z0-9\\-]+))|(?:BSMT|FRNT|LBBY|LOWR|OFC|PH|REAR|SIDE|UPPR))?)\\,?\\s+((?:(?:\\d+(?:\\x20+\\w+\\.?)+(?:(?:\\x20+STREET|ST|DRIVE|DR|AVENUE|AVE|ROAD|RD|LOOP|COURT|CT|CIRCLE|LANE|LN|BOULEVARD|BLVD)\\.?)?)|(?:(?:P\\.\\x20?O\\.|P\\x20?O)\\x20*Box\\x20+\\d+)|(?:General\\x20+Delivery)|(?:C[\\\\\\/]O\\x20+(?:\\w+\\x20*)+))\\,?\\x20*(?:(?:(?:APT|BLDG|DEPT|FL|HNGR|LOT|PIER|RM|S(?:LIP|PC|T(?:E|OP))|TRLR|UNIT|\\x23)\\.?\\x20*(?:[a-zA-Z0-9\\-]+))|(?:BSMT|FRNT|LBBY|LOWR|OFC|PH|REAR|SIDE|UPPR))?)?\\,?\\s+((?:[A-Za-z]+\\x20*)+)\\,\\s+(A[LKSZRAP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])\\s+(\\d+(?:-\\d+)?)\\s*$")
               .WithMessage("Please specify a valid home address");

            RuleFor(x => x.PhoneNumber)
                .Matches(" ^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$ ")
                .WithMessage("Must be a valid phone number.");
        }

        private static bool BeWithinRange(DateTime dateOfBirth)
        {
            var age = DateTime.Today - dateOfBirth;
            var diff = age.TotalDays >= 0 && age.TotalDays <= 36500;
            return (diff);
        }
    }
}
