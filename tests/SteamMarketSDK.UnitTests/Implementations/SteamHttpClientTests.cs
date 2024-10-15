using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Moq.Protected;
using SteamMarketSDK.Contracts.Entities.Inventory;
using SteamMarketSDK.Contracts.Entities.Market.BuyOrderStatus;
using SteamMarketSDK.Contracts.Entities.Market.ItemOrdersActivity;
using SteamMarketSDK.Contracts.Entities.Market.MyHistory;
using SteamMarketSDK.Contracts.Entities.Market.MyListings;
using SteamMarketSDK.Contracts.Entities.Market.PriceHistory;
using SteamMarketSDK.Contracts.Entities.Market.Search;
using SteamMarketSDK.Core.Contracts.Interfaces;
using SteamMarketSDK.Core.Implementations;
using SteamMarketSDK.Implementations;
using System.Net;
using Xunit;

namespace SteamMarketSDK.UnitTests.Implementations;

[TestSubject(typeof(SteamHttpClient))]
public class SteamHttpClientTests
{
	public SteamHttpClientTests()
	{
		MockedHandler = new Mock<HttpClientHandler>();
		SteamHttpClient = new SteamHttpClient(MockedHandler.Object);
	}

	private Mock<HttpClientHandler> MockedHandler { get; }
	private SteamConverter SteamConverter { get; }
	private SteamHttpClient SteamHttpClient { get; }

	public static IEnumerable<object[]> SucceedData()
	{
		const string localhost = "https://localhost";
		yield return [localhost, File.ReadAllText("Data/MarketHistoryResponse.json"), new MyHistoryResponse()];
		yield return [localhost, File.ReadAllText("Data/GetBuyOrderStatusResponse.json"), new BuyOrderStatusResponse()];
		yield return [localhost, File.ReadAllText("Data/GetOrderItemActivityResponse.json"), new ItemOrdersActivityResponse()];
		yield return [localhost, File.ReadAllText("Data/MyListingsResponse.json"), new MyListingsResponse()];
		yield return [localhost, File.ReadAllText("Data/PriceHistoryResponse.json"), new PriceHistoryResponse()];
		yield return [localhost, File.ReadAllText("Data/SearchResponse.json"), new SearchResponse()];
		yield return [localhost, File.ReadAllText("Data/InventoryItemsResponse.json"), new InventoryResponse()];
	}

	[Theory]
	[MemberData(nameof(SucceedData))]
	public async Task GetObjectAsync_Success<T>(string reqUri, string content, T _) where T : notnull
	{
		var httpResponseMessage = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(content)
		});

		var httpRequestMessage = new HttpRequestMessage { RequestUri = new Uri(reqUri), Method = HttpMethod.Get };

		MockedHandler.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync",
				ItExpr.Is<HttpRequestMessage>(message => message.Equals(httpRequestMessage)),
				ItExpr.IsAny<CancellationToken>())
			.Returns(httpResponseMessage);

		var response = await SteamHttpClient.GetObjectAsync<T>(httpRequestMessage, CancellationToken.None);
		response.Should().NotBeNull();

		MockedHandler.Protected().Verify<Task<HttpResponseMessage>>("SendAsync",
			Times.Once(),
			ItExpr.Is<HttpRequestMessage>(message => message.Equals(httpRequestMessage)),
			ItExpr.IsAny<CancellationToken>());
	}
}