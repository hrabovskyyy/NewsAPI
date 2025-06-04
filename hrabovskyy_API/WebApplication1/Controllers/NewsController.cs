using Microsoft.AspNetCore.Mvc;
using NewsManagerAPI.Models;
using NewsManagerAPI.Services;

namespace NewsManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private static readonly List<NewsItem> News = new();
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    // 🔹 CRUD: Локальний список новин
    [HttpGet]
    public IActionResult GetAll() => Ok(News);

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var item = News.FirstOrDefault(x => x.Id == id);
        return item is null ? NotFound($"Новина з ID {id} не знайдена.") : Ok(item);
    }

    [HttpPost]
    public IActionResult Add([FromBody] NewsItem item)
    {
        item.Id = News.Count > 0 ? News.Max(x => x.Id) + 1 : 1;
        item.SavedAt = DateTime.UtcNow;
        News.Add(item);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] NewsItem updated)
    {
        var item = News.FirstOrDefault(x => x.Id == id);
        if (item is null) return NotFound($"Новина з ID {id} не знайдена.");

        item.Title = updated.Title;
        item.Description = updated.Description;
        item.Url = updated.Url;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = News.FirstOrDefault(x => x.Id == id);
        if (item is null) return NotFound($"Новина з ID {id} не знайдена.");

        News.Remove(item);
        return NoContent();
    }

    // 🔹 Отримання новин з NewsAPI або fallback
    [HttpGet("public")]
    public async Task<IActionResult> GetFromPublicApi(
        [FromQuery] string country = "ua",
        [FromQuery] string category = "technology",
        [FromQuery] int pageSize = 5)
    {
        if (pageSize is < 1 or > 100)
            return BadRequest(new { error = "pageSize має бути в межах 1..100." });

        Console.WriteLine($"➡️ Запит новин: country={country}, category={category}, pageSize={pageSize}");

        var articles = await _newsService.GetTopHeadlinesAsync(country, category, pageSize);

        if (articles == null || articles.Count == 0)
        {
            Console.WriteLine($"❌ Немає новин для '{country}' → fallback до 'us/general'...");
            articles = await _newsService.GetTopHeadlinesAsync("us", "general", pageSize);

            if (articles == null || articles.Count == 0)
            {
                Console.WriteLine("⚠️ Навіть fallback не дав новин — повертаємо порожній список.");
                return Ok(new { articles = new List<Article>() });
            }
        }

        // 🧾 Завжди повертаємо як об'єкт
        return Ok(new { articles });
    }

    // 🔹 Пошук новин
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromQuery] string? from = null,
        [FromQuery] string? to = null,
        [FromQuery] string? sortBy = "publishedAt")
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { error = "Параметр 'q' (ключове слово для пошуку) обов'язковий." });

        var results = await _newsService.SearchNewsAsync(q, from, to, sortBy);

        return results.Count == 0
            ? NotFound(new { message = "Новини за запитом не знайдено." })
            : Ok(new { articles = results });
    }
}