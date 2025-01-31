using CurrencyBot.Services;
using Microsoft.Extensions.Configuration;

namespace CurrencyBot
{
    internal static class Program
    {
        static async Task Main()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<BotService>()
                .Build();

            using var cts = new CancellationTokenSource();
            var botService = new BotService(config["bot-api-key"]!,
                new ExchangeRateResponseHandler(new ExchangeRateService()));

            await botService.StartAsync(cts.Token);

            Console.WriteLine("Press Enter to terminate");
            Console.ReadLine();

            Console.WriteLine("Bot terminated");
            cts.Cancel();
        }
    }
}
