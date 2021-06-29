using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace telegramBot
{
    class Program
    {
        // private static TelegramBotClient client;
        //
        // static void Main(string[] args)
        // {
        //     client = new TelegramBotClient("1839671942:AAFruLDYynRsOvIlpEnf4YIDsTAqdEok2CI");
        //     client.OnMessage += BotOnMessageReceived;
        //     client.OnMessageEdited += BotOnMessageReceived;
        //     client.StartReceiving();
        //     Console.ReadLine();
        //     client.StopReceiving(); 
        //     Console.WriteLine("Hello World!");
        // }
        //
        // private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        // {
        //     var message = messageEventArgs.Message;         
        //     if (message?.Type == MessageType.TextMessage)
        //     {
        //         await client.SendTextMessageAsync(message.Chat.Id, message.Text);
        //     }
        // }
        
        private static ITelegramBotClient _botClient;

        static void Main() {
            _botClient = new TelegramBotClient("1839671942:AAFruLDYynRsOvIlpEnf4YIDsTAqdEok2CI");
            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();
            Console.ReadKey();
            _botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e) {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                await _botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text:   "You said:\n" + e.Message.Text
                );
            }
        }
    }
}
