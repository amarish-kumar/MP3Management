using MP3Management.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MP3Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MP3File> MP3File { get; set; }
        public DbSet<Playlist> Playlist { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}