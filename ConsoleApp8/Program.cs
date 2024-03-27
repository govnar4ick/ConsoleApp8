using Microsoft.VisualBasic;
using System.Collections;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

internal class Program
{
    private static void Main(string[] args)
    {
        Start();
        Console.ReadLine();
    }
    private static async void Start()
    {
        var botClient = new TelegramBotClient("7196084162:AAF1OI_cJwaBnQCfFm7XgZASMxfk87oy1YA");

        using CancellationTokenSource cts = new();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        // Send cancellation request to stop bot
        cts.Cancel();
    }
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;
        if (message.Text == "/start")
        {
           await CreateKeyBoard(botClient, update, cancellationToken);
        }
        var chatId = message.Chat.Id;
        if (message.Text == "CATALOG")
        {
            await botClient.SendPhotoAsync(
            chatId: chatId,
            photo: InputFile.FromUri("https://raw.githubusercontent.com/govnar4ick/photo/main/armawear_stiker_tshirt.jpg"),
            caption: "armawear 'STICKER' T - SHIRT, 15$",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
            await botClient.SendPhotoAsync(
            chatId: chatId,
            photo: InputFile.FromUri("https://raw.githubusercontent.com/govnar4ick/photo/main/armawear_stiker_tshirt.jpg"),
            caption: "armawear 'STICKER' T - SHIRT, 15$",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
            return;
        }
        if (message.Text == "CONTACTS")
        {
            string conact = "Контакты администраторов - " +
                "Я - @khorovadz" +" "+
                "Жамо - @term1x";
            Message mess = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: conact,
            cancellationToken: cancellationToken);
            return;


        }
        //    Message message = await botClient.SendPhotoAsync(
        //chatId: chatId,
        //photo: InputFile.FromUri("https://raw.githubusercontent.com/govnar4ick/PHOTOS/main/armawear_stiker_tshirt.jpg?token=GHSAT0AAAAAACQGAI7GBUOLU2IFCUK2ECLIZQEBUBA"),
        //caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
        //parseMode: ParseMode.Html,
        //cancellationToken: cancellationToken);


        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
        //int meID = 1270728099;
        //if (chatId == meID)
        //{
        //    string answer = "Hi admin!!";
        //    if (message.Text == "Hi")
        //    {
        //        answer = "Hi!";
        //    }
       // Message mess = await botClient.SendTextMessageAsync(
       //chatId: chatId,
       //text: answer,
       //cancellationToken: cancellationToken);
       // return;
        //}
        // Echo received message text
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You said:\n" + messageText,
            cancellationToken: cancellationToken);
    }
    private static async Task CreateKeyBoard(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "CATALOG" },
                new KeyboardButton[] { "NEWS" },
                new KeyboardButton[] { "CONTACTS" }
            })
            {
                ResizeKeyboard = true
            };

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Choose a response",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
    //private static async Task SendPhotoAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    //{

    //}
}
