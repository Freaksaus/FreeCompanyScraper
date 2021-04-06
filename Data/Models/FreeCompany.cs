using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class FreeCompany
    {
        [Key]
        [Required]
        [MaxLength(30)]
        public string Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public int MemberCount { get; set; }
        [Required]
        [MaxLength(10)]
        public string Tag { get; set; }
        [Required]
        [MaxLength(200)]
        public string EstateName { get; set; }
        [Required]
        [MaxLength(40)]
        public string EstateAddress { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateScraped { get; set; }
    }
}
