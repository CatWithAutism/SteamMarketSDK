using SteamWebWrapper.Contracts.Entities.Market.AccountInfo;
using SteamWebWrapper.Contracts.Entities.Market.BuyOrderStatus;
using SteamWebWrapper.Contracts.Entities.Market.CancelBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.CreateSellOrder;
using SteamWebWrapper.Contracts.Entities.Market.ItemOrdersActivity;
using SteamWebWrapper.Contracts.Entities.Market.MyHistory;
using SteamWebWrapper.Contracts.Entities.Market.MyListings;
using SteamWebWrapper.Contracts.Entities.Market.PriceHistory;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;
using SteamWebWrapper.Contracts.Entities.Market.Search;
using SteamWebWrapper.Contracts.Exceptions;
using SteamWebWrapper.Contracts.Interfaces;
using SteamWebWrapper.Core.Contracts.Interfaces;
using SteamWebWrapper.Core.Implementations;
using System.Text.RegularExpressions;
using System.Web;

namespace SteamWebWrapper.Implementations;

public partial class MarketWrapper : IMarketWrapper
{
	public MarketWrapper(SteamHttpClient httpClient) => SteamHttpClient = httpClient;

	private static ISteamConverter SteamConverter { get; } = new SteamConverter();
	
	private string SessionId
	{
		get
		{
			var steamCommunity = new Uri("https://steamcommunity.com/");
			return SteamHttpClient.GetSessionId(steamCommunity) ??
			       throw new InvalidOperationException(
				       $"Your client is not authorized or do not have session id for domain {steamCommunity.Host}");
		}
	}

	private SteamHttpClient SteamHttpClient { get; }

