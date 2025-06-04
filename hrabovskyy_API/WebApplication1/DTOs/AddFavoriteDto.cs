namespace NewsManagerAPI.DTOs;

public class AddFavoriteDto
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public string TelegramUserId { get; set; } = string.Empty;
}
