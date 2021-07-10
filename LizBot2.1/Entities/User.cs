using DSharpPlus.Entities;
using LizBot2._1.Services;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LizBot2._1.Entities
{
    public class User : IEquatable<User>
    {
        public ulong UserId { get; set; }
        public VoiceInfo Voice { get; set; }
        public SpeechGenerator SpeechGenerator { get; set; }
        public LinkedState LinkingModel { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public bool Equals([AllowNull] User other)
        {
            return other != null &&
                   UserId == other.UserId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId);
        }
    }

    public class LinkedState
    {
        public DiscordChannel ListeningChannel { get; set; }
    }
}
