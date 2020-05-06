using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("Album")]
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public IEnumerable<Song> Songs { get; set; }
        public string Image { get; set; }
        public Album()
        {
            this.CreationDate = DateTime.Now;
        }
    }
}
