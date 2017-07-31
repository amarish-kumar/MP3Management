using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MP3Management.Models
{
    public class Playlist
    {
        public Playlist()
        {
            this.MP3Files = new HashSet<MP3File>();
        }
        public int PlaylistID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MP3File> MP3Files { get; set; }
    }
}