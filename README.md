# 📰 NewsFavorites API

> 🧪 Проєкт **у процесі активної розробки**. Структура API може змінюватись. Ваші pull-запити вітаються!

**NewsFavorites API** — це ASP.NET Core Web API, що дозволяє отримувати актуальні новини з публічного API [NewsAPI.org](https://newsapi.org), зберігати улюблені новини користувачів і додавати до них нотатки. Ідеально підходить як основа для Telegram-бота або мобільного застосунку новин.

---

## 🚀 Функціонал

- 🔍 Пошук новин за ключовим словом, країною або категорією
- 💾 Додавання новин до списку улюблених
- 📝 Можливість додати нотатку до збереженої новини
- ✏️ Редагування нотатки до новини
- 🗑️ Видалення улюбленої новини
- 📄 Отримання списку всіх улюблених новин

---

## 🛠 Технології

- ASP.NET Core 7 Web API
- C# / .NET
- NewsAPI.org інтеграція
- Swagger (автоматична документація)
- Dependency Injection (`INewsService`)
- DTO-шари (`AddFavoriteDto`, `UpdateFavoriteNoteDto`)
- Локальна конфігурація `appsettings.json` для API-ключа

---

## 📁 Основні компоненти

| Файл / Клас                 | Призначення |
|----------------------------|-------------|
| `Program.cs`               | Конфігурація сервісів і запуск застосунку |
| `NewsController.cs`        | Отримання новин через NewsAPI |
| `FavoritesController.cs`   | CRUD-операції з улюбленими новинами |
| `NewsService.cs` / `INewsService.cs` | Сервіс взаємодії з NewsAPI |
| `AddFavoriteDto.cs`, `UpdateFavoriteNoteDto.cs` | DTO-моделі для взаємодії |
| `FavoriteNewsItem.cs`, `NewsItem.cs`, `NewsApiResponse.cs` | Моделі даних |
| `NewsApiOptions.cs`        | Конфігурація API-ключа через DI |
| `appsettings.json`         | Зберігання ключа до NewsAPI |
| `launchSettings.json`      | Налаштування запуску проєкту (Swagger, порти) |

---

## 📦 Приклад запитів

```http
GET /api/news?q=technology&country=us
POST /api/favorites
PUT /api/favorites/{id}
DELETE /api/favorites/{id}
GET /api/favorites
```

---

## 🧪 Запуск локально

1. Скопіюйте `.env` або додайте у `appsettings.json`:
```json
{
  "NewsApi": {
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```

2. Запустіть:
```bash
dotnet run
```

3. Відкрийте Swagger:
```
https://localhost:7102/swagger
```

---

---

## 📄 Ліцензія

Цей проєкт ліцензовано під MIT License — див. файл [LICENSE](LICENSE) для деталей.

---

## 💡 Автор
Розроблено як курсовий проєкт студентом у 2025 році.
