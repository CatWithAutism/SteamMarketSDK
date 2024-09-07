using SteamMarketSDK.Contracts.Entities.Market.AccountInfo;
using SteamMarketSDK.Contracts.Entities.Market.BuyOrderStatus;
using SteamMarketSDK.Contracts.Entities.Market.CancelBuyOrder;
using SteamMarketSDK.Contracts.Entities.Market.CreateBuyOrder;
using SteamMarketSDK.Contracts.Entities.Market.CreateSellOrder;
using SteamMarketSDK.Contracts.Entities.Market.ItemOrdersActivity;
using SteamMarketSDK.Contracts.Entities.Market.MyHistory;
using SteamMarketSDK.Contracts.Entities.Market.MyListings;
using SteamMarketSDK.Contracts.Entities.Market.PriceHistory;
using SteamMarketSDK.Contracts.Entities.Market.PriceOverview;
using SteamMarketSDK.Contracts.Entities.Market.Search;
using SteamMarketSDK.Contracts.Exceptions;
using SteamMarketSDK.Contracts.Interfaces;
using SteamMarketSDK.Core.Contracts.Interfaces;
using SteamMarketSDK.Core.Implementations;
using System.Text.RegularExpressions;
using System.Web;

namespace SteamMarketSDK.Implementations;

public partial class MarketWrapper(SteamHttpClient httpClient) : IMarketWrapper
{
	[GeneratedRegex(@"{\s*\""wallet_currency\""[A-Za-z0-9:\.\s,\""\\_\-]+}")]
	private static partial Regex GetWalletCurrencyRegex();
	
	[GeneratedRegex(@"dateCanUseMarket\s*=\s*new\s*Date\(\""(.+?)\""\)")]
	private static partial Regex GetMarketAvailabilityRegex();

	private static ISteamConverter SteamConverter { get; } = new SteamConverter();

	private string SessionId
	{
		get
		{
			if (string.IsNullOrEmpty(httpClient.SessionId))
			{
				throw new InvalidOperationException(
					$"Your client is not authorized or do not have session id for domain {httpClient.BaseAddress}");
			}

			return httpClient.SessionId;
		}
	}

	public virtual void Dispose() => httpClient.Dispose();

	/// <summary>
	///     Request to cancel buy order.
	/// </summary>
	/// <param name="buyOrderId">Buy order id</param>
	/// <param name="cancellationToken">Cancellation token</param>
	public virtual async Task<CancelBuyOrderResponse> CancelBuyOrderAsync(long buyOrderId, CancellationToken cancellationToken)
	{
		const string requestUri = "https://steamcommunity.com/market/cancelbuyorder/";

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("sessionid", SessionId),
			new KeyValuePair<string, string>("buy_orderid", buyOrderId.ToString())
		});

		var request = GetHttpRequestMessage(requestUri, HttpMethod.Post, content, "https://steamcommunity.com/market/");

		return await httpClient.GetObjectAsync<CancelBuyOrderResponse>(request, cancellationToken);
	}

