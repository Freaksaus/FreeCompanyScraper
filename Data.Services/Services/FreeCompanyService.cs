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

        public void Update(Domain.Models.FreeCompany freeCompany)
        {
            if (freeCompany == null)
            {
                throw new ArgumentException(nameof(freeCompany));
            }

            var record = (from f in _db.FreeCompanies
                          where f.Id == freeCompany.Id
                          select f).SingleOrDefault();

            if (record == null)
            {
                throw new ArgumentException(nameof(freeCompany));
            }

            record.EstateAddress = freeCompany.EstateAddress;
            record.EstateName = freeCompany.EstateName;
            record.MemberCount = freeCompany.MemberCount;
            record.Name = freeCompany.Name;
            record.DateScraped = freeCompany.DateScraped;
            record.Tag = freeCompany.Tag;

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

        private static Domain.Models.FreeCompany ConvertToDomainModel(Data.Models.FreeCompany freeCompany)
        {
            return new Domain.Models.FreeCompany()
            {
                DateCreated = freeCompany.DateCreated,
                DateScraped = freeCompany.DateScraped,
                EstateAddress = freeCompany.EstateAddress,
                EstateName = freeCompany.EstateName,
                Id = freeCompany.Id,
                MemberCount = freeCompany.MemberCount,
                Name = freeCompany.Name,
                Tag = freeCompany.Tag
            };
        }

        private static Data.Models.FreeCompany ConvertToDataModel(Domain.Models.FreeCompany freeCompany)
        {
            return new Data.Models.FreeCompany()
            {
                DateCreated = freeCompany.DateCreated,
                DateScraped = freeCompany.DateScraped,
                EstateAddress = freeCompany.EstateAddress,
                EstateName = freeCompany.EstateName,
                Id = freeCompany.Id,
                MemberCount = freeCompany.MemberCount,
                Name = freeCompany.Name,
                Tag = freeCompany.Tag
            };
        }
    }
}
