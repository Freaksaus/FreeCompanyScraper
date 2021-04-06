using System.Collections.Generic;

namespace Data.Services.Services
{
    public interface IFreeCompanyService
    {
        void Add(Domain.Models.FreeCompany freeCompany);
        void Update(Domain.Models.FreeCompany freeCompany);
        Domain.Models.FreeCompany Get(string id);
        IEnumerable<Domain.Models.FreeCompany> Get();
    }
}
