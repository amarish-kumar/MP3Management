using System.Collections.Generic;

namespace MP3Management.Models
{
    public class MP3File
    {
        public MP3File()
        {
            this.Playlists = new HashSet<Playlist>();
        }
        public int MP3FileID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string AlbumName { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}