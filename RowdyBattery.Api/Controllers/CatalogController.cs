using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RowdyBattery.Data;
using RowdyBattery.Domain.Catalog;

namespace RowdyBattery.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly StoreContext _db;
    public CatalogController(StoreContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Items.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var item = await _db.Items.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    public record CreateItemDto(string Name, decimal Price);
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price < 0) return BadRequest();

        var nextId = await _db.Items.AnyAsync() ? await _db.Items.MaxAsync(i => i.Id) + 1 : 1;
        var item = new Item(nextId, dto.Name, dto.Price);
        _db.Items.Add(item);
        await _db.SaveChangesAsync();
        return Created($"/catalog/{item.Id}", item);
    }

    public record UpdateItemDto(string Name, decimal Price);
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemDto dto)
    {
        var item = await _db.Items.FindAsync(id);
        if (item is null) return NotFound();
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price < 0) return BadRequest();

        var updated = new Item(id, dto.Name, dto.Price);
        _db.Entry(item).CurrentValues.SetValues(updated);
        await _db.SaveChangesAsync();
        return Ok(updated);
    }

    public record CreateRatingDto(int Stars, string UserName, string? Review);
    [HttpPost("{id:int}/ratings")]
    public async Task<IActionResult> Rate(int id, [FromBody] CreateRatingDto dto)
    {
        if (dto.Stars < 1 || dto.Stars > 5 || string.IsNullOrWhiteSpace(dto.UserName)) return BadRequest();

        var item = await _db.Items.Include(i => i.Ratings).FirstOrDefaultAsync(i => i.Id == id);
        if (item is null) return NotFound();

        item.AddRating(new Rating(dto.Stars, dto.UserName, dto.Review));
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item is null) return NotFound();
        _db.Items.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
