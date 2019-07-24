using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.BLL.BllModels
{
    public class EntityWrapper
    {
        public Entity EntityObject { get; set; }

        public CrudOperation Operation { get; set; }
    }
}
