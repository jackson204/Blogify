using Blogify.AdminApi.ViewModels;

namespace Blogify.AdminApi.Services
{
    public interface IArticleService
    {
        Task<List<ArticleListItemViewModel>> GetArticleListAsync();
    }
}
