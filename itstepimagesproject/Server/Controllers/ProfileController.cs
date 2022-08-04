using Azure.Cosmos;
using Azure.Storage.Blobs;
using itstepimagesproject.Server.Domain.Profiles.Commands;
using itstepimagesproject.Server.Domain.Profiles.Queries;
using itstepimagesproject.Server.Models;
using itstepimagesproject.Server.Services;
using itstepimagesproject.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IDistributedCache distributedCache;

        public ProfileController(UserManager<AppUser> userManager,
            IConfiguration configuration,
            IMediator mediator,
            IDistributedCache distributedCache)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.mediator = mediator;
            this.distributedCache = distributedCache;
        }

        // Old methods for CRUD SQL profile

        //[HttpGet]
        //public IEnumerable<Profile> Get()
        //{
        //    return resourcesDbContext.Profiles.ToList();
        //}
        //
        //[HttpGet("{username}")]
        ////[Authorize]
        //public async Task<ActionResult<Profile>> GetByUserNameAsync(string username)
        //{
        //    var profile = await resourcesDbContext.Profiles.FirstOrDefaultAsync(x => x.UserName == username);
        //    var list = resourcesDbContext.Profiles.ToList();
        //    if (profile == null) return NotFound();
        //    return Ok(profile);
        //}
        //
        //[HttpPost]
        //public async Task<ActionResult<Profile>> PostAsync(RegistrationCredentialsDto registrationDto)
        //{
        //    var user = await userManager.FindByNameAsync(registrationDto.Email);
        //    if (user == null) return BadRequest();
        //    var profile = new Profile
        //    {
        //        
        //        Name = registrationDto.Name,
        //        PhotoUrl = "img/default.png",
        //        AppUserId = user.Id,
        //        UserName = user.UserName
        //    };
        //    resourcesDbContext.Profiles.Add(profile);
        //    await resourcesDbContext.SaveChangesAsync();
        //    return Ok(profile);
        //}
        //
        //[HttpPost("upload")]
        ////[Authorize]
        //public async Task<IActionResult> Upload()
        //{
        //    
        //    if (HttpContext.Request.Form.Files.Any())
        //    {
        //        foreach(var file in HttpContext.Request.Form.Files)
        //        {
        //            var newName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            var blobServiceClient = new BlobServiceClient(configuration["BlobStorage"]);
        //            var containerClient = blobServiceClient.GetBlobContainerClient("upload");
        //            var username = HttpContext.User.Identity.Name;
        //            var profile = await resourcesDbContext.Profiles.FirstOrDefaultAsync(p => p.UserName == username);
        //            await containerClient.UploadBlobAsync(newName, file.OpenReadStream());
        //            profile.PhotoUrl = "https://itstepimagesstorage.blob.core.windows.net/upload/" + newName;
        //            resourcesDbContext.Profiles.Update(profile);
        //            await resourcesDbContext.SaveChangesAsync();
        //        }
        //    }
        //    return Ok();
        //}

        // New methods for NOSQL profile

        //[Route("")]
        //[HttpGet]
        //public async Task<Profile> GetById([FromQuery] GetProfileByIdQuery query)
        //{
        //    return await mediator.Send(query);
        //}

        [Route("{id}/name")]
        [HttpPut]
        public void ChangeProfileName(string id, ChangeProfileNameCommand command)
        {
            command.Id = id;
            mediator.Send(command);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateProfileCommand command)
        {
            var user = await userManager.FindByNameAsync(command.Email);
            if (user == null) return BadRequest();
            await mediator.Send(command);
            await distributedCache.RemoveAsync("allProfiles");
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<Profile>> Get()
        {
            IEnumerable<Profile> profiles = null;
            var profilesJson = await distributedCache.GetStringAsync("allProfiles");
            if (string.IsNullOrEmpty(profilesJson))
            {
                profiles = await mediator.Send(new GetAllProfileQuery());
                var jsonToSave = JsonSerializer.Serialize(profiles);
                await distributedCache.SetStringAsync("allProfiles", jsonToSave);
            } else
            {
                profiles = JsonSerializer.Deserialize<IEnumerable<Profile>>(profilesJson);
            }
            return profiles;
        }
        
        [HttpGet("{Name}")]
        //[Authorize]
        public async Task<Profile> GetByNameAsync([FromRoute] GetProfileByNameQuery query)
        {
            try
            {
                return await mediator.Send(query);

            } catch(Exception ex)
            {
                return null;
            }
        }

        [HttpGet("detail/{Email}")]
        //[Authorize]
        public async Task<ActionResult<Profile>> GetByNameAsync([FromRoute] GetProfileByEmailQuery query)
        {
            try
            {
                var user = await userManager.FindByNameAsync(query.Email);
                if (user == null) return BadRequest();
                return await mediator.Send(query);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("test")]
        [HttpGet]
        public async Task<string> Test()
        {
            var time = await distributedCache.GetStringAsync("time");
            if(string.IsNullOrEmpty(time))
            {
                time = DateTime.Now.ToString();
                await distributedCache.SetStringAsync("time", time);
            }
            return time;
        }

        [HttpPost("upload")]
        //[Authorize]
        public async Task<IActionResult> Upload()
        {
            
            if (HttpContext.Request.Form.Files.Any())
            {
                foreach(var file in HttpContext.Request.Form.Files)
                {
                    var newName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var blobServiceClient = new BlobServiceClient(configuration["BlobStorage"]);
                    var containerClient = blobServiceClient.GetBlobContainerClient("upload");
                    var username = HttpContext.User.Identity.Name;
                    await containerClient.UploadBlobAsync(newName, file.OpenReadStream());
                    var newPhotoUrl = "https://itstepimagesstorage.blob.core.windows.net/upload/" + newName;
                    var command = new ChangeProfilePhotoCommand
                    {
                        Name = username,
                        Photo = newPhotoUrl
                    };
                    await mediator.Send(command);
                }
            }
            return Ok();
        }
    }
}