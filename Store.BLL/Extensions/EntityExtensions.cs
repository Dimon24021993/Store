using System.Linq;
using Store.BLL.BllModels;
using Store.Domain.Entities;

namespace Store.BLL.Extensions
{
    public static class EntityExtensions
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> entities, Pagination pagination) where T : Entity
        {
            entities =
                pagination.Desc ?
                    entities.SkipLast((pagination.Page - 1) * pagination.Size).TakeLast(pagination.Size) :
                    entities.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size);

            return entities;
        }
    }
}