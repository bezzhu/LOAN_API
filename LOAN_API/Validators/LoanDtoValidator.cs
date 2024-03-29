using LOAN_API.Models;
using LOAN_API.Models.DTO;
using FluentValidation;
using System;

namespace LOAN_API.Validators
{
    public class LoanDtoValidator : AbstractValidator<LoanDto>
    {
        public LoanDtoValidator() {

            RuleFor(x => x.LoanType)
            .NotEmpty().WithMessage("Loan type is required.")
            .Must(x => Enum.IsDefined(typeof(LoanType), x)).WithMessage("Invalid loan type.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.");

            RuleFor(x => x.Period)
                .NotEmpty().WithMessage("Period is required.")
                .GreaterThan(0).WithMessage("Period must be greater than 0.");
        }
    }
}
