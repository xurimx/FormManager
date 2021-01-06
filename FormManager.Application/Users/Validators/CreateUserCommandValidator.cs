using FluentValidation;
using FormManager.Application.Users.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {      
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.Username).MinimumLength(6);
            RuleFor(x => x.Role).Must(x => x.Equals("admin") || x.Equals("user")).WithMessage("Invalid role");
        }
    }
}
