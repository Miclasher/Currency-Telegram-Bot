using CurrencyBot.Models;

namespace CurrencyBot.Services
{
    public class ExchangeRateResponseHandler
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateResponseHandler(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<CurrencyExchangeRate> GetCurerncyExchangeRate(string currency, DateOnly date)
        {
            var response = await _exchangeRateService.GetExchangeRateResponse(date);

            if (!response.ExchangeRates.TryGetValue(currency, out CurrencyExchangeRate? value))
            {
                throw new InvalidOperationException($"No exchange rate found for currency {currency}. Use /info to get list of supported currencies.");
            }

            return value;
        }
    }
}
