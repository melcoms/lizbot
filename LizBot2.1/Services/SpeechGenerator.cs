using LizBot2._1.Extensions;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Services
{
    public class SpeechGenerator
    {
        private SpeechSynthesizer _cognitiveSpeechSynthesizer;
        private readonly IConfiguration _config;

        public SpeechGenerator(VoiceInfo voice, IConfiguration config)
        {
            _config = config;
            _cognitiveSpeechSynthesizer = new SpeechSynthesizer(GetSpeechConfig(voice), null);
        }

        public async Task<byte[]> GetSpokenText(string text)
        {
            using var result = await _cognitiveSpeechSynthesizer.SpeakTextAsync(text);
            return result.AudioData.MonoToStereo();
        }


        private SpeechConfig GetSpeechConfig(VoiceInfo voice)
        {
            var speechConfig = SpeechConfig.FromSubscription(_config["ApiConfig:AzureCognitive:Key"], _config["ApiConfig:AzureCognitive:Region"]);
            speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw48Khz16BitMonoPcm);
            speechConfig.SpeechSynthesisVoiceName = voice.Name;
            return speechConfig;
        }
    }
}
