using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace itstepimagesproject.Shared.DTO.Validators
{
    public class LoginCredentialsDtoValidator : AbstractValidator<LoginCredentialsDto>
    {
        public LoginCredentialsDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
