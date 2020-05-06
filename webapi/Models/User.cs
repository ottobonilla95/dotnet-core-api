using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Models
{

    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string ProfileImage { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public User()
        {
            this.CreationDate = DateTime.Now;
        }
    }
}
