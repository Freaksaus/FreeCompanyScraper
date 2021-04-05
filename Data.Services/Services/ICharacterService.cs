using System.Collections.Generic;

namespace Data.Services.Services
{
    public interface ICharacterService
    {
        void Add(Domain.Models.Character character);
        Domain.Models.Character Get(string id);
        IEnumerable<Domain.Models.Character> Get();
    }
}
