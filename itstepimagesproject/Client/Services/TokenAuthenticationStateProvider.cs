using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using OpenQA.Selenium.Html5;
using Blazored.LocalStorage;
using System.Net.Http;

namespace itstepimagesproject.Client.Services
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly HttpClient httpClient;

        public TokenAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            this.localStorage = localStorage;
            this.httpClient = httpClient;
        }

        public async void SetTokenAsync(string accessToken, string refreshToken)
        {
            await localStorage.SetItemAsync("accessToken", accessToken);
            await localStorage.SetItemAsync("refreshToken", refreshToken);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<string> GetAccessToken()
        {
            return await localStorage.GetItemAsync<string>("accessToken");
        }

        public async Task<string> GetRefreshToken()
        {
            return await localStorage.GetItemAsync<string>("refreshToken");
        }

        public async Task RemoveTokens()
        {
            await localStorage.RemoveItemAsync("accessToken");
            await localStorage.RemoveItemAsync("refreshToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetAccessToken();
            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var name = identity.FindFirst("unique_name")?.Value;
            if(!String.IsNullOrWhiteSpace(name))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, identity.FindFirst("unique_name").Value));
            }
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var data = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(data);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4) 
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            
            return Convert.FromBase64String(base64);
        }
    }
}
