using System;

namespace Domain.Models
{
    public class FreeCompany
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MemberCount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateScraped { get; set; }
    }
}
