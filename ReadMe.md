# SteamMarketSDK

Here is the library which you can use to interact with your account on [SteamCommunity](https://steamcommunity.com/).


## 2-Factor

First of all you have to get your 2-factor authorization data. You can get it here by [SteamDesktopAuthenticator](https://github.com/Jessecar96/SteamDesktopAuthenticator)

It's time to create your SteamClient.
Now you need to open your maFile and copy your secrets to SteamGuardAuthenticator when you create an instance.

```csharp
		var serializedSteamGuard = Configuration["steamGuard"];

		ISteamGuardAuthenticator? steamGuardAuthenticator =
			JsonSerializer.Deserialize<SteamGuardAuthenticator>(serializedSteamGuard);

		var httpClientHandler = new HttpClientHandler
		{
			CookieContainer = new CookieContainer(),
			UseCookies = true,
			AutomaticDecompression = DecompressionMethods.All
		};

		var steamHttpClient = new SteamHttpClient(httpClientHandler, new StemConverter(), true);
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
        
        //here is you have authorized HTTPClient
```

To be honest you don't need use an authorized client for all operations, but some of them will not work without it.
When your HttpClient is ready you can put in any Wrapper you want.


```csharp
IMarketWrapper MarketWrapper = new MarketWrapper(steamHttpClient);
```

Here is a list of supporting functions:
```csharp
	Task<CancelBuyOrderResponse> CancelBuyOrderAsync(long buyOrderId, CancellationToken cancellationToken);

	Task<bool> CancelSellOrderAsync(long listingId, CancellationToken cancellationToken);

	Task<CreateBuyOrderResponse> CreateBuyOrderAsync(CreateBuyOrderRequest createBuyOrderRequest,
		CancellationToken cancellationToken);

	Task<CreateSellOrderResponse> CreateSellOrderAsync(CreateSellOrderRequest createSellOrderRequest,
		CancellationToken cancellationToken);

	Task<AccountInfoResponse> GetAccountInfoAsync(CancellationToken cancellationToken);

	Task<BuyOrderStatusResponse> GetBuyOrderStatusAsync(long buyOrderId, CancellationToken cancellationToken);

	Task<long> GetItemNameIdAsync(long appId, string marketHashName, CancellationToken cancellationToken);

	Task<ItemOrdersActivityResponse> GetItemOrdersActivityAsync(long itemNameId, long currency,
		string language = "english", string country = "US", CancellationToken cancellationToken = default);

	Task<MyListingsResponse> GetListingsAsync(long offset, long count, CancellationToken cancellationToken);

	Task<PriceHistoryResponse> GetPriceHistoryAsync(long appId, string marketHashName,
		CancellationToken cancellationToken);

	Task<PriceOverviewResponse> GetPriceOverviewAsync(PriceOverviewRequest priceRequest,
		CancellationToken cancellationToken);

	Task<MyHistoryResponse> GetTradeHistoryAsync(long offset, long count, CancellationToken cancellationToken);
```

Other contracts you can find in Contracts -> Interfaces
