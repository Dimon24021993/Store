using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.BLL.BllModels;
using Store.BLL.Interfaces;
using Store.DAL;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly DataBaseContext db;
        private IItemsService ItemsService { get; set; }
        public ItemsController(DataBaseContext dbContext, IItemsService itemsService)
        {
            db = dbContext;
            ItemsService = itemsService;
        }

        [HttpPost]
        public async Task<ActionResult> GetItemsPagination(Pagination pagination)
        {
            try
            {
                pagination.Entities = await ItemsService.GetItems(pagination).ToListAsync();
                pagination.Pages = Convert.ToInt32(Math.Ceiling(ItemsService.TotalCount() / (pagination.Size * 1.0)));
                return Ok(pagination);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
            return db.Items;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return db.Users;
        }

        [HttpGet]
        public async Task<ActionResult<Item>> GetItem(Guid itemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await ItemsService.GetItemAsync(itemId);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<Item>> GetItemByNo(string itemNo)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(itemNo))
            {
                return BadRequest(ModelState);
            }

            var item = await ItemsService.GetItemAsync(itemNo);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem([FromRoute] Guid id, [FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Items.Add(item);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            return Ok(item);
        }

        private bool ItemExists(Guid id)
        {
            return db.Items.Any(e => e.Id == id);
        }
    }
}