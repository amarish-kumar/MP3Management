using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MP3Management.Models
{
    public class Playlist
    {
        public Playlist()
        {
            this.MP3Files = new HashSet<MP3File>();
        }
        public int PlaylistID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MP3File> MP3Files { get; set; }
    }
}