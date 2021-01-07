using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordGui.Test
{
    public class Bot
    {
        private DiscordSocketClient _client { get; set; }
        private CommandService _commandService { get; set; }
        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose
            });
            
            _commandService = new CommandService(new CommandServiceConfig()
            {
                LogLevel = LogSeverity.Verbose
            });
        }

        public async Task MainAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            await _client.LoginAsync(TokenType.Bot, "");
            var gui = new Gui(_client, _commandService);
            gui.StartApplication();
            await Task.Delay(-1);
        }
    }
}