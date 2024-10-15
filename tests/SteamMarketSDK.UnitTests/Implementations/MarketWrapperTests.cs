using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using SteamMarketSDK.Contracts.Entities.Market.MyHistory;
using SteamMarketSDK.Contracts.Interfaces;
using SteamMarketSDK.Core.Contracts.Interfaces;
using SteamMarketSDK.Core.Implementations;
using SteamMarketSDK.Implementations;
using Xunit;

namespace SteamMarketSDK.UnitTests.Implementations;

[TestSubject(typeof(MarketWrapper))]
public class MarketWrapperTests
{
	private Mock<SteamHttpClient> SteamHttpClientMock { get; } = new(MockBehavior.Strict, [new Mock<HttpClientHandler>().Object]);

	[Fact]
	public async Task GetTradeHistoryAsync_Success()
	{
		const int count = 150;
		const int offset = 0;

		var requestUri = GetMyHistoryUri(offset, count);

		var responseContent = await File.ReadAllTextAsync("Data/MarketHistoryResponse.json");
		var historyObject = (new SteamConverter()).DeserializeObject<MyHistoryResponse>(responseContent);

		SteamHttpClientMock.Setup(t => t.GetObjectAsync<MyHistoryResponse>(
				It.Is<string>(reqUri => reqUri.Equals(requestUri, StringComparison.InvariantCultureIgnoreCase)),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(historyObject);

		IMarketWrapper marketWrapper = new MarketWrapper(SteamHttpClientMock.Object);

		var tradeHistory = await marketWrapper.GetTradeHistoryAsync(offset, count, CancellationToken.None);

		SteamHttpClientMock.Verify(t => t.GetObjectAsync<MyHistoryResponse>(
			It.Is<string>(reqUri => reqUri.Equals(requestUri, StringComparison.InvariantCultureIgnoreCase)),
			It.IsAny<CancellationToken>()));

		const int expectedCount = 135;

		tradeHistory.PageSize.Should().Be(count);
		tradeHistory.Events.Count.Should().Be(count);
		tradeHistory.Listings.Count.Should().Be(expectedCount);
		tradeHistory.Assets.Count.Should().Be(expectedCount);
		tradeHistory.Start.Should().Be(offset);
	}


	private string GetMyHistoryUri(int offset, int count) =>
		$"https://steamcommunity.com/market/myhistory/?query=&count={count}&start={offset}&norender=true";
}