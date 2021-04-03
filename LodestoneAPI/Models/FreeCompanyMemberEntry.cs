using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodestoneAPI.Models
{
    public class FreeCompanyMemberEntry
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long FreeCompanyId { get; set; }
    }
}
