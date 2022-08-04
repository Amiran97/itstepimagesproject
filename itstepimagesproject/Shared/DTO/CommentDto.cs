using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace itstepimagesproject.Shared.DTO
{
    public class CommentDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("postId")]
        public string PostId { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("author")]
        public ProfileDto Author { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
