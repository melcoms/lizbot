using FFmpeg.NET;
using LizBot2._1.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace LizBot2._1.Services
{
    public class YouTubeService : IYouTubeService
    {
        private Queue<Stream> _songs;
        private YoutubeClient _client;

        public YouTubeService()
        {
            _songs = new Queue<Stream>();
            _client = new YoutubeClient();
        }

        public async Task<string> EnqueueSong(string searchTerm)
        {
            // This is slow, need to use Google YouTube Data API for searching to select top N records.
            // It currently gets all search results
            var videos = await _client.Search.GetVideosAsync(searchTerm);
            var firstVideo = videos[0];

            var streamManifest = await _client.Videos.Streams.GetManifestAsync(firstVideo.Id);

            var streamInfo = streamManifest.GetAudioOnlyStreams()
                                    .GetWithHighestBitrate();

            var guuid = Guid.NewGuid().ToString();

            var inpath = $"C:\\lizbot\\{guuid}.{streamInfo.Container}";

            await _client.Videos.Streams.DownloadAsync(streamInfo, inpath);

            var ffmpeg = Process.Start(startInfo: new ProcessStartInfo
            {
                FileName = "C:\\ffmpeg\\ffmpeg.exe",
                Arguments = $@"-i ""{inpath}"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            });

            Stream pcm = ffmpeg.StandardOutput.BaseStream;

            _songs.Enqueue(pcm);

            return firstVideo.Title;
        }

        public Stream GetSong()
        {
            return _songs.Dequeue();
        }
    }
}
