using LizBot2._1.Entities;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Manager
{
    public class VoiceManager
    {
        private List<VoiceInfo> _voices;
        private Random _random;
        private SpeechSynthesizer _cognitiveSpeechSynthesizer;
        private IConfiguration _config;


        public VoiceManager(IConfiguration config)
        {
            _config = config;
            _cognitiveSpeechSynthesizer = new SpeechSynthesizer(GetConfig(), null);
            _voices = new List<VoiceInfo>();
            _random = new Random();
        }

        public List<VoiceInfo> GetListOfVoices() => _voices;

        public VoiceInfo GetVoiceFromListNr(int number)
        {
            return _voices.ElementAt(number - 1);
        }

        public VoiceInfo GetRandomVoice()
        {
            var coreVoices = _voices.Where(a => a.Locale.ToLower() == _config["ApiConfig:AzureCognitive:VoiceFilters:Region"]).ToList();

            var rand = _random.Next(0, coreVoices.Count);

            return coreVoices.ElementAt(rand);
        }

        private SpeechConfig GetConfig() => SpeechConfig.FromSubscription(_config["ApiConfig:AzureCognitive:Key"], _config["ApiConfig:AzureCognitive:Region"]);

        public async Task LoadVoices()
        {
            var result = await _cognitiveSpeechSynthesizer.GetVoicesAsync();
            _voices = result.Voices.ToList().Where(a => a.VoiceType.ToString().ToLower() == _config["ApiConfig:AzureCognitive:VoiceFilters:Type"]).ToList();
        }
    }
}
