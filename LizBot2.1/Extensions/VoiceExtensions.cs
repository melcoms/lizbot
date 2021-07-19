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

        public static byte[] MonoToStereo(this byte[] monoAudioIn)
        {
            var outArray = new byte[monoAudioIn.Length * 2];

            for (int i = 0; i < monoAudioIn.Length; i += 2)
            {
                outArray[i * 2 + 0] = monoAudioIn[i];
                outArray[i * 2 + 1] = monoAudioIn[i + 1];
                outArray[i * 2 + 2] = monoAudioIn[i];
                outArray[i * 2 + 3] = monoAudioIn[i + 1];
            }

            return outArray;
        }
    }
}
