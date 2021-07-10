using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using LizBot2._1.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Commands
{
    public class RandomisationCommandModule : BaseCommandModule
    {
        public Random _random { get; set; }

        [Command("flipacoin")]
        [Description​Attribute("Flips a coin. There isn't much more to it.")]
        public async Task FlipCoinCommand(CommandContext ctx)
        {
            var num = _random.Next(0, 2);

            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Blue)
                .WithTitle($"It's {(num == 0 ? "Heads" : "Tails")}");

            if (num == 0)
            {
                embed = embed.WithImageUrl("https://www.nicepng.com/png/detail/438-4383655_its-all-ogre-now-freetoedit-shrek-ogre-head.png");
            }
            else
            {
                embed = embed.WithImageUrl("https://i.redd.it/t0b5f6oyhhx31.jpg");
            }

            ctx.RespondAsync(embed);
        }

        [Command("randomfrom")]
        [Description​Attribute("Randomly selects an item from a list. List items must be comma-seperated.")]
        public async Task RandomSelectionCommand(CommandContext ctx, [RemainingText] string text)
        {
            var items = text.Split(',');

            if (items.Length == 0)
            {
                await ctx.Channel.SendMessageAsync("Usage: -randomfrom [item1], [item2] ...");
            }

            var itemIndex = _random.Next(0, items.Length);

            var item = items[itemIndex].Trim();

            await ctx.Channel.SendMessageAsync($"Guess it'll be {item}.");
        }
    }
}
