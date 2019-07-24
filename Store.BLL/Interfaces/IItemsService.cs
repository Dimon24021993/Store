using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.BLL.BllModels;
using Store.Domain.Entities;

namespace Store.BLL.Interfaces
{
    public interface IItemsService
    {
        Task<Item> GetItemAsync(Guid itemId);
        Task<Item> GetItemAsync(string itemNo);
        IQueryable<Item> GetItems(Pagination pagination);
        Task SetItemRate(Guid id, decimal rate);
        Task SaveItemAsync(Item item);
        Task DeleteItemAsync(Guid itemId);
        Task<IEnumerable<Picture>> GetItemPictures(Guid itemId);
        int TotalCount();
    }
}