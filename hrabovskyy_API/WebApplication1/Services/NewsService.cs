using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using NewsManagerAPI.Models;

namespace NewsManagerAPI.Services;

public class NewsService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly NewsApiOptions _options;
    private readonly ILogger<NewsService> _logger;

    private readonly Dictionary<string, (DateTime fetchedAt, List<NewsItem> articles)> _cache = new();
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public NewsService(HttpClient httpClient, IOptions<NewsApiOptions> options, ILogger<NewsService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<List<NewsItem>> GetTopHeadlinesAsync(string country, string category, int pageSize)
    {
        var cacheKey = $"{country}_{category}_{pageSize}";

        if (_cache.TryGetValue(cacheKey, out var cached) &&
            DateTime.UtcNow - cached.fetchedAt < _cacheDuration)
        {
            _logger.LogInformation("[{Time}] 🧠 Cache hit for {CacheKey}", DateTime.UtcNow, cacheKey);
            return cached.articles;
        }

        var url =
            $"https://newsapi.org/v2/top-headlines?country={country}&category={category}&pageSize={pageSize}&apiKey={_options.ApiKey}";
        _logger.LogInformation("[{Time}] 🔗 Fetching from NewsAPI: {Url}", DateTime.UtcNow, url);

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "MyNewsApp/1.0");

        var response = await _httpClient.SendAsync(request);
        
        var rawContent = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("[{Time}] 📡 Response received: {Content}", DateTime.UtcNow, rawContent);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("[{Time}] ❌ NewsAPI error: {StatusCode}", DateTime.UtcNow, response.StatusCode);
            return new List<NewsItem>();
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.LogWarning("[{Time}] ⚠️ Empty response from NewsAPI", DateTime.UtcNow);
            return new List<NewsItem>();
        }

        _logger.LogDebug("[{Time}] 📨 Response content: {Content}", DateTime.UtcNow, content);

        var result = JsonSerializer.Deserialize<NewsApiResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null)
        {
            _logger.LogError("[{Time}] ❌ Failed to deserialize NewsAPI response", DateTime.UtcNow);
            return new List<NewsItem>();
        }

        _logger.LogInformation("[{Time}] 🧾 Status: {Status}, 🔢 Total: {Total}, 📚 Articles count: {Count}",
            DateTime.UtcNow, result.Status, result.TotalResults, result.Articles?.Count ?? 0);

        var articles = result?.Status?.ToLower() == "ok"
            ? result.Articles?.Select(a => new NewsItem
            {
                Title = a.Title,
                Description = a.Description,
                Url = a.Url,
                Source = a.UrlToImage,
                PublishedAt = a.PublishedAt
            }).ToList()
            : new List<NewsItem>();

        if (articles == null)
        {
            _logger.LogWarning("[{Time}] ⚠️ NewsAPI returned null for articles", DateTime.UtcNow);
            return new List<NewsItem>();
        }

        _cache[cacheKey] = (DateTime.UtcNow, articles);
        return articles;
    }

    public async Task<List<Article>> SearchNewsAsync(string query, string? from = null, string? to = null,
        string? sortBy = "publishedAt")
    {
        var url = $"https://newsapi.org/v2/everything?q={query}&sortBy={sortBy}&apiKey={_options.ApiKey}";

        if (!string.IsNullOrWhiteSpace(from)) url += $"&from={from}";
        if (!string.IsNullOrWhiteSpace(to)) url += $"&to={to}";

        _logger.LogInformation("[{Time}] 🔍 Searching news: {Url}", DateTime.UtcNow, url);

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "MyNewsApp/1.0");

        var response = await _httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("[{Time}] ❌ NewsAPI search error: {StatusCode}", DateTime.UtcNow, response.StatusCode);
            return new List<Article>();
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.LogWarning("[{Time}] ⚠️ Empty response from NewsAPI (search)", DateTime.UtcNow);
            return new List<Article>();
        }

        var result = JsonSerializer.Deserialize<NewsApiResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null)
        {
            _logger.LogError("[{Time}] ❌ Failed to deserialize NewsAPI response (search)", DateTime.UtcNow);
            return new List<Article>();
        }

        return result.Articles ?? new List<Article>();
    }
}
