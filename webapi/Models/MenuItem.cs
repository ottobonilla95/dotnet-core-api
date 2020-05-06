using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Models
{
    [Table("MenuItem")]
    public class MenuItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Action { get; set; }
    }
}
