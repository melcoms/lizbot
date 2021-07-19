using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using LizBot2._1.Extensions;
using LizBot2._1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Commands
{
    public class FunnyCommandModule : BaseCommandModule
    {
        public IDadJokeService DadJokeService { get; set; }
        public IEvilInsultService EvilInsultService { get; set; }

        [Command("dadjoke")]
        [Description​Attribute("Gets a dad joke.")]
        public async Task DadJokeCommand(CommandContext ctx)
        {
            var dadJoke = DadJokeService.GetDadJoke();

            await ctx.RespondAsync(EmbedExtensions.GetSuperSimpleDiscordEmbed(dadJoke));
        }

        [Command("annoyingping")]
        [Description​Attribute("Pings mentioned user unmercifully for the given number of times (max 20).")]
        public async Task DadJokeCommand(CommandContext ctx, DiscordUser mentioned, int count)
        {
            if (count > 20) count = 20;

            for (int i = 0; i < count; i++)
            {
                var dadJoke = DadJokeService.GetDadJoke();

                await new DiscordMessageBuilder()
                        .WithContent($"Hey, {mentioned.Mention}! {dadJoke}")
                        .WithAllowedMentions(new IMention[] { new UserMention(mentioned) })
                        .SendAsync(ctx.Channel);
            }

            ctx.Message.DeleteAsync();
        }

        [Command("insult")]
        [Description​Attribute("Insults the mentioned user through a DM.")]
        public async Task DadJokeCommand(CommandContext ctx, DiscordMember mentioned)
        {
            var channel = await mentioned.CreateDmChannelAsync();
            var evilInsult = EvilInsultService.GetEvilInsult();
            channel.SendMessageAsync(evilInsult);
            ctx.Message.DeleteAsync();
        }

    }
}
