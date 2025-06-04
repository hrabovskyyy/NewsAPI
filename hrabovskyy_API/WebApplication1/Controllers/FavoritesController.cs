using Microsoft.AspNetCore.Mvc;
using NewsManagerAPI.DTOs;
using NewsManagerAPI.Models;

namespace NewsManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private static readonly List<FavoriteNewsItem> Favorites = new();

    [HttpGet("user/{telegramUserId}")]
    public IActionResult GetAllByUser(string telegramUserId)
    {
        var userFavorites = Favorites
            .Where(f => f.TelegramUserId == telegramUserId)
            .ToList();

        return Ok(userFavorites);
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var item = Favorites.FirstOrDefault(f => f.Id == id);
        return item is null
            ? NotFound($"Новину з ID {id} не знайдено.")
            : Ok(item);
    }

    [HttpPost]
    public IActionResult Add([FromBody] AddFavoriteDto dto)
    {
        var item = new FavoriteNewsItem
        {
            Title = dto.Title,
            Url = dto.Url,
            TelegramUserId = dto.TelegramUserId
        };

        Favorites.Add(item);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateNote(Guid id, [FromBody] UpdateFavoriteNoteDto dto)
    {
        var item = Favorites.FirstOrDefault(f => f.Id == id);
        if (item is null)
            return NotFound($"Новину з ID {id} не знайдено.");

        item.Note = dto.Note;
        return NoContent();
    }

    // DELETE /api/favorites/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var item = Favorites.FirstOrDefault(f => f.Id == id);
        if (item is null)
            return NotFound($"Новину з ID {id} не знайдено.");

        Favorites.Remove(item);
        return NoContent();
    }
}
