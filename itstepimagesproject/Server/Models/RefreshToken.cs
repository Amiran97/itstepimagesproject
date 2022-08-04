using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public string AppUserId { get; set; }
        public DateTime ExpiresAt { get; set; }

        public AppUser AppUser { get; set; }
    }
}
