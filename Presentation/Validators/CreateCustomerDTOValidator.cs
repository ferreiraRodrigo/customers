using Customers.Presentation.Dtos;
using FluentValidation;

namespace Customers.Presentation.Validators
{
    public class CreateCustomerDTOValidator : AbstractValidator<CreateCustomerDTO>
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
        }
    }
}
