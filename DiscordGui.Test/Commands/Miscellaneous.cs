using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordGui.Test.Commands
{
    public class Miscellaneous : ModuleBase<SocketCommandContext>
    {
        [Command("latency")]
        [Alias("ping")]
        [Summary("Display My Latency")]
        public async Task LatencyCommand()
        {
            await Context.Channel.SendMessageAsync($"Pong! My latency is {Context.Client.Latency} ms!");
        }
    }
}