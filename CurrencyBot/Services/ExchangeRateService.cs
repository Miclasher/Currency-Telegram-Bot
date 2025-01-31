using CurrencyBot.Models;
using System.Text.Json;
using System.Runtime.Caching;

namespace CurrencyBot.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly static CacheItemPolicy CacheItemPolicy = new CacheItemPolicy();
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<ExchangeRateResponse> GetExchangeRateResponse(DateOnly date)
        {
            ExchangeRateResponse? response;
            bool wasCached = false;

            if (_cache.Contains(date.ToString()))
            {
                wasCached = true;
                response = JsonSerializer.Deserialize<ExchangeRateResponse>((string)_cache.Get(date.ToString()));
            }
            else
            {
                response = await FetchExchangeRateResponse(ConvertDateToUrl(date));
                if (response is null || response.ExchangeRates.Count == 0)
                {
                    throw new InvalidOperationException("No exchange rates found for specified date.");
                }
            }

            if (!wasCached)
            {
                _cache.Set(date.ToString(), JsonSerializer.Serialize(response), CacheItemPolicy);
            }

            return response!;
        }

        private async Task<ExchangeRateResponse?> FetchExchangeRateResponse(string url)
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ExchangeRateResponse>(json);
        }

        private static string ConvertDateToUrl(DateOnly date)
        {
            return $"https://api.privatbank.ua/p24api/exchange_rates?date={date:dd.MM.yyyy}";
        }
    }
}
