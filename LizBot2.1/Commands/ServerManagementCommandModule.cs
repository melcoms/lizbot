using DSharpPlus;
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
    public class ServerManagementCommandModule : BaseCommandModule
    {
        public UserManager _manager { get; set; }

        [Command("clear")]
        [RequireUserPermissions(Permissions.Administrator)]
        [Description​Attribute("Clears all messages in a channel. Can only be used by administrators.")]
        public async Task ClearHistoryCommand(CommandContext ctx)
        {
            var messages = await ctx.Channel.GetMessagesAsync(1000);

            await ctx.Channel.DeleteMessagesAsync(messages);
        }
    }
}
