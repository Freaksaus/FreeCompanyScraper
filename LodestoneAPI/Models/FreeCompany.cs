using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodestoneAPI.Models
{
    public class FreeCompany
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MemberCount { get; set; }
        public string Tag { get; set; }
        public string EstateName { get; set; }
        public string EstateAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateScraped { get; set; }
    }
}
