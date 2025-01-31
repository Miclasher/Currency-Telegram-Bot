using System.Text.Json.Serialization;

namespace CurrencyBot.Models
{
    public class CurrencyExchangeRate
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("saleRate")]
        public decimal SaleRate { get; set; }
        [JsonPropertyName("purchaseRate")]
        public decimal PurchaseRate { get; set; }
    }
}
