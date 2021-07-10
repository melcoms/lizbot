using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Commands
{
    public class GreetingCommandModule : BaseCommandModule
    {
        [Command("hello")]
        [Description​Attribute("Displays a basic greeting to the user")]
        public async Task BasicGreetCommand(CommandContext ctx)
        {
            await ctx.RespondAsync($"Well, hello there, {ctx.Member.DisplayName}! I see you have joined the dark side...");
        }

        [Command("hellothere")]
        public async Task StarWarsGreetCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"General Kenobi!");
            await ctx.Channel.SendMessageAsync($"https://giphy.com/gifs/general-grievous-UIeLsVh8P64G4");
        }
    }
}
