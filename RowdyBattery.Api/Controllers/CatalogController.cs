using Microsoft.AspNetCore.Mvc;
using RowdyBattery.Data;
using RowdyBattery.Domain.Catalog;
using RowdyBattery.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace RowdyBattery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly StoreContext _context;

        public CatalogController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Catalog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.Include(i => i.Ratings).ToListAsync();
        }

        // GET: api/Catalog/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.Include(i => i.Ratings)
                                           .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();
            return item;
        }

        // POST: api/Catalog
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem([FromBody] CreateItemDto dto)
        {
            var item = new Item { Name = dto.Name, Price = dto.Price };
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        // PUT: api/Catalog/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateItemDto dto)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = dto.Name;
            item.Price = dto.Price;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Catalog/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return NotFound();
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Catalog/{id}/ratings
        [HttpPost("{id}/ratings")]
        public async Task<IActionResult> AddRating(int id, [FromBody] CreateRatingDto dto)
        {
            var item = await _context.Items.Include(i => i.Ratings)
                                           .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();

            try
            {
                // Rating's setters are private; use its public constructor
                var rating = new Rating(dto.Stars, dto.UserName, dto.Review);
                item.Ratings.Add(rating);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (ArgumentException ex)
            {
                // Domain validation failed (e.g., invalid stars)
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
