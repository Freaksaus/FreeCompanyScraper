using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Character
    {
        [Key]
        [Required]
        [MaxLength(30)]
        public string Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string FreeCompanyId { get; set; }
        [Required]
        public int HighestLevel { get; set; }
        [Required]
        public int Race { get; set; }
        [Required]
        public int Clan { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public DateTime DateScraped { get; set; }

    }
}
