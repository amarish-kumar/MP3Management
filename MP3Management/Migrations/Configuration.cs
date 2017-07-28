namespace MP3Management.Migrations
{
    using MP3Management.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MP3Management.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MP3Management.Data.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var mp3Files = new List<MP3File>
            {
                new MP3File{Name="No Love Lost", Author="Joy Division", AlbumName="An Ideal For Living", Playlists = new List<Playlist>() },
                new MP3File{Name="Shadowplay", Author="Joy Division", AlbumName="Unknown Pleasures", Playlists=new List<Playlist>()},
                new MP3File{Name="Black Math", Author="The White Stripes", AlbumName="Elephant", Playlists=new List<Playlist>()},
                new MP3File{Name="Parabola", Author="Tool", AlbumName="Lateralus", Playlists=new List<Playlist>() },
                new MP3File{Name="It's No Good", Author="Depeche Mode", AlbumName="Ultra", Playlists=new List<Playlist>() }
            };
            mp3Files.ForEach(s => context.MP3File.Add(s));
            context.SaveChanges();

            var playlists = new List<Playlist>
            {
                new Playlist{Name="Favourites", Description="Some cool mp3 files", MP3Files = new List<MP3File>() },
                new Playlist{Name="Programming music", Description= "mp3 files for late night programming", MP3Files = new List<MP3File>()}
            };
            playlists.ForEach(s => context.Playlist.Add(s));
            context.SaveChanges();
        }
    }
}
