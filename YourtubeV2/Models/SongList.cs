using System;
using System.Collections.Generic;
using System.Text;

namespace YourtubeV2.Models
{
    public class SongList
    {
        public bool Select { get; set; }
        public int Id { get; set; }
        public string SongId { get; set; }
        public string SongName { get; set; }
        public string Downloaded { get; set; }
        public int PlaylistId { get; set; }
    }
}
