using LodestoneAPI.Models;
using System;
using System.Collections.Generic;

namespace LodestoneAPI.Services
{
    public class LodestoneAPI : ILodestoneAPI
    {
        public Character GetCharacter(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FreeCompanyEntry> GetFreeCompanies(string serverName)
        {
            throw new NotImplementedException();
        }

        public FreeCompany GetFreeCompany(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FreeCompanyMemberEntry> GetFreeCompanytMembers(long id)
        {
            throw new NotImplementedException();
        }
    }
}
