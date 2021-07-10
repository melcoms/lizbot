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
        public IDadJokeService Service { get; set; }

        [Command("dadjoke")]
        [Description​Attribute("Gets a dad joke.")]
        public async Task DadJokeCommand(CommandContext ctx)
        {
            var dadJoke = Service.GetDadJoke();

            await ctx.RespondAsync(EmbedExtensions.GetSuperSimpleDiscordEmbed(dadJoke));
        }

        [Command("annoyingping")]
        [Description​Attribute("Pings mentioned user unmercifully for the given number of times (max 20).")]
        public async Task DadJokeCommand(CommandContext ctx, DiscordUser mentioned, int count)
        {
            if (count > 20) count = 20;

            for (int i = 0; i < count; i++)
            {
                var dadJoke = Service.GetDadJoke();

                await new DiscordMessageBuilder()
                        .WithContent($"Hey, {mentioned.Mention}! {dadJoke}")
                        .WithAllowedMentions(new IMention[] { new UserMention(mentioned) })
                        .SendAsync(ctx.Channel);
            }

            ctx.Message.DeleteAsync();
        }

    }
}
