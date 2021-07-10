using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.VoiceNext;
using LizBot2._1.Commands;
using LizBot2._1.Interfaces;
using LizBot2._1.Manager;
using LizBot2._1.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1
{
    public class Client
    {
        private readonly DiscordClient _discordClient;
        private readonly UserManager _userManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDadJokeService _dadJokeService;
        private List<string> _dadJokes;

        public Client(IServiceProvider serviceProvider, IConfiguration config, UserManager userManager, IDadJokeService dadJokeService)
        {
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _dadJokeService = dadJokeService;
            _dadJokes = new List<string>();
            _discordClient = new DiscordClient(new DiscordConfiguration
            {
                TokenType = TokenType.Bot,
                Token = config["ApiConfig:Discord:DiscordToken"]
            });

            _discordClient.MessageCreated += BaseCommandHandler;

            RegisterCommands();
            SetupVoiceFirst();
        }

        public async Task ConnectClient() => await _discordClient.ConnectAsync();

        private void RegisterCommands()
        {
            var commands = _discordClient.UseCommandsNext(new CommandsNextConfiguration()
            {
                UseDefaultCommandHandler = false,
                Services = _serviceProvider
            });

            commands.RegisterCommands<GreetingCommandModule>();
            commands.RegisterCommands<SpeechCommandModule>();
            commands.RegisterCommands<RandomisationCommandModule>();
            commands.RegisterCommands<UserManagementCommandModule>();
            commands.RegisterCommands<ServerManagementCommandModule>();
            commands.RegisterCommands<WeatherCommandModule>();
            commands.RegisterCommands<FunnyCommandModule>();
            commands.SetHelpFormatter<HelpFormatter>();

            _discordClient.ConnectAsync();
        }

        

        private void SetupVoiceFirst()
        {
            _discordClient.UseVoiceNext(new VoiceNextConfiguration()
            {
                AudioFormat = AudioFormat.Default,
                EnableIncoming = false,
                PacketQueueSize = 25
            });
        }

        private Task BaseCommandHandler(DiscordClient client, MessageCreateEventArgs e)
        {
            var cnext = client.GetCommandsNext();
            var msg = e.Message;
            var prefix = "";
            var cmdString = "";

            var cmdStart = msg.GetStringPrefixLength("-");

            if (cmdStart == -1)
            {
                if (_userManager.IsUserLinkedToChannel(e.Author.Id, e.Channel))
                {
                    prefix = "-";
                    cmdString = "t " + msg.Content;
                }
                else
                {
                    return Task.CompletedTask;
                }
            }
            else
            {
                prefix = msg.Content.Substring(0, cmdStart);
                cmdString = msg.Content.Substring(cmdStart);
            }

            var command = cnext.FindCommand(cmdString, out var args);
            if (command == null) return Task.CompletedTask;

            var ctx = cnext.CreateContext(msg, prefix, command, args);
            Task.Run(async () => await cnext.ExecuteCommandAsync(ctx));

            return Task.CompletedTask;
        }

    }
}
