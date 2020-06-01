using Discord;
using Discord.Audio;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Bercow_Bot.Services
{
    public class AudioService
    { 
        private readonly ConcurrentDictionary<ulong, IAudioClient> _connectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();
        private readonly DiscordSocketClient _discord;

        public AudioService(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
        }

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            var audioClient = await target.ConnectAsync();

            audioClient.SpeakingUpdated  += AudioClient_SpeakingUpdated;

            _connectedChannels.TryAdd(guild.Id, audioClient);
        }

        private Task AudioClient_SpeakingUpdated(ulong userID, bool speaking)
        {
            SocketUser user = _discord.GetUser(userID);
            Console.WriteLine(user.Username + (speaking ? " started" : " stopped") + " speaking");
            return Task.CompletedTask;
        }
    }
}
