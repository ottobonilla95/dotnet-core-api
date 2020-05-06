using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("Song")]
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }
        public Song()
        {
            this.CreationDate = DateTime.Now;
        }
    }
}
