using itstepimagesproject.Server.Models;
using itstepimagesproject.Server.Services;
using itstepimagesproject.Shared.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutheticationController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly TokenGenerator tokenGenerator;
        private readonly AccountDbContext accountDbContext;
        //private readonly ResourcesDbContext resourcesDbContext;

        public AutheticationController(UserManager<AppUser> userManager,
            TokenGenerator tokenGenerator,
            AccountDbContext accountDbContext)
        {
            this.userManager = userManager;
            this.tokenGenerator = tokenGenerator;
            this.accountDbContext = accountDbContext;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponseDto>> LoginAsync(LoginCredentialsDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.Email);
            if (user == null) return BadRequest();
            if (!await userManager.CheckPasswordAsync(user, loginDto.Password)) return BadRequest();
            var accessToken = tokenGenerator.GenerateAccessToken(user);
            var refreshToken = tokenGenerator.GenerateRefreshToken();
            accountDbContext.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.Now.Add(tokenGenerator.Options.RefreshExpiration),
                AppUserId = user.Id
            });
            await accountDbContext.SaveChangesAsync();

            var response = new AuthenticationResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponseDto>> RegisterAsync(RegistrationCredentialsDto registerCredentials)
        {
            var user = new AppUser
            {
                Email = registerCredentials.Email,
                UserName = registerCredentials.Email
            };

            var result = await userManager.CreateAsync(user, registerCredentials.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var accessToken = tokenGenerator.GenerateAccessToken(user);
            var refreshToken = tokenGenerator.GenerateRefreshToken();

            accountDbContext.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.Now.Add(tokenGenerator.Options.RefreshExpiration),
                AppUserId = user.Id
            });
            accountDbContext.SaveChanges();

            //var profile = new Profile
            //{
            //    Name = registerCredentials.Name,
            //    PhotoUrl = "img/default.png",
            //    AppUserId = user.Id,
            //    UserName = user.UserName
            //};
            //resourcesDbContext.Profiles.Add(profile);
            //await resourcesDbContext.SaveChangesAsync();

            var response = new AuthenticationResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return Ok(response);
        }

        [HttpGet("refresh/{oldRefreshToken}")]
        public async Task<ActionResult<AuthenticationResponseDto>> RefreshAsync(string oldRefreshToken)
        {
            var token = await accountDbContext.RefreshTokens.FindAsync(oldRefreshToken);

            if (token == null)
                return BadRequest();

            accountDbContext.RefreshTokens.Remove(token);

            if (token.ExpiresAt < DateTime.Now)
                return BadRequest();

            var user = await userManager.FindByIdAsync(token.AppUserId);

            var accessToken = tokenGenerator.GenerateAccessToken(user);
            var refreshToken = tokenGenerator.GenerateRefreshToken();

            accountDbContext.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.Now.Add(tokenGenerator.Options.RefreshExpiration),
                AppUserId = user.Id
            });
            accountDbContext.SaveChanges();

            var response = new AuthenticationResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return Ok(response);
        }

        [HttpGet("logout/{refreshToken}")]
        public async Task<IActionResult> LogoutAsync(string refreshToken)
        {
            var token = accountDbContext.RefreshTokens.Find(refreshToken);
            if (token != null)
            {
                accountDbContext.RefreshTokens.Remove(token);
                await accountDbContext.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
