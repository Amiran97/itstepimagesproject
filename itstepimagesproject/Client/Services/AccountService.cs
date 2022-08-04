using itstepimagesproject.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace itstepimagesproject.Client.Services
{
    public class AccountService
    {
        private readonly HttpClient httpClient;
        private readonly TokenAuthenticationStateProvider tokenAuthenticationStateProvider;
        public AccountService(HttpClient httpClient, TokenAuthenticationStateProvider tokenAuthenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.tokenAuthenticationStateProvider = tokenAuthenticationStateProvider;
        }

        public async Task Login(LoginCredentialsDto credentials)
        { 
            var response = await httpClient.PostAsJsonAsync("/api/Authetication/login", credentials);
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
            tokenAuthenticationStateProvider.SetTokenAsync(result.AccessToken, result.RefreshToken);
        }

        public async Task Register(RegistrationCredentialsDto credentials)
        {
            var response = await httpClient.PostAsJsonAsync("/api/Authetication/register", credentials);
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
            tokenAuthenticationStateProvider.SetTokenAsync(result.AccessToken, result.RefreshToken);
        }

        public async Task Logout() 
        {
            await httpClient.GetAsync($"/api/Authetication/logout/{tokenAuthenticationStateProvider.GetRefreshToken()}");
            await tokenAuthenticationStateProvider.RemoveTokens();
        }

        public async Task<List<ProfileDto>> GetAllUsers()
        {
            var list = await httpClient.GetFromJsonAsync<List<ProfileDto>>("/api/Profile");
            return list;
        }

        public async Task<ProfileDto> GetProfile(string email)
        {
            return await httpClient.GetFromJsonAsync<ProfileDto>($"/api/Profile/detail/{email}");
        }
    }
}
