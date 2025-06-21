using Blogify.AdminApi.Data;
using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogContext>(options => 
    options.UseInMemoryDatabase("BlogDb"));
builder.Services.AddScoped<ArticleRepository>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BlogContext>();
    
    // Add Categories
    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { Id = 1, Name = "前端開發" },
            new Category { Id = 2, Name = "後端開發" },
            new Category { Id = 3, Name = "資料庫" },
            new Category { Id = 4, Name = "DevOps" },
            new Category { Id = 5, Name = "行動開發" }
        );
        context.SaveChanges();
    }

    // Add Articles
    if (!context.Articles.Any())
    {
        context.Articles.AddRange(
            new Article
            {
                Title = "前端開發入門",
                Excerpt = "這是一篇關於前端開發的入門文章。",
                Author = "小明",
                CategoryId = 1,
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
                Title = "後端開發基礎",
                Excerpt = "這是一篇關於後端開發的基礎文章。",
                Author = "小華",
                CategoryId = 2,
                Tags = "Node.js,Express,API",
                ReadTime = 7,
                Image = "https://example.com/image2.jpg",
                Status = "draft",
                Featured = false,
                Content = "後端開發涉及伺服器、資料庫和應用程式邏輯的開發。",
                Views = 50,
                UpdatedAt = DateTime.Now.AddDays(-1)
            },
            new Article
            {
                Title = "資料庫設計原則",
                Excerpt = "這是一篇關於資料庫設計的原則文章。",
                Author = "小美",
                CategoryId = 3,
                Tags = "SQL,NoSQL,Database",
                ReadTime = 6,
                Image = "https://example.com/image3.jpg",
                Status = "published",
                Featured = true,
                Content = "資料庫設計是確保資料有效存儲和檢索的關鍵。",
                Views = 75,
                UpdatedAt = DateTime.Now.AddDays(-2)
            }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
