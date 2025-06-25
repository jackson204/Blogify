using System.ComponentModel.DataAnnotations;
using Blogify.AdminApi.Models;

namespace Blogify.AdminApi.ViewModel
{
    public class ArticleViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public List<Category> CategoryList { get; set; } = new();

        public int? Id { get; set; }

        public string Excerpt { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        [Required]
        public int Category { get; set; }

        public string Tags { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int ReadTime { get; set; } = 5;

        [Url]
        public string Image { get; set; } = string.Empty;

        public string Status { get; set; } = "draft";

        public bool Featured { get; set; }
    }
}
