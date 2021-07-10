using DSharpPlus;
using LizBot2._1.Interfaces;
using LizBot2._1.Manager;
using LizBot2._1.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LizBot2._1
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<UserManager>()
                .AddSingleton<VoiceManager>()
                .AddSingleton<WeatherService>()
                .AddSingleton<Client>()
                .AddSingleton<IConfiguration>(configuration)
                .AddScoped<IDadJokeService, DadJokeService>()
                .AddSingleton<Random>()
                .BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<Client>();

            await Task.Delay(-1);
        }
    }
}
