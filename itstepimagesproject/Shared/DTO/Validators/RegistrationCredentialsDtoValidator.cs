using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace itstepimagesproject.Shared.DTO.Validators
{
    public class RegistrationCredentialsDtoValidator : AbstractValidator<RegistrationCredentialsDto>
    {
        public RegistrationCredentialsDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty()
                .Equal(x => x.Password);

            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
