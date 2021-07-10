using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LizBot2._1.Extensions
{
    public static class EmbedExtensions
    {
        public static DiscordEmbed GetSimpleDiscordEmbed(string title, string message)
            => new DiscordEmbedBuilder()
                .WithColor(DiscordColor.HotPink)
                .WithTitle(title)
                .WithDescription(message);

        public static DiscordEmbed GetSuperSimpleDiscordEmbed(string message)
           => new DiscordEmbedBuilder()
               .WithColor(DiscordColor.HotPink)
               .WithDescription(message);
    }
}
