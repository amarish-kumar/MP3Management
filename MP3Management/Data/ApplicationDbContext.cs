using MP3Management.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MP3Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer(new ApplicationDbInitializer());
        }
        public DbSet<MP3File> MP3File { get; set; }
        public DbSet<Playlist> Playlist { get; set; }
    }
}