using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using LizBot2._1.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LizBot2._1.Commands
{
    public class SpeechCommandModule : BaseCommandModule
    {
        public UserManager _manager { get; set; }

        [Command("join")]
        [Description​Attribute("Connects the bot to the user's current audio channel. Must be used before using speech commands.")]
        public async Task JoinCommand(CommandContext ctx, DiscordChannel channel = null)
        {
            channel ??= ctx.Member.VoiceState?.Channel;
            await channel.ConnectAsync();
        }

        [Command("t")]
        [Description​Attribute("Speaks given text. Bot must be joined to audio channel first.")]
        public async Task SpeakCommand(CommandContext ctx, [RemainingText] string text)
        {
            if (text.Contains("http"))
            {
                return;
            }

            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);
            var transmit = connection.GetTransmitSink();

            if (text.ToLower().Contains("<") && text.ToLower().Contains(">"))
            {
                text = Regex.Replace(text, @"<.*>", "");
            }

            var user = _manager.GetUser(ctx.User.Id);
            var generatedSpeech = await user.SpeechGenerator.GetSpokenText(text);

            await transmit.WriteAsync(generatedSpeech);
            await transmit.FlushAsync();
        }

        [Command("disconnect")]
        [Description​Attribute("Disconnects bot from current voice channel.")]
        public async Task LeaveCommand(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);

            connection.Disconnect();
        }
    }
}
