using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Models
{
    [Table("MenuItemRelation")]
    public class MenuItemRelation
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int? MenuItemFatherId { get; set; }
        public MenuItem MenuItemFather { get; set; }
        public int sequence { get; set; }

    }
}
