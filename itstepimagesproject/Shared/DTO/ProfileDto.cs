using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace itstepimagesproject.Shared.DTO
{
    public class ProfileDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
        public string AppUserId { get; set; }
    }
}
