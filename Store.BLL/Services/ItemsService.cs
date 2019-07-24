using Microsoft.EntityFrameworkCore;
using Store.BLL.BllModels;
using Store.BLL.Extensions;
using Store.BLL.Interfaces;
using Store.DAL;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.BLL.Services
{
    public sealed class ItemsService : EntitiesService, IItemsService
    {
        public ItemsService(DataBaseContext context) : base(context) { }

        public async Task<Item> GetItemAsync(Guid itemId)
        {
            var item = await GetItems().FirstOrDefaultAsync(x => x.Id == itemId || x.ItemNo == itemId.ToString());
            return item;
        }

        public async Task<Item> GetItemAsync(string itemNo)
        {
            var item = await GetItems().FirstOrDefaultAsync(x => x.ItemNo == itemNo);
            return item;
        }

        public IQueryable<Item> GetItems(Pagination pagination)
        {
            return GetItems().OrderBy(x => x.Sort).Pagination(pagination);
        }

        public IQueryable<Item> GetItems()
        {
            return context.Items
                          .Include(x => x.Pictures)
                          //.Include(x => x.Rates)
                          //.Include(x => x.ItemGroups)
                          //.ThenInclude(x => x.Group)
                ;
        }


        public async Task SetItemRate(Guid id, decimal rate)
        {
            throw new NotImplementedException();
        }


        public async Task SaveItemAsync(Item item)
        {
            await Save(item);
        }

        public async Task DeleteItemAsync(Guid itemId)
        {
            await Delete<Item>(itemId);
        }

        public async Task<IEnumerable<Picture>> GetItemPictures(Guid itemId)
        {
            return (await GetByIdAsync(itemId, new List<Expression<Func<Item, object>>>
                       {
                           x => x.Pictures
                       })).Pictures;
        }

        public int TotalCount()
        {
            return context.Items.Count();
        }
    }
}