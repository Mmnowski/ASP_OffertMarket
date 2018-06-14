using GASPG.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GASPG.Data
{
    public class GASPGDbContext : IdentityDbContext<AppUser>
    {
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Developer> Developers { get; set; }
        public virtual DbSet<Game> Games { get; set; }

        public GASPGDbContext(DbContextOptions<GASPGDbContext> options) : base(options)
        {
        }


    }
}
