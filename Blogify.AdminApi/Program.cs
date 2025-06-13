using Blogify.AdminApi.Models;
using Blogify.AdminApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 設定資料庫連線
// 切換為 In-Memory Database
builder.Services.AddDbContext<BlogContext>(options =>
    options.UseInMemoryDatabase("BlogDb"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IArticleService, ArticleService>();

var app = builder.Build();

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BlogContext>();
    SeedData(db);
}

app.Run();


void SeedData(BlogContext db)
{
    var user1 = new User { UserName = "admin", Email = "admin@test.com" };
    var user2 = new User { UserName = "user", Email = "user@test.com" };
    db.Users.AddRange(user1, user2);
    db.SaveChanges(); 

    var cat1 = new Category { Name = "技術" };
    var cat2 = new Category { Name = "生活" };
    db.Categories.AddRange(cat1, cat2);
    db.SaveChanges();

    var tag1 = new Tag { Name = ".NET" };
    var tag2 = new Tag { Name = "C#" };
    var tag3 = new Tag { Name = "隨筆" };
    db.Tags.AddRange(tag1, tag2, tag3);
    db.SaveChanges();

    var art1 = new Article {
        Title = "ASP.NET Core 入門",
        Content = "這是一篇關於 ASP.NET Core 的教學文章。",
        CategoryId = cat1.Id,
        Author = user1.UserName,
        Tags = new List<Tag> { tag1, tag2 },
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        Status = ArticleStatus.Published,
        IsFeatured = true,
        Views = 100,
        ReadingTimeMinutes = 5
    };
    var art2 = new Article {
        Title = "生活隨筆",
        Content = "今天心情不錯，記錄一下。",
        CategoryId = cat2.Id,
        Author = user2.UserName,
        Tags = new List<Tag> { tag3 },
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        Status = ArticleStatus.Published,
        IsFeatured = false,
        Views = 20,
        ReadingTimeMinutes = 2
    };
    db.Articles.AddRange(art1, art2);

    db.SaveChanges();
}
