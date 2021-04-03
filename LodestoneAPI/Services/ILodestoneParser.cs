using System.Collections.Generic;
using System.Threading.Tasks;

namespace LodestoneAPI.Services
{
    public interface ILodestoneParser
    {
        Task<List<Models.FreeCompanyEntry>> ParseFreeCompanySearchPage(string html);
        Task<List<Models.FreeCompanyMemberEntry>> ParseFreeCompanyMemberPage(string html, string freeCompanyId);
        Task<Models.Character> ParseCharacterPage(string html, string characterId);
    }
}
