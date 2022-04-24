using Customers.Presentation.Dtos.Authentication;
using FluentValidation;

namespace Customers.Presentation.Validators
{
    public class AuthenticationLoginValidator : AbstractValidator<AuthenticationLoginDTO>
    {
        public AuthenticationLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is not valid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.");
            
            RuleFor(x => x.Scopes)
                .NotEmpty()
                .WithMessage("Scopes are required.");
        }
    }
}
