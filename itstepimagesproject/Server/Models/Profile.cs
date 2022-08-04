using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Models
{
    public class Profile
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("profileId")]
        public string ProfileId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("smallImage")]
        public string SmallImage { get; set; }
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
        [JsonPropertyName("postsCounter")]
        public int PostsCounter { get; set; }
        [JsonPropertyName("subscriptionsCounter")]
        public int SubscriptionsCounter { get; set; }
        [JsonPropertyName("subscribersCounter")]
        public int SubscribersCounter { get; set; }
    }
}
