using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Text;

namespace LizBot2._1.Extensions
{
    public static class VoiceExtensions
    {
        public static string GetFriendlyName(this VoiceInfo voice)
        {
            var voiceParts = voice.ShortName.Split("-");

            return voiceParts[2];
        }
    }
}
