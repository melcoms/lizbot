using LizBot2._1.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Services
{
    public class WeatherService
    {
        private readonly IConfiguration _config;

        public WeatherService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<WeatherResponseModel> GetWeather(string city)
        {
            using var httpClient = new HttpClient();
            using (var response = await httpClient.GetAsync(GetRequestUri(city)))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherResponseModel>(apiResponse);
            }
        }

        private string GetRequestUri(string city)
        {
            return $"http://api.weatherstack.com/current?access_key={_config["ApiConfig:Weather:Token"]}&query={city}";
        }
    }
}
