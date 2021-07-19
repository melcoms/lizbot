using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using LizBot2._1.Extensions;
using LizBot2._1.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Commands
{
    public class UserManagementCommandModule : BaseCommandModule
    {
        public UserManager _manager { get; set; }

        [Command("getvoices")]
        [Description​Attribute("Get list of voices available for bot.")]
        public async Task GetVoicesCommand(CommandContext ctx)
        {
            var voices = _manager.GetVoiceManager().GetListOfVoices();

            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.HotPink)
                .WithTitle("Supported Voices");

            foreach (var voice in voices)
            {
                embed = embed.AddField($"Voice {voices.IndexOf(voice) + 1}", $"{voice.GetFriendlyName()}");
            };

            await ctx.RespondAsync(embed);
        }

        [Command("selectvoice")]
        [Description​Attribute("Select a voice from the voice list (-getvoices) for bot to use when speaking for you.")]
        public async Task SelectVoiceCommand(CommandContext ctx, int voiceNr)
        {
            var voice = _manager.GetVoiceManager().GetVoiceFromListNr(voiceNr);
            _manager.ChangeVoice(voice, ctx.Member.Id);
            await ctx.RespondAsync($"You now have {voice.GetFriendlyName()} as a voice.");
        }

        [Command("myvoice")]
        [Description​Attribute("Gets your currently assigned bot voice.")]
        public async Task SelectVoiceCommand(CommandContext ctx)
        {
            var user = _manager.GetUser(ctx.User.Id);

            await ctx.RespondAsync($"You current voice is {user.Voice.GetFriendlyName()}.");
        }

        [Command("link")]
        [Description​Attribute("Link voice reader to you in the current channel. All normal messages you send will be read out.")]
        public async Task LinkUserCommand(CommandContext ctx)
        {
            if (ctx.Member.VoiceState?.Channel == null)
            {
                await ctx.RespondAsync(EmbedExtensions.GetSuperSimpleDiscordEmbed("Please connect to a voice channel before using the link command"));
                return;
            }
            _manager.LinkUserToChannel(ctx.User.Id, ctx.Channel);
            var channel = ctx.Member.VoiceState?.Channel;
            await channel.ConnectAsync();
            await ctx.RespondAsync(EmbedExtensions.GetSuperSimpleDiscordEmbed($"I'll now read everything you send to {ctx.Channel.Name}."));
        }

        [Command("unlink")]
        [Description​Attribute("Unlink voice reader from you.")]
        public async Task UnLinkUserCommand(CommandContext ctx)
        {
            _manager.UnlinkUser(ctx.User.Id, ctx.Channel);

            await ctx.RespondAsync(EmbedExtensions.GetSuperSimpleDiscordEmbed("I'll now stop reading your text unless you use the explicit -t command"));
        }
    }
}
