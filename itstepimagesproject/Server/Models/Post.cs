using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Models
{
    public class Post
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("postId")]
        public string PostId { get; set; }
        [JsonPropertyName("poster")]
        public string Poster { get; set; }
        [JsonPropertyName("photos")]
        public List<string> Photos { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
        [JsonPropertyName("commentsCounter")]
        public int CommentsCounter { get; set; }
        [JsonPropertyName("likesCounter")]
        public int LikesCounter { get; set; }
        [JsonPropertyName("author")]
        public ProfilePreview Author { get; set; }
    }
}
