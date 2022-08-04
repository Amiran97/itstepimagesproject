using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace itstepimagesproject.Shared.DTO
{
    public class PostDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("poster")]
        public string Poster { get; set; }
        [JsonPropertyName("commentsCounter")]
        public int СommentsCounter { get; set; }
        [JsonPropertyName("likesCounter")]
        public int LikesCounter { get; set; }
        [JsonPropertyName("author")]
        public ProfileDto Author { get; set; }
    }
}
