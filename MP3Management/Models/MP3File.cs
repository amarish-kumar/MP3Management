using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MP3Management.Models
{
    public class MP3File
    {
        public MP3File()
        {
            this.Playlists = new HashSet<Playlist>();
        }
        public int MP3FileID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string AlbumName { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}