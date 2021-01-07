using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordGui.Test
{
    class Program
    {
        static void Main(string[] args)
            => new Bot().MainAsync().GetAwaiter().GetResult();
    }
}