namespace NewsManagerAPI.Models;

public class FavoriteNewsItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;

    // 🔹 Нове поле для ідентифікації користувача Telegram
    public string TelegramUserId { get; set; } = string.Empty;
}
