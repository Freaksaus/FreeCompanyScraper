using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodestoneAPI.Models
{
    public class FreeCompanyEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int MemberCount { get; set; }

    }
}
