using System;
using System.Collections.Generic;
using LodestoneAPI.Models;

namespace LodestoneAPI.Services
{
    public interface ILodestoneAPI
    {
        public IEnumerable<FreeCompanyEntry> GetFreeCompanies(string serverName);

        public FreeCompany GetFreeCompany(long id);

        public IEnumerable<FreeCompanyMemberEntry> GetFreeCompanytMembers(long id);

        public Character GetCharacter(long id);
    }
}
