using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Services.Services
{
    public class FreeCompanyService : IFreeCompanyService
    {
        private Data.Models.LodestoneContext _db;
        public FreeCompanyService(IServiceScopeFactory serviceScopeFactory)
        {
            _db = serviceScopeFactory.CreateScope().ServiceProvider.GetService<Data.Models.LodestoneContext>();
        }

        public void Add(Domain.Models.FreeCompany freeCompany)
        {
            if(freeCompany == null)
            {
                throw new ArgumentException(nameof(freeCompany));
            }

            var entity = ConvertToDataModel(freeCompany);
            _db.FreeCompanies.Add(entity);
            _db.SaveChanges();
        }

        public Domain.Models.FreeCompany Get(string id)
        {
            return (from f in _db.FreeCompanies
                    where f.Id == id
                    select ConvertToDomainModel(f)).SingleOrDefault();
        }

        public IEnumerable<Domain.Models.FreeCompany> Get()
        {
            return (from f in _db.FreeCompanies
                    select ConvertToDomainModel(f)).ToList();
        }

        private Domain.Models.FreeCompany ConvertToDomainModel(Data.Models.FreeCompany freeCompany)
        {
            return new Domain.Models.FreeCompany()
            {
                DateCreated = freeCompany.DateCreated,
                DateScraped = freeCompany.DateScraped,
                Id = freeCompany.Id,
                MemberCount = freeCompany.MemberCount,
                Name = freeCompany.Name
            };
        }

        private Data.Models.FreeCompany ConvertToDataModel(Domain.Models.FreeCompany freeCompany)
        {
            return new Data.Models.FreeCompany()
            {
                DateCreated = freeCompany.DateCreated,
                DateScraped = freeCompany.DateScraped,
                Id = freeCompany.Id,
                MemberCount = freeCompany.MemberCount,
                Name = freeCompany.Name
            };
        }
    }
}
