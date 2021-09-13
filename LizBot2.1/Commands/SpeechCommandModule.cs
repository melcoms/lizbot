using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using LizBot2._1.Extensions;
using LizBot2._1.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [Command("rr")]
        [Description​Attribute("try it...?")]
        public async Task RickRollCommand(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);
            var transmit = connection.GetTransmitSink();

            var rickRoll = File.ReadAllBytes($"{Directory.GetCurrentDirectory()}\\Resources\\rickroll.wav").MonoToStereo();

            await transmit.WriteAsync(rickRoll);
            await transmit.FlushAsync();
        }

        [Command("sc")]
        public async Task ScreamingCowboyCommand(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);
            var transmit = connection.GetTransmitSink();

            var screamingCowboy = File.ReadAllBytes($"{Directory.GetCurrentDirectory()}\\Resources\\cowboy.wav");

            await transmit.WriteAsync(screamingCowboy);
            await transmit.FlushAsync();
        }

        [Command("ohdaddy")]
        [RequireRoles(RoleCheckMode.Any, new string[] { "Officer" })]
        public async Task OhDaddyCommand(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);
            var transmit = connection.GetTransmitSink();

            var user = _manager.GetUser(ctx.User.Id);
            var generatedSpeech = await user.SpeechGenerator.GetSpokenText($"Oh daddy! YES!");

            await transmit.WriteAsync(generatedSpeech);
            await transmit.FlushAsync();

            var ohdaddy = File.ReadAllBytes($"{Directory.GetCurrentDirectory()}\\Resources\\moan.wav");

            var voiceChannel = ctx.Member.VoiceState?.Channel;
            var members = voiceChannel.Users;

            var channel = await ctx.Guild.CreateChannelAsync("LIZ FUCK SESH", ChannelType.Voice);
            await ctx.Member.PlaceInAsync(channel);

            foreach (var member in members.Where(a => !a.IsBot))
            {
                await member.SendMessageAsync($"Oh daddy, {member.DisplayName}!");
                await member.SendMessageAsync("https://i.pinimg.com/474x/1b/ad/d7/1badd7d3b2d3bccb414448b23d21a4a3--gifs-posts.jpg");
            }

            await transmit.WriteAsync(ohdaddy);
            await transmit.FlushAsync();

            await channel.DeleteAsync();
        }

    }
}
