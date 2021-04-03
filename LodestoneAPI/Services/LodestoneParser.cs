using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodestoneAPI.Services
{
    public class LodestoneParser : ILodestoneParser
    {
        public async Task<IEnumerable<Models.FreeCompanyEntry>> ParseFreeCompanies(string html)
        {
            return new List<Models.FreeCompanyEntry>();
        }
    }
}
