using CurrencyBot.Models;

namespace CurrencyBot.Services
{
    public interface IExchangeRateService
    {
        public Task<ExchangeRateResponse> GetExchangeRateResponse(DateOnly date);
    }
}
