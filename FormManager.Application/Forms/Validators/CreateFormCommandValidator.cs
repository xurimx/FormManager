using FluentValidation;
using FormManager.Application.Forms.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Forms.Validators
{
    public class CreateFormCommandValidator : AbstractValidator<CreateFormCommand>
    {
        public CreateFormCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotEmpty().MinimumLength(3);
            RuleFor(x => x.Telephone).NotEmpty().MinimumLength(9).MaximumLength(15)
                .Matches(@"((^\+\d*)+$)|(^\d+$)").WithMessage("Input string was not a valid phone number.");
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(x => x.Company).NotEmpty().NotNull().MinimumLength(2).MaximumLength(20);
            RuleFor(x => x.Appointment).NotNull().NotEmpty().GreaterThanOrEqualTo(DateTime.Now).LessThanOrEqualTo(DateTime.Now.AddYears(1));
        }
    }
}
