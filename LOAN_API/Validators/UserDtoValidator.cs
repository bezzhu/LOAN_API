using LOAN_API.Models.DTO;
using FluentValidation;

namespace LOAN_API.Validator
{

    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("FirstName is required.");
            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("LastName is required.");
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("UserName is required.");
            RuleFor(u => u.Age)
                .NotEmpty().GreaterThanOrEqualTo(18).WithMessage("Age must be at least 18.");
            RuleFor(u => u.Income)
                .NotEmpty().WithMessage("Income is required.");
            RuleFor(u => u.Email)
                .NotEmpty().EmailAddress().WithMessage("Invalid email address.");
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.");
            
        }
    }
}
