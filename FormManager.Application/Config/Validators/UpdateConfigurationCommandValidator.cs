using FluentValidation;
using FormManager.Application.Config.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Config.Validators
{
    public class UpdateConfigurationCommandValidator : AbstractValidator<UpdateConfigurationCommand>
    {
        public UpdateConfigurationCommandValidator()
        {
            RuleFor(x => x.From).EmailAddress();
            RuleFor(x => x.To).EmailAddress();
            RuleFor(x => x.Host).NotEmpty().NotNull();
            RuleFor(x => x.Port).Matches("^[\\d]+$").WithMessage("The input is not a number.");
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
