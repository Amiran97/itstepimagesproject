using itstepimagesproject.Server.Domain.Profiles.Commands;
using itstepimagesproject.Server.Domain.Profiles.Queries;
using itstepimagesproject.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IDistributedCache distributedCache;

        public PostController(UserManager<AppUser> userManager,
            IConfiguration configuration,
            IMediator mediator,
            IDistributedCache distributedCache)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.mediator = mediator;
            this.distributedCache = distributedCache;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePostCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [Route("")]
        [HttpGet]
        public async Task<IEnumerable<Post>> GetAll()
        {
            return await mediator.Send(new GetAllPostQuery());
        }

        [Route("{PostId}")]
        [HttpGet]
        public async Task<Post> GetAll([FromRoute] GetPostByIdQuery query)
        {
            return await mediator.Send(query);
        }
    }
}
