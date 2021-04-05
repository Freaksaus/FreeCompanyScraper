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
        private readonly Data.Services.Services.IFreeCompanyService _freeCompanyService;
        private readonly Data.Services.Services.ICharacterService _characterService;
        private readonly Models.ScrapingOptions _options;
        public LodestoneScraper(LodestoneAPI.Services.ILodestoneAPI lodestoneAPI,
            Data.Services.Services.IFreeCompanyService freeCompanyService,
            Data.Services.Services.ICharacterService characterService,
            IOptions<Models.ScrapingOptions> options)
        {
            _lodestoneAPI = lodestoneAPI ?? throw new ArgumentException(nameof(lodestoneAPI));
            _freeCompanyService = freeCompanyService ?? throw new ArgumentException(nameof(freeCompanyService));
            _characterService = characterService ?? throw new ArgumentException(nameof(characterService));
            _options = options.Value ?? throw new ArgumentException(nameof(options));
        }

        public async Task Run()
        {
            foreach (var freecompany in await _lodestoneAPI.GetFreeCompanies(_options.ServerName))
            {
                _freeCompanyService.Add(ConvertToModel(freecompany));

                foreach (var member in await _lodestoneAPI.GetFreeCompanyMembers(freecompany.Id))
                {
                    var character = await _lodestoneAPI.GetCharacter(member.Id);
                    _characterService.Add(ConvertToModel(character));
                }
            }
        }

        private Domain.Models.FreeCompany ConvertToModel(LodestoneAPI.Models.FreeCompanyEntry freeCompany)
        {
            return new Domain.Models.FreeCompany()
            {
                DateCreated = freeCompany.DateCreated,
                DateScraped = DateTime.Now,
                Id = freeCompany.Id,
                MemberCount = freeCompany.MemberCount,
                Name = freeCompany.Name,
            };
        }

        private Domain.Models.Character ConvertToModel(LodestoneAPI.Models.Character character)
        {
            return new Domain.Models.Character()
            {
                Clan = (int)character.Clan,
                DateScraped = DateTime.Now,
                FreeCompanyId = character.FreeCompanyId,
                Gender = (int)character.Gender,
                HighestLevel = character.HighestLevel,
                Id = character.Id,
                Name = character.Name,
                Race = (int)character.Race
            };
        }
    }
}
