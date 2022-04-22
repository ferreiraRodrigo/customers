using Customers.Presentation.Dtos;
using FluentValidation;

namespace Customers.Presentation.Validators
{
    public class CreateCustomerDTOValidator : AbstractValidator<CustomerCreationDTO>
    {
        public CreateCustomerDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

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
        }
    }
}
