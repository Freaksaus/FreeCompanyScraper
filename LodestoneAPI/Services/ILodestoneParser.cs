using System.Collections.Generic;
using System.Threading.Tasks;

namespace LodestoneAPI.Services
{
    public interface ILodestoneParser
    {
        Task<IEnumerable<Models.FreeCompanyEntry>> ParseFreeCompanies(string html);
    }
}
