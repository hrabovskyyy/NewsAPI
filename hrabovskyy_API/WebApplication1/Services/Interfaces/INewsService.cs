using NewsManagerAPI.Models;

namespace NewsManagerAPI.Services;

public interface INewsService
{
    Task<List<NewsItem>> GetTopHeadlinesAsync(string country, string category, int pageSize);
    Task<List<Article>> SearchNewsAsync(string query, string? from = null, string? to = null, string? sortBy = "publishedAt");
}
