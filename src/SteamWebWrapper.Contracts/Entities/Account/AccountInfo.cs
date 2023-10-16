using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Account;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class AccountInfo
{
    [JsonPropertyName("wallet_currency")]
    public int WalletCurrency { get; set; }

    [JsonPropertyName("wallet_country")]
    public string WalletCountry { get; set; }

    [JsonPropertyName("wallet_state")]
    public string WalletState { get; set; }

    [JsonPropertyName("wallet_fee")]
    public float WalletFee { get; set; }

    [JsonPropertyName("wallet_fee_minimum")]
    public float WalletFeeMinimum { get; set; }

    [JsonPropertyName("wallet_fee_percent")]
    public float WalletFeePercent { get; set; }

    [JsonPropertyName("wallet_publisher_fee_percent_default")]
    public float WalletPublisherFeePercentDefault { get; set; }

    [JsonPropertyName("wallet_fee_base")]
    public float WalletFeeBase { get; set; }

    [JsonPropertyName("wallet_balance")]
    public long WalletBalance { get; set; }

    [JsonPropertyName("wallet_delayed_balance")]
    public long WalletDelayedBalance { get; set; }

    [JsonPropertyName("wallet_max_balance")]
    public long WalletMaxBalance { get; set; }

    [JsonPropertyName("wallet_trade_max_balance")]
    public long WalletTradeMaxBalance { get; set; }

    [JsonPropertyName("success")]
    public int Success { get; set; }

    [JsonPropertyName("rwgrsn")]
    public int Rwgrsn { get; set; }
    
    [JsonPropertyName("steam_id")]
    public string SteamId { get; set; }
    
    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }
    
    [JsonPropertyName("strlanguage")]
    public string AccountLanguage { get; set; }
    
    [JsonPropertyName("browser_id")]
    public string? BrowserId { get; set; }
    
    [JsonPropertyName("steam_login_secure")]
    public string? SteamLoginSecure { get; set; }
}