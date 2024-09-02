using FluentAssertions;
using Moq;
using Moq.Protected;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;
using SteamWebWrapper.Core.Implementations;
using SteamWebWrapper.Implementations;
using System.Net;
using System.Web;
using Xunit;

namespace SteamWebWrapper.UnitTests.WebWrapper;

public class GetPriceOverviewTests
{
	public GetPriceOverviewTests()
	{
		MockedHandler = new Mock<HttpClientHandler>();
		SteamHttpClient = new SteamHttpClient(MockedHandler.Object, new SteamConverter());
		MarketWrapper = new MarketWrapper(SteamHttpClient);
	}

	private MarketWrapper MarketWrapper { get; }
	private Mock<HttpClientHandler> MockedHandler { get; }
	private SteamHttpClient SteamHttpClient { get; }

	[Fact]
	public async Task GetMarketHistoryInBrazilianDollarsSuccess()
	{
		const int appId = 730;
		const string hashName = "Sticker | oskar | London 2018";
		const string country = "US";
		const int currency = 7;
		var request = new PriceOverviewRequest(appId, hashName, country, currency);

		var reqUri =
			$"https://steamcommunity.com/market/priceoverview/?country={country}&currency={currency}&appid={appId}&market_hash_name={HttpUtility.UrlEncode(hashName)}";
		var httpRequestMessage = ItExpr.Is<HttpRequestMessage>(req =>
			req.RequestUri != null && req.Method == HttpMethod.Get &&
			req.RequestUri.AbsoluteUri.Equals(reqUri, StringComparison.OrdinalIgnoreCase));

		const string rawResponse =
			"{\n    \"success\": true,\n    \"lowest_price\": \"R$ 2,71\",\n    \"volume\": \"3\",\n    \"median_price\": \"R$ 2,68\"\n}";
		var httpResponseMessage = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(rawResponse)
		});

		MockedHandler.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", httpRequestMessage, ItExpr.IsAny<CancellationToken>())
			.Returns(httpResponseMessage);

		var cancellationTokenSource = new CancellationTokenSource();
		var response = await MarketWrapper.GetPriceOverviewAsync(request, cancellationTokenSource.Token);

		const int volume = 3;
		const float lPrice = 2.71f;
		const float mPrice = 2.68f;

		response.Should().NotBeNull();
		response.Success.Should().BeTrue();
		response.Volume.Should().Be(volume);
		response.LowestPrice.Should().Be(lPrice);
		response.MedianPrice.Should().Be(mPrice);
	}

	[Fact]
	public async Task GetMarketHistoryInDollarsSuccess()
	{
		const int appId = 730;
		const string hashName = "Sticker | oskar | London 2018";
		const string country = "US";
		const int currency = 1;
		var request = new PriceOverviewRequest(appId, hashName, country, currency);

		var reqUri =
			$"https://steamcommunity.com/market/priceoverview/?country={country}&currency={currency}&appid={appId}&market_hash_name={HttpUtility.UrlEncode(hashName)}";
		var httpRequestMessage = ItExpr.Is<HttpRequestMessage>(req =>
			req.RequestUri != null && req.Method == HttpMethod.Get &&
			req.RequestUri.AbsoluteUri.Equals(reqUri, StringComparison.OrdinalIgnoreCase));

		const string rawResponse =
			"{\n    \"success\": true,\n    \"lowest_price\": \"$0.52\",\n    \"volume\": \"3\",\n    \"median_price\": \"$0.53\"\n}";
		var httpResponseMessage = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(rawResponse)
		});

		MockedHandler.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", httpRequestMessage, ItExpr.IsAny<CancellationToken>())
			.Returns(httpResponseMessage);

		var cancellationTokenSource = new CancellationTokenSource();
		var response = await MarketWrapper.GetPriceOverviewAsync(request, cancellationTokenSource.Token);

		const int volume = 3;
		const float lPrice = 0.52f;
		const float mPrice = 0.53f;

		response.Should().NotBeNull();
		response.Success.Should().BeTrue();
		response.Volume.Should().Be(volume);
		response.LowestPrice.Should().Be(lPrice);
		response.MedianPrice.Should().Be(mPrice);
	}

	[Fact]
	public async Task GetMarketHistoryInRublesSuccess()
	{
		const int appId = 730;
		const string hashName = "Sticker | oskar | London 2018";
		const string country = "US";
		const int currency = 5;
		var request = new PriceOverviewRequest(appId, hashName, country, currency);

		var reqUri =
			$"https://steamcommunity.com/market/priceoverview/?country={country}&currency={currency}&appid={appId}&market_hash_name={HttpUtility.UrlEncode(hashName)}";
		var httpRequestMessage = ItExpr.Is<HttpRequestMessage>(req =>
			req.RequestUri != null && req.Method == HttpMethod.Get &&
			req.RequestUri.AbsoluteUri.Equals(reqUri, StringComparison.OrdinalIgnoreCase));

		const string rawResponse =
			"{\n    \"success\": true,\n    \"lowest_price\": \"49 pуб.\",\n    \"volume\": \"3\",\n    \"median_price\": \"48,84 pуб.\"\n}";
		var httpResponseMessage = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(rawResponse)
		});

		MockedHandler.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", httpRequestMessage, ItExpr.IsAny<CancellationToken>())
			.Returns(httpResponseMessage);

		var cancellationTokenSource = new CancellationTokenSource();
		var response = await MarketWrapper.GetPriceOverviewAsync(request, cancellationTokenSource.Token);

		const int volume = 3;
		const float lPrice = 49f;
		const float mPrice = 48.84f;

		response.Should().NotBeNull();
		response.Success.Should().BeTrue();
		response.Volume.Should().Be(volume);
		response.LowestPrice.Should().Be(lPrice);
		response.MedianPrice.Should().Be(mPrice);
	}
}