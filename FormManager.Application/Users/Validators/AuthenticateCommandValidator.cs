using FluentValidation;
using FormManager.Application.Users.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Users.Validators
{
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().MinimumLength(5);
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(6);
        }
    }
}
