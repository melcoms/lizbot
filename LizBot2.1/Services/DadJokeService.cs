using LizBot2._1.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Services
{
    public class DadJokeService : IDadJokeService
    {
        private Queue<string> _dadJokes = new Queue<string>();

        public DadJokeService()
        {
            PrefetchDadJokes();
        }

        public string GetDadJoke()
        {
            if(_dadJokes.Count <= 50)
            {
                PrefetchDadJokes();
            }
            return _dadJokes.Dequeue();
        }

        private async Task<string> GetJokeFromApi()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/plain");

            using (var response = await httpClient.GetAsync("https://icanhazdadjoke.com/"))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task PrefetchDadJokes()
        {
            for (int i = 0; i < 200; i++)
            {
                _dadJokes.Enqueue(await GetJokeFromApi());
            }
        }
    }
}
