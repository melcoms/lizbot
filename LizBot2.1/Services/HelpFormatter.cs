using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LizBot2._1.Services
{
    public class HelpFormatter : DefaultHelpFormatter
    {
        public HelpFormatter(CommandContext ctx) : base(ctx) { }

        public override CommandHelpMessage Build()
        {
            EmbedBuilder.Color = DiscordColor.SpringGreen;
            return base.Build();
        }
    }
}
