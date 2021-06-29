using Telegram.Bot;

namespace TelegramBot
{
    public class ProjectModerationBotService
    {
        private readonly ITelegramBotClient _botClient;

        public ProjectModerationBotService(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        
        public void Start()
        {
        }
    }
}