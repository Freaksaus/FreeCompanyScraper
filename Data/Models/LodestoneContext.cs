using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public class LodestoneContext : DbContext
    {
        public LodestoneContext(DbContextOptions<LodestoneContext> options) : base(options)
        {

        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<FreeCompany> FreeCompanies { get; set; }
    }
}
