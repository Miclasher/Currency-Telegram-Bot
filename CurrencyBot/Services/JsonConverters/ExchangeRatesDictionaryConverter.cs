using CurrencyBot.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyBot.Services.JsonConverters
{
    public class ExchangeRatesDictionaryConverter : JsonConverter<Dictionary<string, CurrencyExchangeRate>>
    {
        public override Dictionary<string, CurrencyExchangeRate>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var exchangeRates = new Dictionary<string, CurrencyExchangeRate>();

            var values = JsonSerializer.Deserialize<List<CurrencyExchangeRate>>(ref reader, options);

            if (values is not null)
            {
                foreach (var rate in values)
                {
                    if (rate.PurchaseRate == 0 || rate.SaleRate == 0)
                    {
                        continue;
                    }

                    exchangeRates.Add(rate.Currency, rate);
                }
            }

            return exchangeRates;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, CurrencyExchangeRate> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Values, options);
        }
    }
}
