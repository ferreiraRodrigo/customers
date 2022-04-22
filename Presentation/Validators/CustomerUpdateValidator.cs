using Customers.Presentation.Dtos;
using FluentValidation;

namespace Customers.Presentation.Validators
{
    public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateDTO>
    {
        public CustomerUpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
