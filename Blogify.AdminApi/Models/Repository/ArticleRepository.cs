namespace Blogify.AdminApi.Models.Repository;

public static class ArticleRepository
{

    private static readonly List<Article> _articles =
    [
      
        new Article
        {
            Id = 1,
            Title = "前端開發入門",
            Excerpt = "這是一篇關於前端開發的入門文章。",
            Author = "小明",
            Category = 1,
            Tags = "HTML,CSS,JavaScript",
            ReadTime = 5,
            Image = "https://example.com/image1.jpg",
            Status = "published",
            Featured = true,
            Content = "前端開發是指網頁和應用程式的客戶端部分的開發。",
            Views = 100,
            UpdatedAt = DateTime.Now
        },
        new Article
        {
            Id = 2,
            Title = "後端開發基礎",
            Excerpt = "這是一篇關於後端開發的基礎文章。",
            Author = "小華",
            Category = 2,
            Tags = "Node.js,Express,API",
            ReadTime = 7,
            Image = "https://example.com/image2.jpg",
            Status = "draft",
            Featured = false,
            Content = "後端開發涉及伺服器、資料庫和應用程式邏輯的開發。",
            Views = 50,
            UpdatedAt = DateTime.Now
        },
        new Article
        {
            Id = 3,
            Title = "資料庫設計原則",
            Excerpt = "這是一篇關於資料庫設計的原則文章。",
            Author = "小美",
            Category = 3,
            Tags = "SQL,NoSQL,Database",
            ReadTime = 6,
            Image = "https://example.com/image3.jpg",
            Status = "published",
            Featured = true,
            Content = "資料庫設計是確保資料有效存儲和檢索的關鍵。",
            Views = 75,
            UpdatedAt = DateTime.Now
        },
        new Article
        {
            Id = 4,
            Title = "DevOps實踐",
            Excerpt = "這是一篇關於DevOps實踐的文章。",
            Author = "小強",
            Category = 4,
            Tags = "CI/CD,Automation,DevOps",
            ReadTime = 8,
            Image = "https://example.com/image4.jpg",
            Status = "draft",
            Featured = false,
            Content = "DevOps是一種文化和實踐，旨在提高開發和運營團隊之間的協作。",
            Views = 30,
            UpdatedAt = DateTime.Now
        },
        new Article
        {
            Id = 5,
            Title = "行動開發趨勢",
            Excerpt = "這是一篇關於行動開發趨勢的文章。",
            Author = "小芳",
            Category = 5,
            Tags = "iOS,Android,Mobile",
            ReadTime = 4,
            Image = "https://example.com/image5.jpg",
            Status = "published",
            Featured = true,
            Content = "行動開發是指為智能手機和平板電腦開發應用程式。",
            Views = 120,
            UpdatedAt = DateTime.Now
        },
        new Article
        {
            Id = 6,
            Title = "全端開發概述",
            Excerpt = "這是一篇關於全端開發的概述文章。",
            Author = "小志",
            Category = 1,
            Tags = "Full Stack,Web Development",
            ReadTime = 5,
            Image = "https://example.com/image6.jpg",
            Status = "draft",
            Featured = false,
            Content = "全端開發涉及前端和後端技術的綜合應用。",
            Views = 80,
            UpdatedAt = DateTime.Now
        }
        
        
      
    ];

    public static List<Article> GetArticles()
    {
        return _articles;
    }
}
