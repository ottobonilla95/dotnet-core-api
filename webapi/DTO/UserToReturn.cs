using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.DTO
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
    }
}
