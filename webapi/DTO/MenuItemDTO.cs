using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.DTO
{
    public class MenuItemDTO
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public string MenuFatherId { get; set; }
    }
}
