using itstepimagesproject.Server.Domain.Profiles.Commands;
using itstepimagesproject.Server.Domain.Profiles.Queries;
using itstepimagesproject.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly IMediator mediator;
        public CommentController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [Route("{PostId}")]
        [HttpGet]
        public async Task<IEnumerable<Comment>> GetComments([FromRoute] GetPostCommentsByIdQuery query)
        {
            return await mediator.Send(query);
        }

        [Route("")]
        [HttpPost]
        public async Task AppendComment(CreateCommentInPostCommand command)
        {
            await mediator.Send(command);
        }
    }
}
