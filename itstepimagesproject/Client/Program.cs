using Blazored.LocalStorage;
using FluentValidation;
using itstepimagesproject.Client.Services;
using itstepimagesproject.Shared.DTO;
using itstepimagesproject.Shared.DTO.Validators;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace itstepimagesproject.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient<IValidator<LoginCredentialsDto>, LoginCredentialsDtoValidator>();
            builder.Services.AddTransient<IValidator<RegistrationCredentialsDto>, RegistrationCredentialsDtoValidator>();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<TokenAuthenticationStateProvider, TokenAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetService<TokenAuthenticationStateProvider>());

            //builder.Services.AddScoped<TokenAuthenticationStateProvider>();
            //builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
            await builder.Build().RunAsync();
        }
    }
}
