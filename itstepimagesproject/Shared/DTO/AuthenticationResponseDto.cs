using System;
using System.Collections.Generic;
using System.Text;

namespace itstepimagesproject.Shared.DTO
{
    public class AuthenticationResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
