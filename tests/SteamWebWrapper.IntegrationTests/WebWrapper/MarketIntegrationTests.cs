using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.CreateSellOrder;
using SteamWebWrapper.Contracts.Entities.Market.PriceHistory;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;
using SteamWebWrapper.Contracts.Entities.Market.Search;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Implementations;
using SteamWebWrapper.IntegrationTests.Fixtures;
using SteamWebWrapper.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace SteamWebWrapper.IntegrationTests.WebWrapper;

public class MarketIntegrationTests : IClassFixture<SteamHttpClientFixture>
{
    private ISteamHttpClient SteamHttpClient { get; set; }

    private IMarketWrapper MarketWrapper { get; set; }

    private IInventoryWrapper InventoryWrapper { get; set; }
    
    public MarketIntegrationTests(SteamHttpClientFixture steamHttpClientFixture)
    {
        SteamHttpClient = steamHttpClientFixture.SteamHttpClient;
        InventoryWrapper = new InventoryWrapper(steamHttpClientFixture.SteamHttpClient);
        MarketWrapper = new MarketWrapper(steamHttpClientFixture.SteamHttpClient);
    }

    [Fact]
    public async Task CollectMarketAccountInfoTest()
    {
        var accountInfo = await MarketWrapper.CollectMarketAccountInfoAsync(CancellationToken.None);

        accountInfo.Should().NotBeNull();
        accountInfo.Success.Should().Be(1);
    }
    
