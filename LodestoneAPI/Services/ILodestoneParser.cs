using System.Collections.Generic;
using System.Threading.Tasks;

namespace LodestoneAPI.Services
{
    public interface ILodestoneParser
    {
        Task<Models.FreeCompanySearchResult> ParseFreeCompanySearchPage(string html, string searchText, int page);
        Task<List<Models.FreeCompanyMemberEntry>> ParseFreeCompanyMemberPage(string html, string freeCompanyId);
        Task<Models.Character> ParseCharacterPage(string html, string characterId);
        Task<Models.FreeCompany> ParseFreeCompanyPage(string html, string freeCompanyId);
    }
}
