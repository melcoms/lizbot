using DSharpPlus.Entities;
using LizBot2._1.Entities;
using LizBot2._1.Services;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LizBot2._1.Manager
{
    public class UserManager
    {
        private List<User> _users;
        private VoiceManager _voiceManager;
        private IConfiguration _configuration;

        public UserManager(IConfiguration configuration, VoiceManager voiceManager)
        {
            _users = new List<User>();
            _configuration = configuration;
            _voiceManager = voiceManager;
            _voiceManager.LoadVoices();
        }

        public User GetUser(ulong id)
        {
            var user = _users.FirstOrDefault(a => a.UserId == id);

            if(user == null)
            {
                var defaultVoice = _voiceManager.GetRandomVoice();
                user = new User()
                {
                    UserId = id,
                    Voice = defaultVoice,
                    SpeechGenerator = new SpeechGenerator(defaultVoice, _configuration)
                };

                _users.Add(user);
            }

            return user;
        }

        private void UpdateUser(User user)
        {
            _users.Remove(user);
            _users.Add(user);
        }

        public void ChangeVoice(VoiceInfo voice, ulong userId)
        {
            var foundUser = GetUser(userId);
            foundUser.Voice = voice;
            foundUser.SpeechGenerator = new SpeechGenerator(voice, _configuration);
            UpdateUser(foundUser);
        }

        public VoiceManager GetVoiceManager()
        {
            return _voiceManager;
        }

        public bool IsUserLinkedToChannel(ulong userId, DiscordChannel channel)
        {
            var linkedState = GetUser(userId).LinkingModel;

            if(linkedState == null)
            {
                return false;
            }

            if(linkedState.ListeningChannel.Id == channel.Id)
            {
                return true;
            }

            return false;
        }

        public void LinkUserToChannel(ulong userId, DiscordChannel channel)
        {
            var user = GetUser(userId);

            user.LinkingModel = new LinkedState()
            {
                ListeningChannel = channel
            };

            UpdateUser(user);
        }

        public void UnlinkUser(ulong userId, DiscordChannel channel)
        {
            var user = GetUser(userId);

            user.LinkingModel = null;

            UpdateUser(user);
        }
    }
}
