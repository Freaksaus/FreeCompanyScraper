using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LodestoneAPI.Models;

namespace LodestoneAPI.Services
{
    public interface ILodestoneAPI
    {
        public Task<IEnumerable<FreeCompanyEntry>> GetFreeCompanies(string serverName);

        public Task<FreeCompany> GetFreeCompany(long id);

        public Task<IEnumerable<FreeCompanyMemberEntry>> GetFreeCompanytMembers(long id);

        public Task<Character> GetCharacter(long id);
    }
}
