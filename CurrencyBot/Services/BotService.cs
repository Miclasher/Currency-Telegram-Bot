using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CurrencyBot.Services
{
    public class BotService
    {
        private readonly TelegramBotClient _botClient;
        private readonly ExchangeRateResponseHandler _exchangeRateResponseHandler;

        public BotService(string token, ExchangeRateResponseHandler exchangeRateResponseHandler)
        {
            _botClient = new TelegramBotClient(token);
            _exchangeRateResponseHandler = exchangeRateResponseHandler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var me = await _botClient.GetMe(cancellationToken);
            Console.WriteLine($"@{me.Username} is running...");

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() },
                cancellationToken
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is null)
            {
                return;
            }

            var words = message.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1 && message.Text == "/start")
            {
                await _botClient.SendMessage(message.Chat.Id, "Welcome to Currency Exchange Bot!\n" +
                    "Send date and currency in following format to get exchange rate to UAH\n" +
                    "USD dd.mm.yyy", cancellationToken: cancellationToken);
            }
            else if (words.Length == 1 && message.Text == "/info")
            {
                await _botClient.SendMessage(message.Chat.Id, """
                    USD - US dollar
                    EUR - euro
                    CHF - Swiss franc
                    GBP - British pound
                    PLN - Polish zloty
                    """, cancellationToken: cancellationToken);
            }
            else if (words.Length == 2 && DateOnly.TryParse(words[1], out DateOnly date))
            {
                try
                {
                    var currency = words[0].ToUpper();

                    var exchangeRate = await _exchangeRateResponseHandler.GetCurerncyExchangeRate(currency, date);

                    await _botClient.SendMessage(message.Chat.Id, $"Exchange rate for {currency} on {date:dd.MM.yyyy} is following\n" +
                        $"Sale rate: {exchangeRate.SaleRate:.##} UAH\n" +
                        $"Purchase rate: {exchangeRate.PurchaseRate:.##} UAH", cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    await _botClient.SendMessage(message.Chat.Id, ex.Message, cancellationToken: cancellationToken);
                }
            }
            else
            {
                await _botClient.SendMessage(message.Chat.Id, "Invalid input. Please send date and currency in following format to get exchange rate to UAH\n" +
                    "USD dd.mm.yyy", cancellationToken: cancellationToken);
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Exception: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
