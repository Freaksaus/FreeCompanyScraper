using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodestoneAPI.Models
{
    public class Character
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Race Race { get; set; }
        public Clan Clan { get; set; }
        public Gender Gender { get; set; }
        public long FreeCompanyId { get; set; }
    }
}
