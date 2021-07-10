using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using LizBot2._1.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Commands
{
    public class WeatherCommandModule : BaseCommandModule
    {
        public WeatherService Service { get; set; }

        [Command("weather")]
        [Description​Attribute("Gets current weather in city/town.")]
        public async Task WeatherForCityCommand(CommandContext ctx, [RemainingText] string city)
        {
            var currentWeather = await Service.GetWeather(city);

            var weatherEmbed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Blue)
                .WithTitle($"Weather In {currentWeather.Request.Query}")
                .AddField("Temperature", $"{currentWeather.Current.Temperature}°C", true)
                .AddField("Humidity", $"{currentWeather.Current.Humidity}%", true)
                .AddField("Feels Like", $"{currentWeather.Current.Feelslike}°C", true);

            await ctx.RespondAsync(weatherEmbed);
        }
    }
}
