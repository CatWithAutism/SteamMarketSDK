namespace SteamMarketSDK.Contracts.Entities.Market.PriceOverview;

public class PriceOverviewRequest
{
	/// <summary>
	///     Default constructor.
	/// </summary>
	/// <param name="appId">App id of the game.</param>
	/// <param name="marketHashName">Market hash name used in your browser</param>
	/// <param name="country">Any country or your steam account country.</param>
	/// <param name="currency">Currency specified on steam doc</param>
	public PriceOverviewRequest(long appId, string marketHashName, string country, long currency)
	{
		AppId = appId;
		MarketHashName = marketHashName;
		Country = country;
		Currency = currency;
	}

	public long AppId { get; private set; }
	public string Country { get; private set; }
	public long Currency { get; private set; }
	public string MarketHashName { get; private set; }
}