using CurrencyBot.Models;
using CurrencyBot.Services.JsonConverters;
using System.Text.Json;

namespace CurrencyBot.Tests
{
    [TestClass]
    public class ExchangeRatesDictionaryConverterTests
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new ExchangeRatesDictionaryConverter() }
        };

        [TestMethod]
        public void DeserializeValidJson()
        {
            const string json = """
        [
            { "currency": "USD", "saleRateNB": 15.05, "purchaseRateNB": 15.05, "saleRate": 15.7, "purchaseRate": 15.35 },
            { "currency": "EUR", "saleRateNB": 18.79, "purchaseRateNB": 18.79, "saleRate": 20.0, "purchaseRate": 19.2 }
        ]
        """;

            var result = JsonSerializer.Deserialize<Dictionary<string, CurrencyExchangeRate>>(json, _options);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("USD"));
            Assert.IsTrue(result.ContainsKey("EUR"));

            Assert.AreEqual(15.7m, result["USD"].SaleRate);
            Assert.AreEqual(19.2m, result["EUR"].PurchaseRate);
        }

        [TestMethod]
        public void SerializeValidDictionary()
        {
            var exchangeRates = new Dictionary<string, CurrencyExchangeRate>
            {
                ["USD"] = new() { Currency = "USD", SaleRate = 15.7m, PurchaseRate = 15.35m },
                ["EUR"] = new() { Currency = "EUR", SaleRate = 20.0m, PurchaseRate = 19.2m }
            };

            string json = JsonSerializer.Serialize(exchangeRates, _options);

            Assert.IsTrue(json.Contains("\"currency\":\"USD\""));
            Assert.IsTrue(json.Contains("\"saleRate\":15.7"));
            Assert.IsTrue(json.Contains("\"purchaseRate\":19.2"));
        }

        [TestMethod]
        public void DeserializeEmptyJson()
        {
            const string json = "[]";

            var result = JsonSerializer.Deserialize<Dictionary<string, CurrencyExchangeRate>>(json, _options);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void DeserializeInvalidJson()
        {
            const string json = """
        [
            { "currency": "USD", "saleRate": "INVALID_VALUE" }
        ]
        """;

            Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<Dictionary<string, CurrencyExchangeRate>>(json, _options));
        }
    }
}
