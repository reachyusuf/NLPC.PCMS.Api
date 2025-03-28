using FluentValidation;
using NLPC.PCMS.Common.DTOs.Request;
using NLPC.PCMS.Common.Utilities;

namespace NLPC.PCMS.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Sorry {PropertyName} required")
                .EmailAddress().Must(Utility.IsValidEmail).WithMessage("Sorry Invalid email")
                .MaximumLength(100).WithMessage("Sorry {PropertyName} max length is 100");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Sorry {PropertyName} required")
                //.EmailAddress().Must(Utility.IsValidEmail).WithMessage("Sorry Invalid email")
                .MaximumLength(100).WithMessage("Sorry {PropertyName} max length is 100");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Sorry {PropertyName} required");
        }
    }
}
