using Microsoft.Extensions.Configuration;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Core.Contracts.Entities.Authorization;
using SteamWebWrapper.Core.Contracts.Interfaces;
using SteamWebWrapper.Core.Implementations;
using SteamWebWrapper.Implementations;
using System.Net;
using System.Text.Json;

namespace SteamWebWrapper.IntegrationTests.Fixtures;

public class SteamHttpClientFixture : IDisposable
{
	public SteamHttpClientFixture()
	{
		var serializedSteamGuard = Configuration["steamGuard"];

		ISteamGuardAuthenticator? steamGuardAuthenticator =
			JsonSerializer.Deserialize<SteamGuardAuthenticator>(serializedSteamGuard);

		var httpClientHandler = new HttpClientHandler
		{
			CookieContainer = new CookieContainer(),
			UseCookies = true,
			AutomaticDecompression = DecompressionMethods.All
		};

		var steamHttpClient = new SteamHttpClient(httpClientHandler, new SteamConvertor());
		if (!Configuration["username"].IsNullOrEmpty() && !Configuration["password"].IsNullOrEmpty())
		{
			var steamAuthCredentials = new SteamAuthCredentials
			{
				Login = Configuration["username"] ?? throw new InvalidOperationException(),
				Password = Configuration["password"] ?? throw new InvalidOperationException(),
				MachineName = Configuration["machineName"] ?? $"PrettyPC-{Guid.NewGuid()}"
			};

			steamHttpClient
				.AuthorizeViaOAuthAsync(steamAuthCredentials, steamGuardAuthenticator, CancellationToken.None)
				.GetAwaiter().GetResult();
		}

		SteamHttpClient = steamHttpClient;
	}

	public SteamHttpClient SteamHttpClient { get; }

	private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
		.AddUserSecrets<SteamHttpClientFixture>()
		.AddEnvironmentVariables()
		.Build();

	public void Dispose() => SteamHttpClient.Dispose();
}