	/// <summary>
	///     Request to cancel sell order.
	/// </summary>
	public virtual async Task<bool> CancelSellOrderAsync(long listingId, CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/removelisting/{listingId}";

		var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("sessionid", SessionId) });

		var response = await httpClient.PostAsync(requestUri, content, cancellationToken);
		return response.IsSuccessStatusCode;
	}

	/// <summary>
	///     Request to create buy order.
	/// </summary>
	/// <param name="createBuyOrderRequest">Request.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	public virtual async Task<CreateBuyOrderResponse> CreateBuyOrderAsync(CreateBuyOrderRequest createBuyOrderRequest,
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

		var request = GetHttpRequestMessage(requestUri, HttpMethod.Post, content, $"https://steamcommunity.com/market/listings/730/{urlEncodedHashName}");

		return await httpClient.GetObjectAsync<CreateBuyOrderResponse>(request, cancellationToken);
	}

	/// <summary>
	///     Request to create sell order.
	/// </summary>
	public virtual async Task<CreateSellOrderResponse> CreateSellOrderAsync(CreateSellOrderRequest createSellOrderRequest,
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

		var request = GetHttpRequestMessage(requestUri, HttpMethod.Post, content, $"https://steamcommunity.com/id/{httpClient.SteamId}/inventory/");

		return await httpClient.GetObjectAsync<CreateSellOrderResponse>(request, cancellationToken);
	}

	public virtual async Task<AccountInfoResponse> GetAccountInfoAsync(CancellationToken cancellationToken)
	{
		const string infoPage = "https://steamcommunity.com/market/#";

		var stringResponse = await httpClient.GetStringAsync(infoPage, cancellationToken);

		var match = GetWalletCurrencyRegex().Match(stringResponse);
		var accountInfo = SteamConverter.DeserializeObject<AccountInfoResponse>(match.Value);

		match = GetMarketAvailabilityRegex().Match(stringResponse);
		accountInfo.MarketAllowed = !match.Success;

		if (accountInfo.MarketAllowed)
		{
			return accountInfo;
		}

		if (DateTime.TryParse(match.Groups[1].Value, out var marketAvailableDateTime))
		{
			throw new BadSteamResponseDataException($"We can't be able to determine market available date in response: {stringResponse}");
		}
		
		accountInfo.DateCanUseMarket = marketAvailableDateTime; 
		return accountInfo;
	}

	public virtual async Task<BuyOrderStatusResponse> GetBuyOrderStatusAsync(long buyOrderId,
		CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/getbuyorderstatus?" +
		                 $"sessionid={SessionId}&" +
		                 $"buy_orderid={buyOrderId}";

		var request = GetHttpRequestMessage(requestUri, HttpMethod.Get, null, "https://steamcommunity.com/market/");

		return await httpClient.GetObjectAsync<BuyOrderStatusResponse>(request, cancellationToken);
	}

	/// <summary>
	///     Fetch item name id of specified item
	/// </summary>
	/// <param name="appId">AppId of Steam</param>
	/// <param name="marketHashName">Market Hash Name</param>
	/// <param name="cancellationToken">Cancellation token</param>
	public virtual async Task<long> GetItemNameIdAsync(long appId, string marketHashName, CancellationToken cancellationToken)
	{
		const string searchPattern = @"Market_LoadOrderSpread\(\s*(\d+)\s*\)";
		var requestUri = $"https://steamcommunity.com/market/listings/{appId}/{marketHashName}";

		var stringResponse = await httpClient.GetStringAsync(requestUri, cancellationToken);

		var match = Regex.Match(stringResponse, searchPattern);
		if (match.Success)
		{
			return long.Parse(match.Groups[1].Value);
		}

		throw new BadSteamResponseDataException($"We cannot get item name id because it doesn't exist in the response: {stringResponse}");
	}

	public virtual async Task<ItemOrdersActivityResponse> GetItemOrdersActivityAsync(long itemNameId, long currency,
		string language = "english", string country = "US", CancellationToken cancellationToken = default)
	{
		var requestUri = $"https://steamcommunity.com/market/itemordersactivity?" +
		                 $"item_nameid={itemNameId}&" +
		                 $"currency={currency}&" +
		                 $"country={country}&" +
		                 $"language={language}&" +
		                 $"two_factor=0&" +
		                 $"norender=true";

		return await httpClient.GetObjectAsync<ItemOrdersActivityResponse>(requestUri, cancellationToken);
	}

	/// <summary>
	///     Returns current listings and buy orders.
	/// </summary>
	/// <param name="offset">Offset from zero element. Than more than older.</param>
	/// <param name="count">Count of elements. Max size is 500</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns></returns>
	public virtual async Task<MyListingsResponse> GetListingsAsync(long offset, long count, CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/mylistings?" +
		                 $"count={count}&" +
		                 $"start={offset}&" +
		                 $"norender=true";

		return await httpClient.GetObjectAsync<MyListingsResponse>(requestUri, cancellationToken);
	}

	/// <summary>
	///     Returns price history of the market item.
	/// </summary>
	/// <param name="appId">Game id</param>
	/// <param name="marketHashName">Market hash name.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns></returns>
	public virtual async Task<PriceHistoryResponse> GetPriceHistoryAsync(long appId, string marketHashName,
		CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/pricehistory?" +
		                 $"appid={appId}&" +
		                 $"market_hash_name={HttpUtility.UrlEncode(marketHashName)}";

		return await httpClient.GetObjectAsync<PriceHistoryResponse>(requestUri, cancellationToken);
	}

	/// <summary>
	///     Request prive overwive of specified item.
	/// </summary>
	/// <param name="priceRequest">Price request.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Price overview response</returns>
	public virtual async Task<PriceOverviewResponse> GetPriceOverviewAsync(PriceOverviewRequest priceRequest,
		CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/priceoverview/?" +
		                 $"country={priceRequest.Country}&" +
		                 $"currency={priceRequest.Currency}&" +
		                 $"appid={priceRequest.AppId}&" +
		                 $"market_hash_name={HttpUtility.UrlEncode(priceRequest.MarketHashName)}";

		return await httpClient.GetObjectAsync<PriceOverviewResponse>(requestUri, cancellationToken);
	}

	public virtual async Task<MyHistoryResponse> GetTradeHistoryAsync(long offset, long count,
		CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/myhistory/?" +
		                 $"query=&count={count}&" +
		                 $"start={offset}&" +
		                 $"norender=true";

		return await httpClient.GetObjectAsync<MyHistoryResponse>(requestUri, cancellationToken);
	}

	/// <summary>
	///     Do not use it. This method raw as fuck.
	/// </summary>
	public virtual async Task<SearchResponse> SearchItemsAsync(string? query, long offset, long count, string searchData,
		CancellationToken cancellationToken)
	{
		var requestUri = $"https://steamcommunity.com/market/search/render/?" +
		                 $"query={query}&" +
		                 $"start={offset}&" +
		                 $"count={count}&{searchData}" +
		                 $"&norender=true";

		return await httpClient.GetObjectAsync<SearchResponse>(requestUri, cancellationToken);
	}
	
	private static HttpRequestMessage GetHttpRequestMessage(string requestUri, HttpMethod httpMethod, HttpContent? content, string referer)
	{
		var request = new HttpRequestMessage
		{
			RequestUri = new Uri(requestUri),
			Method = httpMethod,
			Content = content,
			Headers = { Referrer =  string.IsNullOrEmpty(referer) ? null : new Uri(referer) }
		};
		return request;
	}
}