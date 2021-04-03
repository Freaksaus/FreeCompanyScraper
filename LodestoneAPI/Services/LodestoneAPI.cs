using LodestoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LodestoneAPI.Services
{
    public class LodestoneAPI : ILodestoneAPI
    {
        public async Task<Character> GetCharacter(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FreeCompanyEntry>> GetFreeCompanies(string serverName)
        {
            throw new NotImplementedException();
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
