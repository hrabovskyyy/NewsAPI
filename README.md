# 📰 NewsAPI

[![.NET](https://img.shields.io/badge/.NET-7.0-blue?logo=dotnet&style=flat-square)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white&style=flat-square)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![NewsAPI](https://img.shields.io/badge/Powered%20by-NewsAPI.org-blue?style=flat-square)](https://newsapi.org)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)

---

**NewsAPI** — це ASP.NET Core Web API, що дозволяє отримувати актуальні новини з публічного API [NewsAPI.org](https://newsapi.org), зберігати улюблені новини користувачів і додавати до них нотатки.  
Ідеально підходить як бекенд для Telegram-бота або мобільного застосунку новин.

---

## 🚀 Функціонал

- 🔎 Пошук новин за ключовим словом, країною або категорією
- 💾 Додавання новин до обраного
- 📝 Додавання / редагування нотаток
- 🗑️ Видалення новин
- 📄 Перегляд усього списку улюблених

---

## 🛠 Технології

| Технологія            | Опис |
|------------------------|------|
| ![.NET](https://img.shields.io/badge/.NET-7.0-purple?logo=dotnet) | Web API платформа |
| ![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white) | Основна мова |
| ![Swagger](https://img.shields.io/badge/Swagger-UI-green?logo=swagger&logoColor=white) | Документація API |
| ![NewsAPI](https://img.shields.io/badge/API-NewsAPI.org-lightblue?logo=rss&logoColor=white) | Джерело новин |
| 💡 Dependency Injection | Через `INewsService` |
| 📦 DTO-шари            | `AddFavoriteDto`, `UpdateFavoriteNoteDto` |
| ⚙️ appsettings.json    | Зберігання ключа API |

---

## 📁 Структура

```
📁 Controllers/
    ├── NewsController.cs
    └── FavoritesController.cs

📁 Services/
    ├── INewsService.cs
    └── NewsService.cs

📁 Models/
    ├── NewsItem.cs
    ├── FavoriteNewsItem.cs
    └── NewsApiResponse.cs

📁 DTOs/
    ├── AddFavoriteDto.cs
    └── UpdateFavoriteNoteDto.cs

📄 Program.cs
📄 appsettings.json
📄 launchSettings.json
```

---

## 🔗 Приклади запитів

```http
GET     /api/news?q=technology&country=us
POST    /api/favorites
PUT     /api/favorites/{id}
DELETE  /api/favorites/{id}
GET     /api/favorites
```

---

## 🧪 Локальний запуск

1. 🔑 Додай API ключ у `appsettings.json`:
```json
{
  "NewsApi": {
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```

2. ▶️ Запусти проєкт:
```bash
dotnet run
```

3. 🌐 Swagger:
```
https://localhost:7102/swagger
```

---

## 📜 Ліцензія

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)  
Цей проєкт ліцензовано під MIT License.

---

## 👨‍💻 Автор

🧑‍🎓 Курсовий проєкт студента КПІ (2025)  
💡 Ідеальний бекенд для Telegram-бота або новинного застосунку.
