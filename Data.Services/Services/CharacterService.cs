using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Services.Services
{
    public class CharacterService : ICharacterService
    {
        private Data.Models.LodestoneContext _db;
        public CharacterService(IServiceScopeFactory serviceScopeFactory)
        {
            _db = serviceScopeFactory.CreateScope().ServiceProvider.GetService<Data.Models.LodestoneContext>();
        }

        public void Add(Domain.Models.Character character)
        {
            if(character == null)
            {
                throw new ArgumentException(nameof(character));
            }

            var entity = ConvertToDataModel(character);
            _db.Characters.Add(entity);
            _db.SaveChanges();
        }

        public Domain.Models.Character Get(string id)
        {
            return (from f in _db.Characters
                    where f.Id == id
                    select ConvertToDomainModel(f)).SingleOrDefault();
        }

        public IEnumerable<Domain.Models.Character> Get()
        {
            return (from f in _db.Characters
                    select ConvertToDomainModel(f)).ToList();
        }

        private static Domain.Models.Character ConvertToDomainModel(Data.Models.Character character)
        {
            return new Domain.Models.Character()
            {
                Clan = character.Clan,
                DateScraped = character.DateScraped,
                FreeCompanyId = character.FreeCompanyId,
                Gender = character.Gender,
                HighestLevel = character.HighestLevel,
                Id = character.Id,
                Name = character.Name,
                Race = character.Race
            };
        }

        private static Data.Models.Character ConvertToDataModel(Domain.Models.Character character)
        {
            return new Data.Models.Character()
            {
                Clan = character.Clan,
                DateScraped = character.DateScraped,
                FreeCompanyId = character.FreeCompanyId,
                Gender = character.Gender,
                HighestLevel = character.HighestLevel,
                Id = character.Id,
                Name = character.Name,
                Race = character.Race
            };
        }
    }
}
