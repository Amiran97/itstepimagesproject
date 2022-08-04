using System;
using System.Collections.Generic;
using System.Text;

namespace itstepimagesproject.Shared.DTO
{
    public class RegistrationCredentialsDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Name { get; set; }
    }
}
