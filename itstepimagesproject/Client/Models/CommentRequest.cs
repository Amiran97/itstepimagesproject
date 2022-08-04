using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Client.Models
{
    public class CommentRequest
    {
        public string PostId { get; set; }
        public string ProfileEmail { get; set; }
        public string Message { get; set; }
    }
}
