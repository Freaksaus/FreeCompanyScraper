using System;

namespace Domain.Models
{
    public class Character
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FreeCompanyId { get; set; }
        public int HighestLevel { get; set; }
        public int Race { get; set; }
        public int Clan { get; set; }
        public int Gender { get; set; }
        public DateTime DateScraped { get; set; }
    }
}
