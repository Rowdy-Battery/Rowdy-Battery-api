using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RowdyBattery.Data;
using RowdyBattery.Domain.Catalog;

namespace RowdyBattery.Api.Controllers;

[ApiController]
// Support BOTH route styles:
//  - /catalog
//  - /api/Catalog
[Route("[controller]")]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly StoreContext _db;
    public CatalogController(StoreContext db) => _db = db;

    // ----- DTOs (with validation so [ApiController] can auto-400) -----
    public record CreateItemDto(
        [System.ComponentModel.DataAnnotations.Required] string Name,
        [System.ComponentModel.DataAnnotations.Range(0, double.MaxValue)] decimal Price);

    public record UpdateItemDto(
        [System.ComponentModel.DataAnnotations.Required] string Name,
        [System.ComponentModel.DataAnnotations.Range(0, double.MaxValue)] decimal Price);

    public record CreateRatingDto(
        [System.ComponentModel.DataAnnotations.Range(1, 5)] int Stars,
        [System.ComponentModel.DataAnnotations.Required] string UserName,
        string? Review);

    // ----- GET /catalog  or  /api/Catalog -----
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Items.AsNoTracking().ToListAsync());

    // ----- GET /catalog/{id}  or  /api/Catalog/{id} -----
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne(int id)
    {
        var item = await _db.Items.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    // ----- POST /catalog  -> 201 Created -----
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var nextId = await _db.Items.AnyAsync() ? await _db.Items.MaxAsync(i => i.Id) + 1 : 1;
        var item = new Item(nextId, dto.Name, dto.Price);

        _db.Items.Add(item);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOne), new { id = item.Id }, item);
    }

    // ----- PUT /catalog/{id}  -> 200 or 404 -----
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existing = await _db.Items.FindAsync(id);
        if (existing is null) return NotFound();

        var updated = new Item(id, dto.Name, dto.Price);
        _db.Entry(existing).CurrentValues.SetValues(updated);
        await _db.SaveChangesAsync();

        return Ok(updated);
    }

    // ----- DELETE /catalog/{id}  -> 204 or 404 -----
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item is null) return NotFound();

        _db.Items.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ----- POST /catalog/{id}/ratings  -> 200 or 400 or 404 -----
    [HttpPost("{id:int}/ratings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Rate(int id, [FromBody] CreateRatingDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var item = await _db.Items.Include(i => i.Ratings).FirstOrDefaultAsync(i => i.Id == id);
        if (item is null) return NotFound();

            try
            {
                item.AddRating(new Rating(dto.Stars, dto.UserName, dto.Review ?? string.Empty));
            await _db.SaveChangesAsync();
            return Ok(item);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }
}
