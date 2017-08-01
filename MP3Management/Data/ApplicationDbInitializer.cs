using MP3Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MP3Management.Data
{
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var mp3Files = new List<MP3File>
            {
                new MP3File{Name="Black Math", Author="The White Stripes", AlbumName="Elephant", Playlists=new List<Playlist>()},
                new MP3File{Name="No Love Lost", Author="Joy Division", AlbumName="An Ideal For Living", Playlists = new List<Playlist>() },
                new MP3File{Name="Shadowplay", Author="Joy Division", AlbumName="Unknown Pleasures", Playlists=new List<Playlist>()},
                new MP3File{Name="Parabola", Author="Tool", AlbumName="Lateralus", Playlists=new List<Playlist>() },
                new MP3File{Name="It's No Good", Author="Depeche Mode", AlbumName="Ultra", Playlists=new List<Playlist>() },
                new MP3File{Name="Halcyon On and On", Author="Orbital", AlbumName="Single", Playlists=new List<Playlist>() },
                new MP3File{Name="Teardrop", Author="Massive Attack", AlbumName="Mezzanine", Playlists=new List<Playlist>()},
                new MP3File{Name="Angel", Author="Massive Attack", AlbumName="Mezzanine", Playlists=new List<Playlist>()},
                new MP3File{Name="Breathe", Author="The Prodigy", AlbumName="The Fat of the Land", Playlists=new List<Playlist>() },
                new MP3File{Name="Mindfieds", Author="The Prodigy", AlbumName="The Fat of the Land", Playlists=new List<Playlist>() },
                new MP3File{Name="Miss You", Author="Trentemoller", AlbumName="Single", Playlists=new List<Playlist>() },
                new MP3File{Name="Memory Hole", Author="Spacemind", AlbumName="Remix", Playlists=new List<Playlist>() }
            };
            mp3Files.ForEach(s => context.MP3File.Add(s));

            var playlists = new List<Playlist>
            {
                new Playlist{Name="Favourites", Description="Some cool mp3 files", MP3Files = mp3Files.Take(4).Select( x=>x ).ToList() },
                new Playlist{Name="Programming music", Description= "mp3 files for late night programming", MP3Files = mp3Files.Skip(4).Take(7).Select( x=>x ).ToList() },
                new Playlist {Name="Driving", Description = "driving music", MP3Files = mp3Files.Skip(3).Take(8).Select( x=>x ).ToList()},
                new Playlist {Name="Listen Later", Description = "some random mp3 files", MP3Files = mp3Files.Skip(2).Take(6).Select( x=>x ).ToList()}
            };
            playlists.ForEach(s => context.Playlist.Add(s));

            base.Seed(context);
        }
    }
}