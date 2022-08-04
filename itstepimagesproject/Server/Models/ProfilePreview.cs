using System.Text.Json.Serialization;

namespace itstepimagesproject.Server.Models
{
    public class ProfilePreview
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }
}
