using System.Collections.Generic;

namespace LodestoneAPI.Models
{
    public class FreeCompanySearchResult
    {
        public string SearchText { get; set; }
        public int TotalResults { get; set; }
        public int Page { get; set; }

        public List<FreeCompanyEntry> FreeCompanies { get; set; }
    }
}
