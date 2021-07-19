using DSharpPlus;
using LizBot2._1.Interfaces;
using LizBot2._1.Manager;
using LizBot2._1.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LizBot2._1
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(GetBasePath())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<UserManager>()
                .AddSingleton<VoiceManager>()
                .AddSingleton<WeatherService>()
                .AddSingleton<Client>()
                .AddSingleton<IConfiguration>(configuration)
                .AddScoped<IDadJokeService, DadJokeService>()
                .AddScoped<IEvilInsultService, EvilInsultService>()
                .AddSingleton<Random>()
                .BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<Client>();

            await Task.Delay(-1);
        }

        private static string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}