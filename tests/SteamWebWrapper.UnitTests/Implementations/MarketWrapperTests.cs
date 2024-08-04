using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using SteamWebWrapper.Contracts.Exceptions;
using SteamWebWrapper.Contracts.Interfaces;
using SteamWebWrapper.Core.Contracts.Interfaces;
using SteamWebWrapper.Core.Implementations;
using SteamWebWrapper.Implementations;
using System.Net;
using Xunit;

namespace SteamWebWrapper.UnitTests.Implementations;

[TestSubject(typeof(MarketWrapper))]
public class MarketWrapperTests
{
	private Mock<ISteamHttpClient> SteamHttpClientMock { get; init; } = new();

	[Fact]
	public async Task GetTradeHistoryAsync_Success()
	{
		const int count = 150;
		const int offset = 0;
		
		var requestUri = GetMyHistoryUri(offset, count);

		var responseContent = await File.ReadAllTextAsync("Data/MarketHistoryResponse.json");
		var response = new HttpResponseMessage { Content = new StringContent(responseContent), StatusCode = HttpStatusCode.OK, };

		SteamHttpClientMock.Setup(t => t.GetAsync(
				It.Is<string>(reqUri => reqUri.Equals(requestUri, StringComparison.InvariantCultureIgnoreCase)),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(response);

		IMarketWrapper marketWrapper = new MarketWrapper(SteamHttpClientMock.Object);

		var tradeHistory = await marketWrapper.GetTradeHistoryAsync(offset, count, CancellationToken.None);

		SteamHttpClientMock.Verify(t => t.GetAsync(
			It.Is<string>(reqUri => reqUri.Equals(requestUri, StringComparison.InvariantCultureIgnoreCase)),
			It.IsAny<CancellationToken>()));
		
		const int expectedCount = 135;

		tradeHistory.PageSize.Should().Be(count);
		tradeHistory.Events.Count.Should().Be(count);
		tradeHistory.Listings.Count.Should().Be(expectedCount);
		tradeHistory.Assets.Count.Should().Be(expectedCount);
		tradeHistory.Start.Should().Be(offset);
	}
	
	[Fact]
	public async Task GetTradeHistoryAsync_ShouldThrowExceptionOnEmptyResponse()
	{
		const int count = 150;
		const int offset = 0;

		var requestUri = GetMyHistoryUri(offset, count);
		var response = new HttpResponseMessage { Content = new StringContent("null"), StatusCode = HttpStatusCode.OK, };

		SteamHttpClientMock.Setup(t => t.GetAsync(
				It.Is<string>(reqUri => reqUri.Equals(requestUri, StringComparison.InvariantCultureIgnoreCase)),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(response);

		IMarketWrapper marketWrapper = new MarketWrapper(SteamHttpClientMock.Object);

		var func = async () => await marketWrapper.GetTradeHistoryAsync(offset, count, CancellationToken.None);
		await func.Should().ThrowAsync<BadSteamResponseDataException>();

		SteamHttpClientMock.Verify(t => t.GetAsync(
			It.Is<string>(reqUri => reqUri.Equals(requestUri, StringComparison.InvariantCultureIgnoreCase)),
			It.IsAny<CancellationToken>()));
	}
	
	private string GetMyHistoryUri(int offset, int count) => $"https://steamcommunity.com/market/myhistory/?query=&count={count}&start={offset}&norender=true";
}