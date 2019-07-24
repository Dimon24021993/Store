using System.Collections.Generic;
using Store.Domain.Entities;

namespace Store.BLL.BllModels
{
    public class Pagination
    {
        public bool Desc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 1;
        public int Pages { get; set; } = 1;
        public IEnumerable<Entity> Entities { get; set; }
    }
}