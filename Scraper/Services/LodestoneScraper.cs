using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Services
{
    public class LodestoneScraper : ILodestoneScraper
    {
        private readonly LodestoneAPI.Services.ILodestoneAPI _lodestoneAPI;
        private readonly Models.ScrapingOptions _options;
        public LodestoneScraper(LodestoneAPI.Services.ILodestoneAPI lodestoneAPI, IOptions<Models.ScrapingOptions> options)
        {
            _lodestoneAPI = lodestoneAPI ?? throw new ArgumentException(nameof(lodestoneAPI));
            _options = options.Value ?? throw new ArgumentException(nameof(options));
        }

        public async Task Run()
        {
            foreach (var freecompany in _lodestoneAPI.GetFreeCompanies(_options.ServerName))
            {
                foreach (var member in _lodestoneAPI.GetFreeCompanytMembers(freecompany.Id))
                {
                    var character = _lodestoneAPI.GetCharacter(member.Id);
                    Console.WriteLine($"{character.Name}: {character.Race}");

                    //Save to Database
                }
            }
        }
    }
}
