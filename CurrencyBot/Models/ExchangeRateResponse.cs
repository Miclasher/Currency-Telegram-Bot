using CurrencyBot.Services.JsonConverters;
using System.Text.Json.Serialization;

namespace CurrencyBot.Models
{
    public class ExchangeRateResponse
    {
        [JsonPropertyName("date")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly Date { get; set; }
        [JsonPropertyName("baseCurrencyLit")]
        public string BaseCurrency { get; set; } = string.Empty;
        [JsonPropertyName("exchangeRate")]
        [JsonConverter(typeof(ExchangeRatesDictionaryConverter))]
        public Dictionary<string, CurrencyExchangeRate> ExchangeRates { get; set; } = new Dictionary<string, CurrencyExchangeRate>();
    }
}
