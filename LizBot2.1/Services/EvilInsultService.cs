using LizBot2._1.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LizBot2._1.Services
{
    public class EvilInsultService : IEvilInsultService
    {
        private Queue<string> _evilInsults = new Queue<string>();

        public EvilInsultService()
        {
            PrefetchEvilInsults();
        }

        public string GetEvilInsult()
        {
            if(_evilInsults.Count <= 50)
            {
                PrefetchEvilInsults();
            }
            return _evilInsults.Dequeue();
        }

        private async Task<string> GetEvilInsultsFromApi()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/plain");

            using (var response = await httpClient.GetAsync("https://evilinsult.com/generate_insult.php"))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task PrefetchEvilInsults()
        {
            for (int i = 0; i < 200; i++)
            {
                _evilInsults.Enqueue(await GetEvilInsultsFromApi());
            }
        }
    }
}
