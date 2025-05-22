using NewsManagerAPI.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace NewsManagerAPI.Services;

public class NewsService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly NewsApiOptions _options;

    private readonly Dictionary<string, (DateTime fetchedAt, List<NewsItem> articles)> _cache = new();
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public NewsService(HttpClient httpClient, IOptions<NewsApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<List<NewsItem>> GetTopHeadlinesAsync(string country, string category, int pageSize)
    {
        var cacheKey = $"{country}_{category}_{pageSize}";

        // ✅ Перевірка кешу
        if (_cache.TryGetValue(cacheKey, out var cached) &&
            DateTime.UtcNow - cached.fetchedAt < _cacheDuration)
        {
            Console.WriteLine($"🧠 Повертаю з кешу для {cacheKey}");
            return cached.articles;
        }

        var url = $"https://newsapi.org/v2/top-headlines?country={country}&category={category}&pageSize={pageSize}&apiKey={_options.ApiKey}";
        Console.WriteLine("🔗 Запит до NewsAPI: " + url);

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("❌ NewsAPI помилка: " + response.StatusCode);
            return new List<NewsItem>();
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("⚠️ Порожня відповідь від NewsAPI.");
            return new List<NewsItem>();
        }
        Console.WriteLine("📨 Контент відповіді: " + content);
        var result = JsonSerializer.Deserialize<NewsApiResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null)
        {
            Console.WriteLine("❌ Не вдалося десеріалізувати відповідь NewsAPI.");
            return new List<NewsItem>();
        }

        Console.WriteLine("🧾 Status: " + result?.Status);
        Console.WriteLine("🔢 Total: " + result?.TotalResults);
        Console.WriteLine("📚 Кількість статей: " + result?.Articles?.Count);

        var articles = (result?.Status?.ToLower() == "ok") 
            ? result.Articles?.Select(a => new NewsItem
            {
                Title = a.Title,
                Description = a.Description,
                Url = a.Url,
                UrlToImage = a.UrlToImage,
                PublishedAt = a.PublishedAt
            }).ToList()
            : new List<NewsItem>();

        if (articles == null)
        {
            Console.WriteLine("⚠️ NewsAPI повернув null замість списку статей.");
            return new List<NewsItem>();
        }

        // ✅ Кешуємо навіть порожній список (щоб уникнути повторних запитів)
        _cache[cacheKey] = (DateTime.UtcNow, articles);

        return articles;
    }

    public async Task<List<NewsItem>> SearchNewsAsync(string query, string? from = null, string? to = null, string? sortBy = "publishedAt")
    {
        var url = $"https://newsapi.org/v2/everything?q={query}&sortBy={sortBy}&apiKey={_options.ApiKey}";

        if (!string.IsNullOrWhiteSpace(from)) url += $"&from={from}";
        if (!string.IsNullOrWhiteSpace(to)) url += $"&to={to}";

        Console.WriteLine("🔍 Пошук новин: " + url);

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("❌ NewsAPI помилка (пошук): " + response.StatusCode);
            return new List<NewsItem>();
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("⚠️ Порожня відповідь від NewsAPI (пошук).");
            return new List<NewsItem>();
        }
        var result = JsonSerializer.Deserialize<NewsApiResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null)
        {
            Console.WriteLine("❌ Не вдалося десеріалізувати відповідь NewsAPI (пошук).");
            return new List<NewsItem>();
        }

        return result.Articles ?? new List<NewsItem>();
    }
}
        if (articles == null)
        {
            Console.WriteLine("⚠️ NewsAPI повернув null замість списку статей.");
            return new List<NewsItem>();
        }