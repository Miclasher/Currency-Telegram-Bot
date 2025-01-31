using CurrencyBot.Models;
using System.Text.Json;

namespace CurrencyBot.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<ExchangeRateResponse> GetExchangeRateResponse(DateOnly date)
        {
            var response = await FetchExchangeRateResponse(ConvertDateToUrl(date));

            if (response is null || response.ExchangeRates.Count == 0)
            {
                throw new InvalidOperationException("No exchange rates found for specified date.");
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
