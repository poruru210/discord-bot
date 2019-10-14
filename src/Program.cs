using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using DiscordBot.Services;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            using var host = CreateHostBuilder(args).Build();
            await host.Services.GetRequiredService<BotCommandHandler>().InitializeAsync();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureDiscordHost<DiscordSocketClient>((context, configurationBuilder) =>
                {
                    configurationBuilder.SetDiscordConfiguration(new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Verbose,
                        AlwaysDownloadUsers = true,
                        MessageCacheSize = 200
                    });
                    configurationBuilder.SetToken(context.Configuration["token"]);
                })
                .UseCommandService()
                .UseServiceProviderFactory(new DryIocServiceProviderFactory())
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                })
                .ConfigureContainer<Container>((context, container) =>
                {
                    container.Register<BotCommandHandler>(Reuse.Singleton);
                    container.Register<IndicatorService>(Reuse.Singleton);
                })
                .UseConsoleLifetime();
    }
}
