using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordBot.BotCommands
{
    public class Echo : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<Echo> _logger;
        //You can inject the host. This is useful if you want to shutdown the host via a command, but be careful with it.
        private readonly IHost _host;

        public Echo(IHost host, ILogger<Echo> logger)
        {
            _host = host;
            _logger = logger;
        }

        [Command("ping")]
        [Alias("pong", "hello")]
        public async Task PingAsync()
        {
            _logger.LogInformation($"User {Context.User.Username} used the ping command!");
            await ReplyAsync("pong!");
        }
    }
}