	/// <summary>
	///     Request to cancel buy order.
	/// </summary>
	/// <param name="buyOrderId">Buy order id</param>
	/// <param name="cancellationToken">Cancellation token</param>
	public async Task<CancelBuyOrderResponse> CancelBuyOrderAsync(long buyOrderId, CancellationToken cancellationToken)
	{
		const string requestUri = "https://steamcommunity.com/market/cancelbuyorder/";

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("sessionid", SessionId),
			new KeyValuePair<string, string>("buy_orderid", buyOrderId.ToString())
		});

		var request = new HttpRequestMessage
		{
			RequestUri = new Uri(requestUri),
			Method = HttpMethod.Post,
			Content = content,
			Headers = { Referrer = new Uri("https://steamcommunity.com/market/") }
		};

		return await SteamHttpClient.GetObjectAsync<CancelBuyOrderResponse>(request, cancellationToken);
	}

	/// <summary>
	///     Request to cancel sell order.
	/// </summary>
	public async Task<bool> CancelSellOrderAsync(long listingId, CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/removelisting/{listingId}";

		var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("sessionid", SessionId) });

		var response = await SteamHttpClient.PostAsync(requestUri, content, cancellationToken);
		return response.IsSuccessStatusCode;
	}

	/// <summary>
	///     Request to create buy order.
	/// </summary>
	/// <param name="createBuyOrderRequest">Request.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	public async Task<CreateBuyOrderResponse> CreateBuyOrderAsync(CreateBuyOrderRequest createBuyOrderRequest,
		CancellationToken cancellationToken)
	{
		const string requestUri = "https://steamcommunity.com/market/createbuyorder/";
		var urlEncodedHashName = HttpUtility.UrlEncode(createBuyOrderRequest.MarketHashName);

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("sessionid", SessionId),
			new KeyValuePair<string, string>("currency", createBuyOrderRequest.Currency.ToString()),
			new KeyValuePair<string, string>("appid", createBuyOrderRequest.AppId.ToString()),
			new KeyValuePair<string, string>("market_hash_name", createBuyOrderRequest.MarketHashName),
			new KeyValuePair<string, string>("price_total", createBuyOrderRequest.PriceTotal.ToString()),
			new KeyValuePair<string, string>("quantity", createBuyOrderRequest.Quantity.ToString()),
			new KeyValuePair<string, string>("billing_state", string.Empty),
			new KeyValuePair<string, string>("save_my_address", "0")
		});

		var request = new HttpRequestMessage
		{
			RequestUri = new Uri(requestUri),
			Method = HttpMethod.Post,
			Content = content,
			Headers = { Referrer = new Uri($"https://steamcommunity.com/market/listings/730/{urlEncodedHashName}") }
		};

		return await SteamHttpClient.GetObjectAsync<CreateBuyOrderResponse>(request, cancellationToken);
	}

	/// <summary>
	///     Request to create sell order.
	/// </summary>
	public async Task<CreateSellOrderResponse> CreateSellOrderAsync(CreateSellOrderRequest createSellOrderRequest,
		CancellationToken cancellationToken)
	{
		const string requestUri = "https://steamcommunity.com/market/sellitem/";

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("sessionid", SessionId),
			new KeyValuePair<string, string>("appid", createSellOrderRequest.AppId.ToString()),
			new KeyValuePair<string, string>("contextid", createSellOrderRequest.ContextId.ToString()),
			new KeyValuePair<string, string>("assetid", createSellOrderRequest.AssetId.ToString()),
			new KeyValuePair<string, string>("amount", createSellOrderRequest.Quantity.ToString()),
			new KeyValuePair<string, string>("price", createSellOrderRequest.Price.ToString())
		});

		var request = new HttpRequestMessage
		{
			RequestUri = new Uri(requestUri),
			Method = HttpMethod.Post,
			Content = content,
			Headers = { Referrer = new Uri($"https://steamcommunity.com/id/{SteamHttpClient.SteamId}/inventory/") }
		};

		return await SteamHttpClient.GetObjectAsync<CreateSellOrderResponse>(request, cancellationToken);
	}

	public void Dispose() => SteamHttpClient.Dispose();

	public async Task<AccountInfoResponse> GetAccountInfoAsync(CancellationToken cancellationToken)
	{
		const string infoPage = "https://steamcommunity.com/market/#";

		var stringResponse = await SteamHttpClient.GetStringAsync(infoPage, cancellationToken);

		var match = WalletCurrencyRegex().Match(stringResponse);
		var accountInfo = SteamConverter.DeserializeObject<AccountInfoResponse>(match.Value);

		match = Regex.Match(stringResponse, @"dateCanUseMarket\s*=\s*new\s*Date\(\""(.+?)\""\)");
		accountInfo.MarketAllowed = !match.Success;

		if (accountInfo.MarketAllowed)
		{
			return accountInfo;
		}

		accountInfo.DateCanUseMarket = DateTime.Parse(match.Groups[1].Value);
		return accountInfo;
	}

	public async Task<BuyOrderStatusResponse> GetBuyOrderStatusAsync(long buyOrderId,
		CancellationToken cancellationToken)
	{
		var requestUri =
			$"https://steamcommunity.com/market/getbuyorderstatus?sessionid={SessionId}&buy_orderid={buyOrderId}";

		var request = new HttpRequestMessage
		{
			RequestUri = new Uri(requestUri),
			Method = HttpMethod.Get,
			Headers = { Referrer = new Uri($"https://steamcommunity.com/id/{SteamHttpClient.SteamId}/inventory/") }
		};

		return await SteamHttpClient.GetObjectAsync<BuyOrderStatusResponse>(request, cancellationToken);
	}

	/// <summary>
	///     Fetch item name id of specified item
	/// </summary>
	/// <param name="appId">AppId of Steam</param>
	/// <param name="marketHashName">Market Hash Name</param>
	public async Task<long> GetItemNameIdAsync(long appId, string marketHashName, CancellationToken cancellationToken)
	{
		const string searchPattern = @"Market_LoadOrderSpread\(\s*(\d+)\s*\)";
		var requestUri = $"https://steamcommunity.com/market/listings/{appId}/{marketHashName}";

		var stringResponse = await SteamHttpClient.GetStringAsync(requestUri, cancellationToken);

		var match = Regex.Match(stringResponse, searchPattern);
		if (match.Success)
		{
			return long.Parse(match.Groups[1].Value);
		}

		throw new BadSteamResponseDataException(
			$"We cannot get item name id because it doesn't exist in the response. Response: {stringResponse}");
	}

	public async Task<ItemOrdersActivityResponse> GetItemOrdersActivityAsync(long itemNameId, long currency,
		string language = "english", string country = "US", CancellationToken cancellationToken = default)
	{
		var requestUri = $"https://steamcommunity.com/market/itemordersactivity?" +
		                 $"item_nameid={itemNameId}&currency={currency}&country={country}&language={language}&two_factor=0&norender=true";

		return await SteamHttpClient.GetObjectAsync<ItemOrdersActivityResponse>(requestUri, cancellationToken);
	}

	/// <summary>
	///     Returns current listings and buy orders.
	/// </summary>
	/// <param name="offset">Offset from zero element. Than more than older.</param>
	/// <param name="count">Count of elements. Max size is 500</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns></returns>
	public async Task<MyListingsResponse> GetListingsAsync(long offset, long count, CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/mylistings?count={count}&start={offset}&norender=true";

		return await SteamHttpClient.GetObjectAsync<MyListingsResponse>(requestUri, cancellationToken);
	}

	/// <summary>
	///     Returns price history of the market item.
	/// </summary>
	/// <param name="appId">Game id</param>
	/// <param name="marketHashName">Market hash name.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns></returns>
	public async Task<PriceHistoryResponse> GetPriceHistoryAsync(long appId, string marketHashName,
		CancellationToken cancellationToken)
	{
		var requestUri =
			$"https://steamcommunity.com/market/pricehistory?appid={appId}&market_hash_name={HttpUtility.UrlEncode(marketHashName)}";

		return await SteamHttpClient.GetObjectAsync<PriceHistoryResponse>(requestUri, cancellationToken);
	}

	/// <summary>
	///     Request prive overwive of specified item.
	/// </summary>
	/// <param name="priceRequest">Price request.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Price overview response</returns>
	public async Task<PriceOverviewResponse> GetPriceOverviewAsync(PriceOverviewRequest priceRequest,
		CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/priceoverview/?" +
		                 $"country={priceRequest.Country}&" +
		                 $"currency={priceRequest.Currency}&" +
		                 $"appid={priceRequest.AppId}&" +
		                 $"market_hash_name={HttpUtility.UrlEncode(priceRequest.MarketHashName)}";

		return await SteamHttpClient.GetObjectAsync<PriceOverviewResponse>(requestUri, cancellationToken);
	}

	public async Task<MyHistoryResponse> GetTradeHistoryAsync(long offset, long count,
		CancellationToken cancellationToken)
	{
		var requestUri =
			$"https://steamcommunity.com/market/myhistory/?query=&count={count}&start={offset}&norender=true";

		return await SteamHttpClient.GetObjectAsync<MyHistoryResponse>(requestUri, cancellationToken);
	}

	/// <summary>
	///     Do not use it. This method raw as fuck.
	/// </summary>
	public async Task<SearchResponse> SearchItemsAsync(string? query, long offset, long count, string searchData,
		CancellationToken cancellationToken)
	{
		var requestUri =
			$"https://steamcommunity.com/market/search/render/?query={query}&start={offset}&count={count}&{searchData}&norender=true";

		return await SteamHttpClient.GetObjectAsync<SearchResponse>(requestUri, cancellationToken);
	}

	[GeneratedRegex(@"{\s*\""wallet_currency\""[A-Za-z0-9:\.\s,\""\\_\-]+}")]
	private static partial Regex WalletCurrencyRegex();
}