using ApplicationFilmsAndSerials.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationFilmsAndSerials.Data
{
    public class FilmsAndSerialsContext : DbContext
    {
        public FilmsAndSerialsContext(DbContextOptions<FilmsAndSerialsContext> options) : base(options) { }

        public DbSet<User>Users { get; set; }
        public DbSet<Films> Films { get; set; }
        public DbSet<Serials> Serials { get; set; }

    }
}
