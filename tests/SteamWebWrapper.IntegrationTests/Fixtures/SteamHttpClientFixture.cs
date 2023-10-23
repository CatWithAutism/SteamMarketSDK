using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Contracts.Entities.Authorization;
using SteamWebWrapper.Core.Implementations;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.SteamGuard;

namespace SteamWebWrapper.IntegrationTests.Fixtures;

public class SteamHttpClientFixture : IDisposable
{
        
    public ISteamHttpClient SteamHttpClient { get; private set; }
    
    private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .AddUserSecrets<SteamHttpClientFixture>()
        .AddEnvironmentVariables()
        .Build();
    
    public SteamHttpClientFixture()
    {
        var twoFactor = Configuration["twoFactor"];
        var serializedSteamGuard = Configuration["steamGuard"];
        
        ISteamGuardAuthenticator? steamGuardAuthenticator = twoFactor.IsNullOrEmpty() 
            ? JsonSerializer.Deserialize<SteamGuardAuthenticator>(serializedSteamGuard)
            : new SteamGuardTestAuthenticator();


        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer(),
            UseCookies = true,
            AutomaticDecompression = DecompressionMethods.All,
        };

        var steamHttpClientHandler = new SteamHttpClientHandler(httpClientHandler);

        var steamHttpClient = new SteamHttpClient(steamHttpClientHandler);
        var steamAuthCredentials = new SteamAuthCredentials
        {
            Login = Configuration["username"] ?? throw new InvalidOperationException(),
            Password = Configuration["password"] ?? throw new InvalidOperationException(),
            MachineName = Configuration["machineName"] ?? $"PrettyPC-{Guid.NewGuid()}",
            
        };

        steamHttpClient.AuthorizeViaOAuth(steamAuthCredentials, steamGuardAuthenticator, CancellationToken.None).GetAwaiter().GetResult();

        SteamHttpClient = steamHttpClient;
    }
    
    public void Dispose()
    {
        SteamHttpClient.Dispose();
    }

    private class SteamGuardTestAuthenticator : ISteamGuardAuthenticator
    {
        public Task<string> GetDeviceCodeAsync(bool previousCodeWasIncorrect)
        {
            return Task.FromResult(Configuration["twofactor"] ?? throw new InvalidOperationException("Cannot use test authenticator because two factor is null."));
        }

        public Task<string> GetEmailCodeAsync(string email, bool previousCodeWasIncorrect)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AcceptDeviceConfirmationAsync()
        {
            return Task.FromResult(false);
        }

        public string SharedSecret { get; set; }
        public string SerialNumber { get; set; }
        public string RevocationCode { get; set; }
        public string Uri { get; set; }
        public long ServerTime { get; set; }
        public string AccountName { get; set; }
        public string TokenGid { get; set; }
        public string IdentitySecret { get; set; }
        public string Secret1 { get; set; }
        public int Status { get; set; }
        public string DeviceId { get; set; }
        public bool FullyEnrolled { get; set; }
        public SessionData Session { get; set; }
        public Task<bool> DeactivateAuthenticator(int scheme = 1)
        {
            throw new NotImplementedException();
        }

        public string GenerateSteamGuardCode()
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateSteamGuardCodeAsync()
        {
            throw new NotImplementedException();
        }

        public string GenerateSteamGuardCodeForTime(long time)
        {
            throw new NotImplementedException();
        }

        public Confirmation[] FetchConfirmations()
        {
            throw new NotImplementedException();
        }

        public Task<Confirmation[]> FetchConfirmationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AcceptMultipleConfirmations(Confirmation[] confs)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DenyMultipleConfirmations(Confirmation[] confs)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AcceptConfirmation(Confirmation conf)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DenyConfirmation(Confirmation conf)
        {
            throw new NotImplementedException();
        }

        public string GenerateConfirmationUrl(string tag = "conf")
        {
            throw new NotImplementedException();
        }

        public string GenerateConfirmationQueryParams(string tag)
        {
            throw new NotImplementedException();
        }

        public NameValueCollection GenerateConfirmationQueryParamsAsNvc(string tag)
        {
            throw new NotImplementedException();
        }
    }
}