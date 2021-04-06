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
            var addedFreeCompanies = _freeCompanyService.Get().Select(f => f.Id).ToHashSet();
            var addedCharacters = _characterService.Get().Select(f => f.Id).ToHashSet();

            //var freeCompanies = await _lodestoneAPI.GetFreeCompanies(_options.ServerName);
            foreach (var freecompanyEntry in _freeCompanyService.Get())
            {
                var freeCompany = await _lodestoneAPI.GetFreeCompany(freecompanyEntry.Id);
                if (!addedFreeCompanies.Contains(freeCompany.Id))
                {
                    _freeCompanyService.Add(ConvertToModel(freeCompany));
                    addedFreeCompanies.Add(freeCompany.Id);
                }
                else
                {
                    _freeCompanyService.Update(ConvertToModel(freeCompany));
                }    

                var members = await _lodestoneAPI.GetFreeCompanyMembers(freeCompany.Id);
                foreach (var member in members)
                {
                    if (!addedCharacters.Contains(member.Id))
                    {
                        var character = await _lodestoneAPI.GetCharacter(member.Id);
                        _characterService.Add(ConvertToModel(character));
                        addedCharacters.Add(member.Id);
                    }
                }
            }
        }

        private Domain.Models.FreeCompany ConvertToModel(LodestoneAPI.Models.FreeCompany freeCompany)
        {
            return new Domain.Models.FreeCompany()
            {
                DateCreated = freeCompany.DateCreated,
                DateScraped = DateTime.Now,
                EstateAddress = freeCompany.EstateAddress,
                EstateName = freeCompany.EstateName,
                Id = freeCompany.Id,
                MemberCount = freeCompany.MemberCount,
                Name = freeCompany.Name,
                Tag = freeCompany.Tag
            };
        }

        private Domain.Models.Character ConvertToModel(LodestoneAPI.Models.Character character)
        {
            return new Domain.Models.Character()
            {
                Clan = (int)character.Clan,
                DateScraped = DateTime.Now,
                FreeCompanyId = character.FreeCompanyId ?? "",
                Gender = (int)character.Gender,
                HighestLevel = character.HighestLevel,
                Id = character.Id,
                Name = character.Name,
                Race = (int)character.Race
            };
        }
    }
}