    [Fact]
    public async Task GetInventoryTest()
    {
        var inventory = await InventoryWrapper.GetInventory(SteamHttpClient.SteamId.ConvertToUInt64().ToString(), 730, 2, "english", 100);
        
        inventory.Should().NotBeNull();
        inventory.Success.Should().Be(1);
        inventory.Assets.Should().NotBeNullOrEmpty();
        inventory.Descriptions.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetMarketHistoryTest()
    {
        const long offset = 0;
        const long count = 150;
        var marketHistory = await MarketWrapper.GetMarketHistoryAsync(offset, count, CancellationToken.None);
        
        marketHistory.Should().NotBeNull();
        marketHistory.Assets.Should().NotBeNullOrEmpty();
        marketHistory.Assets.Should().NotBeNullOrEmpty();
        marketHistory.Purchases.Should().NotBeNullOrEmpty();
        marketHistory.Listings.Should().NotBeNullOrEmpty();
        marketHistory.Events.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetMyListingsHistoryTest()
    {
        const long offset = 0;
        const long count = 150;
        var marketHistory = await MarketWrapper.GetMyListingsAsync(offset, count, CancellationToken.None);
        
        marketHistory.Should().NotBeNull();
        marketHistory.Success.Should().BeTrue();
        if (marketHistory.Listings.Count > 0)
        {
            marketHistory.Assets.Should().NotBeNullOrEmpty();
        }
    }
    
    [Fact]
    public async Task GetByOrderStatusTest()
    {
        const long offset = 0;
        const long count = 150;
        var marketHistory = await MarketWrapper.GetMyListingsAsync(offset, count, CancellationToken.None);

        marketHistory.Should().NotBeNull();
        marketHistory.Success.Should().BeTrue();

        if (marketHistory.BuyOrders is { Count: > 0 })
        {
            var buyOrderIndex = new Random().Next(0, marketHistory.BuyOrders.Count - 1);
            var buyOrderId = marketHistory.BuyOrders.ElementAt(buyOrderIndex).BuyOrderId;
            var buyOrderStatus = await MarketWrapper.GetBuyOrderStatusAsync(buyOrderId, CancellationToken.None);

            buyOrderStatus.Should().NotBeNull();
            buyOrderStatus.Success.Should().Be(1);
            buyOrderStatus.Active.Should().Be(1);
        }
    }
    
    [Fact]
    public async Task GetPriceOverviewTest()
    {
        const string country = "US";
        const long appId = 730;
        const string marketHashName = "P250 | Sand Dune (Field-Tested)";
        const long currency = 5;

        var priceRequest = new PriceOverviewRequest(appId, marketHashName, country, currency);
        var priceResponse = await MarketWrapper.GetPriceOverviewAsync(priceRequest, CancellationToken.None);

        priceResponse.Should().NotBeNull();
        priceResponse.Success.Should().BeTrue();
    }
    
    [Fact]
    public async Task GetPriceHistoryTest()
    {
        const long appId = 730;
        const string marketHashName = "P250 | Sand Dune (Field-Tested)";
        const int expectedMinCount = 1000;
        
        var priceResponse = await MarketWrapper.GetPriceHistoryAsync(appId, marketHashName, CancellationToken.None);

        priceResponse.Should().NotBeNull();
        priceResponse.Success.Should().BeTrue(); 
        priceResponse.PeriodPrices.Count.Should().BeGreaterThan(expectedMinCount);
    }
    
    [Fact]
    public async Task CreateAndCancelBuyOrderTest()
    {
        var accountInfo = await MarketWrapper.CollectMarketAccountInfoAsync(CancellationToken.None);

        accountInfo.Should().NotBeNull();
        accountInfo.Success.Should().Be(1);
        
        const long minBalance = 300;
        if (!accountInfo.MarketAllowed || accountInfo.WalletBalance < minBalance)
        {
            return;
        }
        
        const long offset = 0;
        const long count = 150;
        var myListings = await MarketWrapper.GetMyListingsAsync(offset, count, CancellationToken.None);
        
        myListings.Should().NotBeNull();
        myListings.Success.Should().BeTrue();
        
        const long appId = 730;
        const long priceTotal = 300;
        const long quantity = 1;
        const string marketHashName = "P250 | Sand Dune (Field-Tested)";
        if (myListings.BuyOrders.Count > 0)
        {
            var existingOrder = myListings.BuyOrders.FirstOrDefault(t => t.HashName == marketHashName);
            if (existingOrder != null)
            {
                var cancelExistingOrder = await MarketWrapper.CancelBuyOrderAsync(existingOrder.BuyOrderId, CancellationToken.None);

                cancelExistingOrder.Should().NotBeNull();
                cancelExistingOrder.Success.Should().Be(1);

                return;
            }
        }

        var createBuyOrderRequest = new CreateBuyOrderRequest(appId, marketHashName, accountInfo.WalletCurrency, priceTotal, quantity);
        var buyOrder = await MarketWrapper.CreateBuyOrderAsync(createBuyOrderRequest, CancellationToken.None);

        buyOrder.Should().NotBeNull();
        buyOrder.Success.Should().Be(1);
        
        var cancelBuyOrder = await MarketWrapper.CancelBuyOrderAsync(buyOrder.BuyOrderId, CancellationToken.None);

        cancelBuyOrder.Should().NotBeNull();
        cancelBuyOrder.Success.Should().Be(1);
    }
    
    [Fact]
    public async Task CreateSellOrderTestFailed()
    {
        var accountInfo = await MarketWrapper.CollectMarketAccountInfoAsync(CancellationToken.None);

        accountInfo.Should().NotBeNull();
        accountInfo.Success.Should().Be(1);

        const long appId = 730;
        const long contextId = 2;
        const long assetId = 30000;
        const long price = 10000;
        const long quantity = 1;
        
        var createSellOrderRequest = new CreateSellOrderRequest(appId, contextId, assetId, price, quantity);
        var sellOrder = await MarketWrapper.CreateSellOrderAsync(createSellOrderRequest, CancellationToken.None);

        sellOrder.Should().NotBeNull();
        sellOrder.Success.Should().BeFalse();
    } 
    
    [Fact]
    public async Task GetItemNameIdTest()
    {
        const long expectedValue = 2383847;
        var itemId = await MarketWrapper.GetItemNameIdAsync(730, "P250 | Sand Dune (Field-Tested)", CancellationToken.None);

        itemId.Should().NotBeNull();
        itemId.Should().Be(expectedValue);
    }
        
    [Fact(Skip = "Is not safe for your money.")]
    public async Task CancelSellOrderTest()
    {
        const long listingId = 4383750158129628696;
        var success = await MarketWrapper.CancelSellOrderAsync(listingId, CancellationToken.None);

        success.Should().BeTrue();
    }
    
    
    /*private DateTime TwoWeekAgo = DateTime.Now.AddDays(-14);
    
    [Fact(Timeout = 999999999)]
    public async Task SearchItemsTest()
    {
        //arm
        // var searchData =
        //      "search_descriptions=0&sort_column=quantity&sort_dir=desc&appid=730&category_730_ItemSet%5B%5D=any&category_730_ProPlayer%5B%5D=any&category_730_StickerCapsule%5B%5D=any&category_730_TournamentTeam%5B%5D=any&category_730_Weapon%5B%5D=any&category_730_Quality%5B%5D=tag_normal&category_730_Rarity%5B%5D=tag_Rarity_Rare_Weapon";
        
        //shirp
        var searchData =
            "search_descriptions=0&sort_column=quantity&sort_dir=desc&appid=730&category_730_ItemSet%5B%5D=any&" +
            "category_730_ProPlayer%5B%5D=any&category_730_StickerCapsule%5B%5D=any&" +
            "category_730_TournamentTeam%5B%5D=any&category_730_Weapon%5B%5D=any&category_730_Quality%5B%5D=tag_normal&" +
            "category_730_Rarity%5B%5D=tag_Rarity_Common_Weapon";
        
        
        //case
        //var searchData = "search_descriptions=0&sort_column=default&sort_dir=desc&appid=730&category_730_ItemSet%5B%5D=any&category_730_ProPlayer%5B%5D=any&category_730_StickerCapsule%5B%5D=any&category_730_TournamentTeam%5B%5D=any&category_730_Weapon%5B%5D=any&category_730_Type%5B%5D=tag_CSGO_Type_WeaponCase";
    
        var query = "";
    
        long offset = 0;
        long count = 100;
    
        List<Data> result = new List<Data>();
    
        long totalCount = 500;
        for (;offset < totalCount; offset+=100)
        {
            try
            {
                var searchResponse = await MarketWrapper.SearchItems(query, offset, count, searchData, CancellationToken.None);
                if (searchResponse.SearchResult == null || searchResponse.SearchResult.Count == 0)
                {
                    throw new InvalidOperationException();
                }
    
                //totalCount = searchResponse.TotalCount;
    
                foreach (var searchMarketItem in searchResponse.SearchResult)
                {
                    int failed = 0;
                    while (true)
                    {
                        try
                        {
                            var priceHistoryResponse = await MarketWrapper.GetPriceHistory(searchMarketItem.Description.AppId, searchMarketItem.HashName, CancellationToken.None);
                            
                            var twoWeeks = priceHistoryResponse.PeriodPrices.Where(t => t.Period > TwoWeekAgo).ToList();
                            var twoWeeksGrouped = twoWeeks.GroupBy(t => t.Period.Date);
                            if (twoWeeks.Count == 0 || twoWeeksGrouped.Count() == 0)
                            {
                                break;
                            }
                            
                            var deviation = StandardDeviation(twoWeeks.Select(t => t.Price));
                            var averageVolume = twoWeeks?.Sum(t => t.Quantity) ?? 0;
    
                            var averageMin = WeightedAverage(twoWeeksGrouped, GetMinAvgValue, GetWeightByDays);
                            var averageMax = WeightedAverage(twoWeeksGrouped, GetMaxAvgValue, GetWeightByDays);
                            var average = WeightedAverage(twoWeeksGrouped, GetAverage, GetWeightByDays);
                    
                            //var currentPrice = await IMarketWrapper.GetPriceOverview(new PriceOverviewRequest(searchMarketItem.Description.AppId, searchMarketItem.HashName, "RU", 5), CancellationToken.None);
    
                            var item = new Data()
                            {
                                Deviation = deviation,
                                Link = $"https://steamcommunity.com/market/listings/{searchMarketItem.Description.AppId}/{searchMarketItem.Description.MarketHashName}",
                                HashName = searchMarketItem.Description.MarketHashName,
                                AverageMinPrice = averageMin,
                                AveragePrice = average,
                                AverageMaxPrice = averageMax,
                                AverageVolume = averageVolume,
                                PercentDeviation = deviation / average * 100,
                                ListingsCount = searchMarketItem.SellListings, 
                            };
                            result.Add(item);
                            
                            Console.WriteLine($"[{DateTime.Now}]Added {item.HashName} - {result.Count}");
                            Thread.Sleep(500);
    
                            failed = 0;
                            break;
                        }
                        catch (Exception e)
                        {
                            failed++;
                            if (failed > 10)
                            {
                                return;
                            }
                            Console.WriteLine(e);
                            Thread.Sleep(15000 * failed);
                        }
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception e)
            {
                offset -= 100;
                Console.WriteLine(e);
                Thread.Sleep(15000);
            }
        }
    
        var sortedResult = result.OrderByDescending(t => t.Deviation).ThenByDescending(t => t.AverageVolume)
            .ThenByDescending(t => t.ListingsCount).ToList();
    
        StringBuilder sb = new StringBuilder();
    
        sb.Append($"Deviation;PercentDeviation;AvgVolume;ListingCount;AvgLowPrice;MedianPrice;AvgMaxPrice;HashName;Link{Environment.NewLine}");
        foreach (var data in sortedResult)
        {
            sb.Append(
                $"{data.Deviation};{data.PercentDeviation};{data.AverageVolume};{data.ListingsCount};{data.AverageMinPrice};{data.AveragePrice};{data.AverageMaxPrice};{data.HashName};{data.Link}{Environment.NewLine}");
        }
        
        File.WriteAllText("result.csv", sb.ToString());
    }
    
    private double GetAverage(IGrouping<DateTime, PriceHistoryPeriodPrice> arg)
    {
        return arg?.Average(t => t.Price) ?? 0;
    }
    
    private double GetWeightByDays(IGrouping<DateTime, PriceHistoryPeriodPrice> arg)
    {
        return (arg.Key.Date - TwoWeekAgo.Date).TotalDays;
    }
    
    private double GetMaxAvgValue(IGrouping<DateTime, PriceHistoryPeriodPrice> arg)
    {
        var anomalyPrices = Anomaly(arg, x => x.Price, 2f);
        return arg.Except(anomalyPrices)
            .OrderByDescending(period => period.Price)
            .First().Price;
    }
    
    private double GetMinAvgValue(IGrouping<DateTime, PriceHistoryPeriodPrice> arg)
    {
        var anomalyPrices = Anomaly(arg, x => x.Price, 2f) ?? new List<PriceHistoryPeriodPrice>();
        return arg
            .Except(anomalyPrices)
            .OrderBy(period => period.Price)
            .First().Price;
    }
    
    public static float WeightedAverage<T>(IEnumerable<T> records, Func<T, double> value, Func<T, double> weight)
    {
        if(records == null)
            throw new ArgumentNullException(nameof(records), $"{nameof(records)} is null.");
    
        int count = 0;
        double valueSum = 0;
        double weightSum = 0;
    
        foreach (var record in records)
        {
            count++;
            double recordWeight = weight(record);
    
            valueSum += value(record) * recordWeight;
            weightSum += recordWeight;
        }
    
        if (count == 0)
            throw new ArgumentException($"{nameof(records)} is empty.");
    
        if (count == 1)
            return (float)value(records.Single());
    
        if (weightSum != 0)
            return (float)(valueSum / weightSum);
        
        throw new DivideByZeroException($"Division of {valueSum} by zero.");
    }
    
    public static IEnumerable<T> Anomaly<T>(IEnumerable<T> source, 
        Func<T, double> map, 
        double maxSigma = 3.0) {
        if (null == source)
            throw new ArgumentNullException(nameof(source));
        else if (null == map)
            throw new ArgumentNullException(nameof(map));
    
        T[] data = source.ToArray();
    
        if (data.Length <= 1)
            yield break;
    
        double s = 0.0;
        double s2 = 0.0;
    
        foreach (var item in data) {
            double x = map(item);
    
            s += x;
            s2 += x * x;
        }
    
        double mean = s / data.Length;
        double sigma = Math.Sqrt(s2 / data.Length - (s / data.Length) * (s / data.Length));
        double leftMargin = mean - maxSigma * sigma;
        double rightMargin = mean + maxSigma * sigma;
    
        foreach (var item in data) {
            double x = map(item);
    
            if (x < leftMargin || x > rightMargin)
                yield return item;
        }
    }
    
    private class Data
    {
        public string Link { get; set; }
        public string HashName { get; set; }
        public float Deviation { get; set; }
        public float AveragePrice { get; set; }
        public float AverageMinPrice { get; set; }
        public float AverageMaxPrice { get; set; }
        public long AverageVolume { get; set; }
        public long ListingsCount { get; set; }
        public float PercentDeviation { get; set; }
    }
    
    static float StandardDeviation(IEnumerable<float> sequence) {
        float result = 0;
    
        if (sequence.Any()) {
            double average = sequence.Average();
            double sum = sequence.Sum(d => Math.Pow(d - average, 2));
            result = (float)(Math.Sqrt((sum) / sequence.Count()));
        }
        return result;
    }*/
}