using CurrencyBot.Models;
using CurrencyBot.Services;
using Moq;

namespace CurrencyBot.Tests
{
    [TestClass]
    public class ExchangeRateResponseTests
    {
        [TestMethod]
        public void ExchangeRateResponseHadlerValidCurrencyTest()
        {
            var exchangeRateResponse = new ExchangeRateResponse
            {
                Date = new DateOnly(2023, 10, 1),
                BaseCurrency = "USD",
                ExchangeRates = new Dictionary<string, CurrencyExchangeRate>
                {
                    { "EUR", new CurrencyExchangeRate { Currency = "EUR", SaleRate = 1.1m, PurchaseRate = 1.0m } }
                }
            };

            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService.Setup(x => x.GetExchangeRateResponse(It.IsAny<DateOnly>())).ReturnsAsync(exchangeRateResponse);

            var exchangeRateResponseHandler = new ExchangeRateResponseHandler(mockExchangeRateService.Object);

            var actualCurrencyExchangeRate = exchangeRateResponseHandler.GetCurerncyExchangeRate("EUR", new DateOnly(2023, 10, 1)).Result;

            Assert.IsNotNull(actualCurrencyExchangeRate);
            Assert.AreEqual("EUR", actualCurrencyExchangeRate.Currency);
            Assert.AreEqual(1.1m, actualCurrencyExchangeRate.SaleRate);
            Assert.AreEqual(1.0m, actualCurrencyExchangeRate.PurchaseRate);
        }

        [TestMethod]
        public void ExchangeRateResponseInvalidCurrency()
        {
            var exchangeRateResponse = new ExchangeRateResponse
            {
                Date = new DateOnly(2023, 10, 1),
                BaseCurrency = "USD",
                ExchangeRates = new Dictionary<string, CurrencyExchangeRate>()
            };

            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService.Setup(x => x.GetExchangeRateResponse(It.IsAny<DateOnly>())).ReturnsAsync(exchangeRateResponse);

            Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
            {
                var exchangeRateResponseHandler = new ExchangeRateResponseHandler(mockExchangeRateService.Object);
                return exchangeRateResponseHandler.GetCurerncyExchangeRate("EUR", new DateOnly(2023, 10, 1));
            });
        }
    }
}
