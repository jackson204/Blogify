using Blogify.AdminApi.Models;
using Blogify.AdminApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogify.AdminApi.Services
{
    public class ArticleService : IArticleService
    {
        private readonly BlogContext _context;
        public ArticleService(BlogContext context)
        {
            _context = context;
        }

        public async Task<List<ArticleListItemViewModel>> GetArticleListAsync()
        {
            return await _context.Articles
                .Include(a => a.Category)
                .Select(a => new ArticleListItemViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Author = a.Author ?? string.Empty,
                    CategoryName = a.Category != null ? a.Category.Name : "",
                    Status = a.Status.ToString(),
                    Views = a.Views,
                    UpdatedAt = a.UpdatedAt,
                    IsFeatured = a.IsFeatured
                })
                .ToListAsync();
        }
    }
}
