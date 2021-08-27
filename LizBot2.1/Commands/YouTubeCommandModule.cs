using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.VoiceNext;
using LizBot2._1.Extensions;
using LizBot2._1.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Commands
{
    public class YouTubeCommandModule : BaseCommandModule
    {
        public YouTubeService Service { get; set; }

        [Command("song")]
        [Description​Attribute("Enqueue Song")]
        public async Task EnqueueSongCommand(CommandContext ctx, [RemainingText] string song)
        {
            var result = await Service.EnqueueSong(song);

            await ctx.RespondAsync(EmbedExtensions.GetSuperSimpleDiscordEmbed(result));
        }

        [Command("play")]
        [Description​Attribute("Launch Queue")]
        public async Task PlayQueueCommand(CommandContext ctx)
        {

            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);
            var transmit = connection.GetTransmitSink();

            var result = Service.GetSong();
            byte[] arrayRes;

            using (var memoryStream = new MemoryStream())
            {
                result.CopyTo(memoryStream);
                arrayRes = memoryStream.ToArray();
            }

            await transmit.WriteAsync(arrayRes);
            await transmit.FlushAsync();
        }
    }
}
