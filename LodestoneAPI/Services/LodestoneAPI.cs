using LodestoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LodestoneAPI.Services
{
    public class LodestoneAPI : ILodestoneAPI
    {
        private readonly HttpClient _client;
        private readonly ILodestoneParser _lodestoneParser;

        public LodestoneAPI(ILodestoneParser lodestoneParser)
        {
            _lodestoneParser = lodestoneParser ?? throw new ArgumentException(nameof(lodestoneParser));

            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://na.finalfantasyxiv.com/lodestone");
            _client.DefaultRequestHeaders.Add("User-Agent", "Free Company Scraper");
        }

        public async Task<Character> GetCharacter(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FreeCompanyEntry>> GetFreeCompanies(string serverName)
        {
            var html = await _client.GetStringAsync($"/freecompany/?house=&character_count=&page=1&worldname={serverName}&join=&order=&q=c&activetime=");
            var result = await _lodestoneParser.ParseFreeCompanies(html);

            return result;
        }

        public async Task<FreeCompany> GetFreeCompany(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FreeCompanyMemberEntry>> GetFreeCompanytMembers(long id)
        {
            throw new NotImplementedException();
        }


    }
}
